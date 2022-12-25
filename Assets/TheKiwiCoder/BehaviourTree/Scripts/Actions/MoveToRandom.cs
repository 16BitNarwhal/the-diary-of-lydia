using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
public class MoveToRandom : ActionNode {

    public float speed = 1.0f;
    public float maxDistance = 10.0f;

    void Awake() {
        // never calls this?
        context.agent.angularSpeed = 0;
        context.agent.updateRotation = false;
        context.agent.updateUpAxis = false;
        context.agent.speed = speed;
    }

    protected override State OnUpdate() {
        if (!context.agent.pathPending && context.agent.remainingDistance < 0.5f) {
            return State.Success;
        }
        return State.Running;
    }

    protected override void OnStart() {
        context.agent.isStopped = false;
        
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection += context.transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, maxDistance, 1);
        Vector3 finalPosition = hit.position;
        context.agent.SetDestination(finalPosition);
    }

    protected override void OnStop() {
        context.agent.isStopped = true;
    }

}

}