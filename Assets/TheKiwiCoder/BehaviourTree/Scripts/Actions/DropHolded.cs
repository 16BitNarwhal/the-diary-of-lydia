using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class DropHolded : ActionNode
{
    protected override void OnStart() {
        Debug.Log("DROP!");
        foreach (Transform child in context.transform) {
            Object.Destroy(child.gameObject);
        }
        if (context.holding != null) {
            context.holding.SetActive(true);
            context.holding.transform.position = context.transform.position;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
