using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHorizontalDir : MonoBehaviour {

    [SerializeField] private bool startFaceLeft = false;
    private UnityEngine.AI.NavMeshAgent agent;
    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent == null) 
            agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();

    }

    void Update() {
        int direction = startFaceLeft ? -1 : 1;
        if (agent.velocity.x < 0.1f) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x)*direction, transform.localScale.y, transform.localScale.z);
        } else if (agent.velocity.x > 0.1f) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*direction, transform.localScale.y, transform.localScale.z);
        }
    }
}
