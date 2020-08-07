using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwingPendulum 
{
    public Tether tether;
    public Web web;
    public Player player;
    public Transform playerTr;
    Vector3 prevPos;


    public void Initialize()
    {
        playerTr.transform.parent = tether.tetherTr;
        web.length = Vector3.Distance(playerTr.localPosition, tether.tetherPos);

    }

    public Vector3 MovePlayer(Vector3 pos, Vector3 prevPos, float time)
    {
        player.velocity += VeloctiyConstraint(pos, prevPos, time);
        player.ApplyGravity();
        player.ApplyDamping();
        player.ClampSpeed();

        pos += player.velocity * time;

       
        if (Vector3.Distance(pos, tether.tetherPos) < web.length)
        {
            pos = Vector3.Normalize(pos - tether.tetherPos) * web.length;
            web.length = (Vector3.Distance(pos, tether.tetherPos));
            return pos;
        }

        prevPos = pos;

        return pos;
    }
    //public Vector3 MovePlayer(Vector3 pos, Vector3 prevPosition, float time)
    //{
    //    player.velocity += VeloctiyConstraint(pos, prevPosition, time);
    //    player.ApplyGravity();
    //    player.ApplyDamping();
    //    player.ClampSpeed();

    //    pos += player.velocity * time;


    //    if (Vector3.Distance(pos, tether.tetherPos) < web.length)
    //    {
    //        pos = Vector3.Normalize(pos - tether.tetherPos) * web.length;
    //        web.length = (Vector3.Distance(pos, tether.tetherPos));
    //        return pos;
    //    }

    //    prevPos = pos;

    //    return pos;
    //}

    public Vector3 VeloctiyConstraint(Vector3 currentPos, Vector3 prevPos, float time)
    {
        float distancetoTether = Vector3.Distance(currentPos, tether.tetherPos);
        Vector3 contstraintPos;
        Vector3 futurePos;

        if (distancetoTether > web.length)
        {
            contstraintPos = Vector3.Normalize(currentPos - tether.tetherPos) * web.length;
            futurePos = (contstraintPos - prevPos) / time;
            return futurePos;
        }
        return Vector3.zero;
    }

    public void SwitchTether(Vector3 newPos)
    {
        playerTr.transform.parent = null;
        tether.tetherTr.position = newPos;
        playerTr.transform.parent = tether.tetherTr;
        tether.tetherPos = tether.tetherTr.InverseTransformPoint(newPos);
        web.length = Vector3.Distance(playerTr.transform.localPosition, tether.tetherPos);
    }

    public Vector3 Fall(Vector3 pos, float time)
    {
        player.ApplyDamping();
        player.ApplyGravity();
        player.ClampSpeed();

        pos += player.velocity * time;
        return pos;
    }
}
