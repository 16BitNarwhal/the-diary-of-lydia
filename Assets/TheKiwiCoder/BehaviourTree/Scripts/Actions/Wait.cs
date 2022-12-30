using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    [System.Serializable]
    public class Wait : ActionNode {

        public float duration = 1;
        public bool random = false;
        public float minDuration = 1, maxDuration = 3;
        
        float waitTime;
        float startTime;

        protected override void OnStart() {
            startTime = Time.time;
            if (random) waitTime = Random.Range(minDuration, maxDuration);
            else waitTime = duration;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            
            float timeRemaining = Time.time - startTime;
            if (timeRemaining > waitTime) {
                return State.Success;
            }
            return State.Running;
        }
    }
}
