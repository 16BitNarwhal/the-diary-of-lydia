using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlayAnimation : ActionNode
{

    public string animationName;
    public bool waitUntilFinished = false;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.animator.Play(animationName);
        // if (waitUntilFinished) {
        //     if (context.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
        //         return State.Running;
        //     }
        // }
        return State.Success;
    }
}
