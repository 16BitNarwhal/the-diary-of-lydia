using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IfPlayerTrapped : DecoratorNode
{

    public bool isTrue = true;
    public bool returnSuccessOnFalse = true;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (Player.instance.IsTrapped() == isTrue) {
            return child.Update();
        }
        if (returnSuccessOnFalse) return State.Success;
        else return State.Failure;
    }
}
