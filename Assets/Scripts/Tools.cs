using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static Vector3 TurnLeft(Vector3 direction, int turnRadius)
    {
        //Apply a negative rotation to the cursor 
        Vector3 newDirection = Quaternion.Euler(0, turnRadius, 0) * direction.normalized;
        Debug.Log(newDirection);
        return newDirection;
    }

    public static Vector3 TurnRight(Vector3 direction, int turnRadius)
    {
        //Apply a positive rotation to the cursor
        Vector3 newDirection = Quaternion.Euler(0, -turnRadius, 0) * direction.normalized;
        Debug.Log(newDirection);
        return newDirection;
    }
}
