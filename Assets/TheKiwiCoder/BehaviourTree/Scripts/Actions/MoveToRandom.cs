using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {
public class MoveToRandom : ActionNode {

    public float speed = 1.0f;
    public float minDistance = 0.0f;
    public float maxDistance = 10.0f;
    public bool respectToPlayer = false;
    public float accelerate = 0.0f;

    void Awake() {
        // never calls this?
        context.agent.angularSpeed = 0;
        context.agent.updateRotation = false;
        context.agent.updateUpAxis = false;
        Debug.Log("start!");
    }

    protected override State OnUpdate() {
        if (!context.agent.pathPending && context.agent.remainingDistance < 0.5f) {
            return State.Success;
        }
        return State.Running;
    }

    protected override void OnStart() {
        context.agent.isStopped = false;
        context.agent.speed = speed;
        if (accelerate > 0) {
            context.agent.acceleration = accelerate;
            context.agent.autoBraking = true;
        } else {
            context.agent.acceleration = speed;
            context.agent.autoBraking = false;
        }
        
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        if (respectToPlayer) {
            randomDirection += Player.instance.transform.position;
        } else {
            randomDirection += context.transform.position;
        }
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxDistance, 1);
        Vector3 finalPosition = hit.position;
        context.agent.SetDestination(finalPosition);
    }

    protected override void OnStop() {
        context.agent.isStopped = true;
    }

}

}