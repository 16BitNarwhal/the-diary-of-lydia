using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    [System.Serializable]
    public class FiniteRepeat : DecoratorNode {

        public int repeatCount = 1;
        public bool randomize = false;

        public int repeatMin = 1;
        public int repeatMax = 1;

        public bool restartOnSuccess = true;
        public bool restartOnFailure = false;

        private int currentCount;
        protected override void OnStart() {
            currentCount = 0;

            if (randomize) {
                repeatCount = Random.Range(repeatMin, repeatMax);
            }
        }

        protected override void OnStop() {

        }

        protected override State OnUpdate() {
            State childState = child.Update();

            if (restartOnSuccess && childState == State.Success ||
                restartOnFailure && childState == State.Failure) {
                    currentCount++;
                    if (currentCount < repeatCount) {
                        return State.Running;
                    }
                    return childState;
            }
            return State.Running;
        }
    }

    
}
