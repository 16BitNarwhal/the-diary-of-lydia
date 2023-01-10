using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class HorizontalAttack : ActionNode
{

    public float speed = 10f;
    
    Vector2 startPosition;
    int direction = 0;
    bool crossedBorder;
    protected override void OnStart() {
        crossedBorder = false;
        direction = Random.Range(0, 2);
        context.agent.enabled = false;
        startPosition = context.transform.position;
    }

    protected override void OnStop() {
        context.agent.enabled = true;
    }

    int bounds = 12;
    protected override State OnUpdate() {
        if (direction == 0) {
            context.transform.position += Vector3.left * speed * Time.deltaTime;
            context.transform.localScale = new Vector3(Mathf.Abs(context.transform.localScale.x), 
                    context.transform.localScale.y, context.transform.localScale.z);
        } else {
            context.transform.position += Vector3.right * speed * Time.deltaTime;
            context.transform.localScale = new Vector3(-Mathf.Abs(context.transform.localScale.x), 
                    context.transform.localScale.y, context.transform.localScale.z);
        }
        if (context.transform.position.x < -bounds && direction == 0) {
            context.transform.position = new Vector3(bounds, context.transform.position.y, context.transform.position.z);
            crossedBorder = true;
        } else if (context.transform.position.x > 10 && direction == 1) {
            context.transform.position = new Vector3(-bounds, context.transform.position.y, context.transform.position.z);
            crossedBorder = true;
        }
        if (crossedBorder && Vector2.Distance(context.transform.position, startPosition) < 0.5f) {
            return State.Success;
        }
        return State.Running;
    }
}
