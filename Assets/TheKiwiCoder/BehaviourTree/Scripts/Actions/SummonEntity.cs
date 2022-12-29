using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class SummonEntity : ActionNode
{

    public GameObject entityPrefab;
    public float maxRandomDistance = 0;

    protected override void OnStart() {
        Vector2 position = context.transform.position;
        position += Random.insideUnitCircle * maxRandomDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas)) {
            position = hit.position;
        }
        Object.Instantiate(entityPrefab, position, Quaternion.identity);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
