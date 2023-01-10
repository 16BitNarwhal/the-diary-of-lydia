using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class BounceAttack : ActionNode
{

    public float xSpeed = 8f;
    public float ySpeed = 15f;
    public float duration = 5f;

    int xDir = -1;
    int yDir = -1;
    float startTime;
    protected override void OnStart() {
        context.agent.enabled = false;
        startTime = Time.time;
        xDir = Random.Range(0, 2)==0 ? -1 : 1;
        yDir = Random.Range(0, 2)==0 ? -1 : 1;
    }

    protected override void OnStop() {
        context.agent.enabled = true;
    }
    float yBounds = 4.2f;
    float xBounds = 8.2f;
    protected override State OnUpdate() {
        if (Time.time - startTime > duration) {
            return State.Success;
        }
        
        if (context.transform.position.x > xBounds && xDir == -1)
            xDir = 1;
        else if (context.transform.position.x < -xBounds && xDir == 1)
            xDir = -1;
        if (context.transform.position.y > yBounds && yDir == 1)
            yDir = -1;
        else if (context.transform.position.y < -yBounds && yDir == -1)
            yDir = 1;
        
        context.transform.localScale = new Vector3(xDir * Mathf.Abs(context.transform.localScale.x), 
                context.transform.localScale.y, context.transform.localScale.z);

        context.transform.position += Vector3.left * xDir * xSpeed * Time.deltaTime;
        context.transform.position += Vector3.up * yDir * ySpeed * Time.deltaTime;

        return State.Running;
    }
}
