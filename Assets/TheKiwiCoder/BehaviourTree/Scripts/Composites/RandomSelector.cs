using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheKiwiCoder {
    [System.Serializable]
    public class RandomSelector : CompositeNode {
        
        public bool useWeights = false;
        public List<int> weights;

        protected int currentChild;

        void Awake() {

        }

        protected override void OnStart() {
            if (useWeights) {
                if (weights.Count != children.Count) {
                    Debug.LogError("RandomSelector: weights count does not match children count");
                    return;
                }
                getWeightedRandom();
            } else {
                this.currentChild = Random.Range(0, children.Count);
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            var child = children[currentChild];
            return child.Update();
        }

        private void getWeightedRandom() {
            int total = weights.Sum();
            int random = Random.Range(0, total);
            int current = 0;
            for (int i = 0; i < weights.Count; i++) {
                current += weights[i];
                if (random < current) {
                    this.currentChild = i;
                    return;
                }
            }
        }
    }
}