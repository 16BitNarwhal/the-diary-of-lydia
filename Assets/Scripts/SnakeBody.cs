using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour {
    
    private float health;
    private SnakeBody previousBodyPart;
    private Vector3 velocity = Vector3.zero;

    void Start() {

    }

    void FixedUpdate() {
        if (previousBodyPart != null) {
            Vector3 direction = previousBodyPart.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position = Vector3.SmoothDamp(transform.position, previousBodyPart.transform.position, ref velocity, 1f, 1f);
            Debug.Log(transform.position);
        }
    }
    
    public void SetPreviousBodyPart(SnakeBody previousBodyPart) {
        this.previousBodyPart = previousBodyPart;
    }

}
