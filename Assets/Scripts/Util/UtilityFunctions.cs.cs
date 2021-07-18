using UnityEngine;

public static class UtilityFunctions 
{
	public static float AngleFromAToB(Vector3 angleA, Vector3 angleB)
    {
        if (angleA == Vector3.zero || angleB == Vector3.zero)
            return 0.0f;
        Vector3 axis = new Vector3(0, 1, 0);
        float angle = Vector3.Angle(angleA, angleB);
        float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(angleA, angleB)));

        // angle in [-179,180]
        float signed_angle = angle * sign;
        return signed_angle;
    }

    public static float GetRandomNumber(float minimum, float maximum, System.Random random)
    {
        return (float)random.NextDouble() * (maximum - minimum) + minimum;
    }
}
