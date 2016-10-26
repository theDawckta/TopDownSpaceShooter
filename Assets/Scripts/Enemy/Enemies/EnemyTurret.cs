using UnityEngine;
using System.Collections;

public class EnemyTurret : EnemyAI
{
    public GameObject TurretBody;
    public GameObject TurretBarrelBody;
    public GameObject TurretBarrel;
    public GameObject TurretBullet;
    public float BulletSpeed = 10.0f;

    private RaycastHit turretHitInfo;
    private Transform player;

    void Update()
    {
        // Handle logic for state changes
        if (CheckForPlayer() && states == ENEMY_STATE.IDLE)
        {
            player = turretHitInfo.transform;
            states = ENEMY_STATE.ATTACK;
            //Debug.Log("ATTACKING...");
        }
        else
        {
            //Debug.Log("IDLING...");
        }
    }

    IEnumerator HandleIdle()
    {
        // Enter state code
        player = null;

        while (states == ENEMY_STATE.IDLE)
        {
            //Debug.Log("starting idle");
            // During state code
            int randomRotation = Random.Range(90, -90);
            float movementTime = Random.Range(0.5f,1.0f);
            float nextMovementTime = Random.Range(1.0f, 2.0f);
            float timePassed = 0.0f;
            Quaternion originalRotation = TurretBarrelBody.transform.rotation;

            while (timePassed < nextMovementTime && states == ENEMY_STATE.IDLE)
            {
                timePassed = timePassed + Time.deltaTime;
                yield return null;
            }
            timePassed = 0.0f;
            while (timePassed < movementTime && states == ENEMY_STATE.IDLE)
            {
                TurretBarrelBody.transform.rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0.0f, randomRotation, 0.0f), timePassed / movementTime);
                timePassed = timePassed + Time.deltaTime;
                yield return null;
            }
            timePassed = 0.0f;
            //Debug.Log("ending idle");
            yield return null;
        }
        // Exit state code
        //Debug.Log("State Changed");
    }

    IEnumerator HandleAttack()
    {
        float nextShotTime = Random.Range(0.5f, 0.5f);
        float shotInterval = Random.Range(0.1f, 0.5f);
        float findSpeed = Random.Range(0.5f, 1.0f);
        int numberOfShots = Random.Range(3, 5);
        Quaternion newRotation;
        Vector3 direction;

        player = turretHitInfo.transform;

        while (states == ENEMY_STATE.ATTACK)
        {
            float timePassed = 0.0f;
            
            for (int i = 0; i < numberOfShots; i++)
            {
                while (timePassed < shotInterval)
                {
                    timePassed = timePassed + Time.deltaTime;
                    yield return null;
                }
                direction = TurretBarrel.transform.position - TurretBarrelBody.transform.position;
                GameObject bullet = (GameObject)Instantiate(TurretBullet, TurretBarrel.transform.position, TurretBarrel.transform.rotation);
                bullet.transform.parent = transform.root;
                Physics.IgnoreCollision(TurretBody.GetComponent<Collider>(), bullet.GetComponent<Collider>());
                Physics.IgnoreCollision(TurretBarrelBody.GetComponent<Collider>(), bullet.GetComponent<Collider>());
                Physics.IgnoreCollision(TurretBarrel.GetComponent<Collider>(), bullet.GetComponent<Collider>());
                bullet.GetComponent<Rigidbody>().AddForce(direction * BulletSpeed, ForceMode.VelocityChange);
                timePassed = 0.0f;
            }
            while (timePassed < findSpeed)
            {
                //find the vector pointing from our position to the target
                Vector3 _direction = (player.transform.position - TurretBarrelBody.transform.position).normalized;

                //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(_direction, Vector3.forward);
                _lookRotation.x = 0.0f;
                _lookRotation.y = 0.0f;
                //rotate us over time according to speed until we are in the required rotation
                TurretBarrelBody.transform.rotation = Quaternion.Slerp(TurretBarrelBody.transform.rotation, _lookRotation, timePassed / findSpeed);
                timePassed = timePassed + Time.deltaTime;
                yield return null;
            }
            if (!CheckForPlayer())
                states = ENEMY_STATE.IDLE;
            while (timePassed < nextShotTime)
            {
                newRotation = Quaternion.LookRotation(player.position - TurretBarrelBody.transform.position, Vector3.forward);
                newRotation.x = 0.0f;
                newRotation.y = 0.0f;
                TurretBarrelBody.transform.rotation = newRotation;
                timePassed = timePassed + Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator HandleDead()
    {
        Destroy(gameObject);
        yield return null;
    }

    private bool CheckForPlayer()
    {
        Vector3 direction = TurretBarrel.transform.position - TurretBarrelBody.transform.position;
        Debug.DrawRay(TurretBarrelBody.transform.position, direction * 10.0f, Color.yellow);
        if (Physics.Raycast(TurretBarrelBody.transform.position, direction, out turretHitInfo, Mathf.Infinity))
        {
            if (turretHitInfo.collider.tag == "Player")
                return true;
            else
                return false;
        }
        else
            return false;
    }
}


