using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player 
{
    public Vector3 velocity;
    public Vector3 gravityDir = new Vector3(0, 1, 0);
    public Vector3 dampeningDir;

    public float gravity = 20.0f;
    public float drag;
    public float maxSpeed = 40.0f;

    public void ClampSpeed()
    {
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }
    public void ApplyGravity()
    {
        velocity -= gravityDir * gravity * Time.deltaTime;
    }

    public void ApplyDamping()
    {
        dampeningDir = -velocity;
        dampeningDir *= drag;
        velocity += dampeningDir;
    }



}
