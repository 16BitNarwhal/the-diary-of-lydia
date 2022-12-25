using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHorizontalDir : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent agent;
    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent == null) 
            agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();

    }

    void Update() {
        if (agent.velocity.x < 0) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if (agent.velocity.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
