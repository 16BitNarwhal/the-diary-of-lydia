using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems stored here
    public class Context {
        public GameObject gameObject;
        public Transform transform;
        public Animator animator;
        public Rigidbody rigidbody;
        public NavMeshAgent agent;
        public Enemy script; // all BTs should have an enemy script
        public GameObject holding; // only applies by Hannah

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.rigidbody = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.script = gameObject.GetComponent<Enemy>();
            context.animator = gameObject.GetComponent<Animator>();

            // if no context.animator, try find on child
            if (context.animator == null) {
                context.animator = gameObject.GetComponentInChildren<Animator>();
            }

            return context;
        }
    }
}