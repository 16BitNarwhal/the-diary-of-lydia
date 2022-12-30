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
    public int maxEntities = 5;

    private List<GameObject> entities = new List<GameObject>();
    protected override void OnStart() {
        // check if we have reached the max number of entities
        entities.RemoveAll(item => item == null);
        if (entities.Count >= maxEntities) {
            return;
        }

        // add new randomly placed entity
        Vector2 position = context.transform.position;
        position += Random.insideUnitCircle * maxRandomDistance;
        NavMeshHit hit;
        // get NavMesh "walkable" area
        
        if (NavMesh.SamplePosition(position, out hit, 10, 1)) {
            position = hit.position;
        }
        entities.Add(Object.Instantiate(entityPrefab, position, Quaternion.identity));
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
