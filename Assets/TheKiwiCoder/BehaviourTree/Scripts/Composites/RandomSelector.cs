using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheKiwiCoder {
    [System.Serializable]
    public class RandomSelector : CompositeNode {
        
        public bool useWeights = false;
        public List<int> weights;
        public bool useMarkov = false;

        [System.Serializable]
        public class Row {
            public int[] weights;
        }
        public Row[] markov;

        protected int currentChild;

        protected override void OnStart() {
            if (useWeights && useMarkov) {
                Debug.LogError("RandomSelector: cannot use both weights and markov chains");
                return;
            }
            
            if (useWeights) {
                if (weights.Count != children.Count) {
                    Debug.LogError("RandomSelector: weights count does not match children count");
                    return;
                }
                getWeightedRandom();
                return;
            }
            
            if (useMarkov) {
                if (markov.Length != children.Count) {
                    Debug.LogError("RandomSelector: markov count does not match children count");
                    return;
                }
                for (int i=0;i<markov.Length;i++) {
                    if (markov[i].weights.Length != children.Count) {
                        Debug.LogError("RandomSelector: markov count does not match children count");
                        return;
                    }
                }
                getMarkovRandom();
                return;
            }
            this.currentChild = Random.Range(0, children.Count);
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

        private void getMarkovRandom() {
            int total = markov[currentChild].weights.Sum();
            int random = Random.Range(0, total);
            int current = 0;
            for (int i = 0; i < markov[currentChild].weights.Length; i++) {
                current += markov[currentChild].weights[i];
                if (random < current) {
                    this.currentChild = i;
                    return;
                }
            }
        }
    }
}