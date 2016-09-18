using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController1 : MonoBehaviour
{
    public GameObject PlayerBullet;
    public float Speed = 10.0f;
    public float BulletSpeed = 10.0f;
    public float JumpForce = 900.0f;
    public float BoostForce = 5.0f;
    public float GunCoolDown = 0.5f;
    public float HookSpeed = 80.0f;
    public float LineSpeed = 90.0f;
    public float ClimbSpeed = 30.0f;
    public float ClimbSlowDownForce = 20.0f;
    public float JetFireDelay = 0.2f;
    public float JetPower = 7.0f;
    public float FuelTime = 2.0f;
    public float RechargeTime = 3.0f;
    public GameObject GrappleArmEnd;
    public LayerMask RopeLayerMask;
    [HideInInspector]
    public bool playerStarted;
    public delegate void OnPlayerStartedEvent();
    public event OnPlayerStartedEvent OnPlayerStarted;
    public delegate void OnPlayerDiedEvent();
    public event OnPlayerDiedEvent OnPlayerDied;
    public delegate void OnPlayerWonEvent();
    public event OnPlayerWonEvent OnPlayerWon;
    public ParticleSystem GunImpact;
    public ParticleSystem MuzzleFlash;
    public GameObject MuzzleEnd;
    public Light MuzzleFlashLight;
    [HideInInspector]
    public float DistanceTraveled;

    private Vector3 playerStartPosition;
    private Animator anim;
    private GameObject body;
    private GameObject grappleShoulder;
    private GameObject turret;
    private Rigidbody playerRigidbody;
    private bool grounded = false;
    private GameObject wallHookGraphic;
    private LineRenderer ropeLineRenderer;
    private List<float> ropeBendAngles = new List<float>();
    private List<Vector3> lineRenderPositions = new List<Vector3>();
    private GameObject wallHook;
    private FixedJoint wallHookFixedJoint;
    private Vector3 wallHookHitPosition = new Vector3();
    private bool hookActive = false;
    private bool wallHookOut = false;
    private bool hooked = false;
    private Vector3 hookPrepStartPosition;
    private Vector3 hookPrepEndPosition;
    private Vector3 playerPreviousPosition;
    private AudioSource playerAudio;
    private AudioClip GunHitSoundEffect;
    private AudioClip GunFireSoundEffect;
    private float MuzzleFlashRange;
    private bool firing = false;
    private bool jetting = false;
    private float currentFuelTime;

    void Awake()
    {
        playerStartPosition = transform.position;
        body = transform.FindChild("Body").gameObject;
        playerRigidbody = GetComponent<Rigidbody>();
        turret = body.transform.FindChild("Turret").gameObject;
        playerAudio = GetComponent<AudioSource>();
        GunFireSoundEffect = Resources.Load("SoundEffects/GunFire") as AudioClip;
        GunHitSoundEffect = Resources.Load("SoundEffects/GunHit") as AudioClip;
        currentFuelTime = FuelTime;
    }

    public void Init()
    {
        StopAllCoroutines();
        playerRigidbody.isKinematic = true;
        transform.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        lineRenderPositions.Clear();
        hookActive = false;
        wallHookOut = false;
        hooked = false;
        currentFuelTime = FuelTime;
        transform.position = playerStartPosition;
        playerRigidbody.detectCollisions = false;
    }

    void Update()
    {  
       
    }

    void FixedUpdate()
    {
        if ((Input.GetAxisRaw("FireHorizontal") != 0.0f || Input.GetAxisRaw("FireVertical") != 0.0) && firing == false)
        {
            firing = true;
            StartCoroutine("FireGun");
        }

        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            Vector3 v3 = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            transform.GetComponent<Rigidbody>().AddForce(Speed * v3.normalized * Time.deltaTime);
        }

        if (Input.GetButton("Jump"))
        {
            Debug.Log("boosting");
            playerRigidbody.AddForce(new Vector3(0, JetPower, 0), ForceMode.Acceleration);
            //if (!jetting)
            //{
            //    StartCoroutine("FireJets");
            //}
        }

        DoDebugDrawing();
    }

    IEnumerator FireGun()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("FireHorizontal"), 0.0f, Input.GetAxisRaw("FireVertical"));
        float timePassed = 0.0f;
        Vector3 origin = new Vector3(turret.transform.position.x,
                                     turret.transform.position.y,
                                     turret.transform.position.z + -turret.transform.position.z);
        Quaternion originalRotation = turret.transform.rotation;

        playerAudio.PlayOneShot(GunFireSoundEffect);
        //MuzzleFlash.Emit(1);
        //StartCoroutine("GunFlash");
        
        //if (Physics.Raycast(origin, direction.normalized, out GunHit, Mathf.Infinity))
        //{
        //    //ParticleSystem.EmitParams emitDirection = new ParticleSystem.EmitParams();
        //    //emitDirection.velocity = Vector3.Reflect(direction, GunHit.normal);
        //    //emitDirection.position = GunHit.point;
        //    if (GunHit.collider.transform.parent.GetComponent<EnemyAI>() != null)
        //    {
        //        //playerAudio.PlayOneShot(GunHitSoundEffect);
        //        GunHit.collider.transform.parent.GetComponent<EnemyAI>().TakeDamage(1.0f);
        //    }
        //    //GunImpact.Emit(emitDirection, 1);
        //}

        while (timePassed < GunCoolDown)
        {
            float percentageComplete = timePassed / GunCoolDown;
            //if(direction != Vector3.zero)
            turret.transform.rotation = Quaternion.Slerp(originalRotation, Quaternion.LookRotation(direction, Vector3.up), percentageComplete);
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }

        turret.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        playerAudio.PlayOneShot(GunFireSoundEffect);
        Instantiate(MuzzleFlash, MuzzleEnd.transform.position, Quaternion.identity);
        GameObject bullet = (GameObject)Instantiate(PlayerBullet, MuzzleEnd.transform.position, MuzzleEnd.transform.rotation);
        bullet.transform.parent = transform.root;
        bullet.GetComponent<Rigidbody>().AddForce(direction * BulletSpeed, ForceMode.VelocityChange);
        firing = false;
        Debug.Log("fire");
        yield return null;
    }

    IEnumerator GunFlash()
    {
        float timePassed = 0.0f;
        while (timePassed < MuzzleFlash.startLifetime / 2)
        {
            float percentageComplete = timePassed / (MuzzleFlash.startLifetime / 2);
            MuzzleFlashLight.range = Mathf.Lerp(0, MuzzleFlashRange, percentageComplete);
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }
        timePassed = 0.0f;
        while (timePassed < MuzzleFlash.startLifetime / 2)
        {
            float percentageComplete = timePassed / (MuzzleFlash.startLifetime / 2);
            MuzzleFlashLight.range = Mathf.Lerp(MuzzleFlashRange, 0, percentageComplete);
            timePassed = timePassed + Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    IEnumerator ShootHook(Vector3 location)
    {
        hookActive = true;
        float timePassed = 0;
        // This is code for sending hook out in mid air, just keeping it around
        //Vector3 hookEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(HookPlayerInput.GetPlayerTouchPosition().x,
        //                                                                  HookPlayerInput.GetPlayerTouchPosition().y,
        //                                                                -(Camera.main.transform.position.z + transform.position.z)));

        wallHookGraphic.transform.parent = null;
        wallHookGraphic.transform.position = new Vector3(wallHookGraphic.transform.position.x, wallHookGraphic.transform.position.y, wallHookGraphic.transform.position.z + -wallHookGraphic.transform.position.z);
        var dist = Vector3.Distance(wallHookGraphic.transform.position, location);
        float timeTakenDuringLerp = dist / HookSpeed;
        while (timePassed < timeTakenDuringLerp)
        {
            float percentageComplete = timePassed / timeTakenDuringLerp;
            wallHookGraphic.transform.position = Vector3.Lerp(wallHookGraphic.transform.position,
                                                        location,
                                                        percentageComplete);
            timePassed += Time.deltaTime;
            yield return null;
        }
        wallHookGraphic.transform.position = location;
        wallHookOut = true;
        hookActive = false;
    }

    IEnumerator ShootRope(Vector3 location)
    {
        hookActive = true;
        ropeLineRenderer.enabled = true;
        float elapsedTime = 0;
        Vector3 ropeEndPoint = new Vector3();
        Vector3 origin = new Vector3(grappleShoulder.transform.position.x, grappleShoulder.transform.position.y, grappleShoulder.transform.position.z);
        var dist = Vector3.Distance(origin, wallHookHitPosition);
        float timeTakenDuringLerp = dist / HookSpeed;
        ropeLineRenderer.SetVertexCount(2);
        while (elapsedTime < timeTakenDuringLerp)
        {
            float percentageComplete = elapsedTime / timeTakenDuringLerp;
            ropeEndPoint = Vector3.Lerp(origin, location, percentageComplete);
            ropeLineRenderer.SetPosition(0, grappleShoulder.transform.position);
            ropeLineRenderer.SetPosition(1, ropeEndPoint);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        lineRenderPositions.Add(wallHookGraphic.transform.position);
        wallHook.transform.position = ropeEndPoint;
        wallHookFixedJoint.connectedBody = transform.GetComponent<Rigidbody>();
        hooked = true;
        hookActive = false;
    }

    IEnumerator RetrieveHookRope()
    {
        // have to fix the line coming back
        wallHookFixedJoint.connectedBody = null;
        hooked = false;
        hookActive = true;
        wallHookOut = false;
        grounded = true;
        float elapsedTime = 0;
        float dist;
        Vector3 startPosition = new Vector3();
        Vector3 endPosition = new Vector3();
        if (lineRenderPositions.Count > 1)
        {
            dist = Vector3.Distance(lineRenderPositions[0], lineRenderPositions[1]);
            startPosition = lineRenderPositions[0];
            endPosition = lineRenderPositions[1];
        }
        else
        {
            dist = Vector3.Distance(lineRenderPositions[0], GrappleArmEnd.transform.position);
            startPosition = lineRenderPositions[0];
            endPosition = GrappleArmEnd.transform.position;
        }
        float timeTakenDuringLerp = dist / HookSpeed;
        while (elapsedTime < timeTakenDuringLerp)
        {
            // retrieve rope
            float percentageComplete = elapsedTime / timeTakenDuringLerp;
            //Debug.Log("percentage complete: " + percentageComplete + "   elapsed time: " + elapsedTime + "   line render position: " + lineRenderPositions[0] + "time taken: " + timeTakenDuringLerp);
            lineRenderPositions[0] = Vector3.Lerp(startPosition, endPosition, percentageComplete);

            // retrieve hook
            wallHookGraphic.transform.position = lineRenderPositions[0];

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        lineRenderPositions.RemoveAt(0);
        if (lineRenderPositions.Count > 0)
            StartCoroutine(RetrieveHookRope());
        else
        {
            ropeLineRenderer.enabled = false;
            wallHookGraphic.transform.position = GrappleArmEnd.transform.position;
            wallHookGraphic.transform.parent = GrappleArmEnd.transform;

        }
        hookActive = false;
    }

    //IEnumerator FireJets()
    //{
    //    while (currentFuelTime > 0.0f)
    //    {
    //        playerRigidbody.AddForce(new Vector3(0, 10, 0), ForceMode.Acceleration);
    //    }
    //    yield return null;
    //}

    IEnumerator ClimbRope()
    {
        grounded = false;
        hookActive = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;
        float scale = 0.1f;
        Vector3 midBezierPoint = wallHookGraphic.transform.position - transform.position;
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ |
                                                          RigidbodyConstraints.FreezeRotationX |
                                                          RigidbodyConstraints.FreezeRotationY;
        midBezierPoint.Normalize();
        Vector3 midPoint = (transform.position + (scale * midBezierPoint)) + transform.GetComponent<Rigidbody>().velocity * 0.2f;
        wallHookFixedJoint.connectedBody = null;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        var dist = Vector3.Distance(transform.position, wallHookGraphic.transform.position);
        float timeTakenDuringLerp = dist / ClimbSpeed;

        while (elapsedTime < timeTakenDuringLerp)
        {
            playerPreviousPosition = transform.position;
            float percentageComplete = elapsedTime / timeTakenDuringLerp;
            transform.position = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, percentageComplete), Vector3.Lerp(midPoint, wallHookGraphic.transform.localPosition, percentageComplete), percentageComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //        LockPlayerPosition();
        lineRenderPositions.Clear();
        ropeLineRenderer.SetVertexCount(0);
        hookActive = false;
    }

    bool CheckHookHit()
    {

        //RaycastHit wallHit = new RaycastHit();
        //bool hit;
        //Vector2 direction = new Vector3(HookPlayerInput.GrappleDirection.x,
        //                                HookPlayerInput.GrappleDirection.y,
        //                                grappleShoulder.transform.position.z + -grappleShoulder.transform.position.z);

        //Vector3 origin = new Vector3(grappleShoulder.transform.position.x, grappleShoulder.transform.position.y, grappleShoulder.transform.position.z + -grappleShoulder.transform.position.z);
        //Debug.DrawRay(origin, direction * 10.0f, Color.yellow, 10.0f);
        //hit = Physics.Raycast(origin, direction, out wallHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"));
        //HookPlayerInput.GrappleDirection = new Vector2();
        //if (hit)
        //{
        //    wallHookHitPosition = wallHit.point + wallHit.normal.normalized * 0.1f;
        //    return true;
        //}
        //else
        return false;
    }

    IEnumerator PlayerDied()
    {
        OnPlayerDied();
        yield return null;
    }

    IEnumerator PlayerWon()
    {
        OnPlayerWon();
        yield return null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBullet")
        {
            playerStarted = false;
            StartCoroutine("PlayerDied");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ |
                                                              RigidbodyConstraints.FreezeRotationX |
                                                              RigidbodyConstraints.FreezeRotationY |
                                                              RigidbodyConstraints.FreezeRotationZ;
            if (hooked)
            {
                wallHookFixedJoint.connectedBody = null;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ |
                                                              RigidbodyConstraints.FreezeRotationX |
                                                              RigidbodyConstraints.FreezeRotationY;
            if (hooked)
            {
                wallHookFixedJoint.connectedBody = transform.GetComponent<Rigidbody>();
            }
        }
    }

    float AngleFromAToB(Vector3 angleA, Vector3 angleB)
    {
        Vector3 axis = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(angleA, angleB);
        float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(angleA, angleB)));

        // angle in [-179,180]
        float signed_angle = angle * sign;
        return signed_angle;
    }

    float GetRopeDistance()
    {
        float distance = 0.0f;
        for (int i = 0; i <= lineRenderPositions.Count - 1; i++)
        {
            if (i < lineRenderPositions.Count - 1)
                distance += Vector3.Distance(lineRenderPositions[i], lineRenderPositions[i + 1]);
            else
                distance += Vector3.Distance(lineRenderPositions[i], transform.position);

        }
        return distance;
    }

    void DoDebugDrawing()
    {
        if (hooked)
        {

        }
    }
}
