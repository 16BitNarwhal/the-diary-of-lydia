using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    public float speed = 5f;
    
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // set pos to 0,0
        transform.position = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        // move 4D based on player input (horizontal & vertical)
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        rb.velocity = new Vector2(input.x, input.y) * speed; 
    }
}
