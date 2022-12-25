using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    class Marker {
        public Vector3 position;
        public Quaternion rotation;

        public Marker(Vector3 position, Quaternion rotation) {
            this.position = position;
            this.rotation = rotation;
        }

        public Marker(GameObject obj) {
            this.position = obj.transform.position;
            this.rotation = obj.transform.rotation;
        }
    }

    public float speed = 5f; // The speed at which the snake moves
    public GameObject bodyPrefab; // The prefab for the body segments
    public int bodyLength = 5; // The starting length of the snake's body
    public int gap = 1; // The gap between each body segment

    // A list of the body segments
    private List<GameObject> bodySegments = new List<GameObject>();
    private List<Marker> path = new List<Marker>();
    private Marker previousMarker;

    void Start() {
        path.Add(new Marker(gameObject));

        // Create the initial body segments
        for (int i = 0; i < bodyLength; i++) {
            AddBodySegment();
        }
    }


    const float updateInterval = 0.1f;
    void FixedUpdate() {
        // update the path if the snake moves
        if (Vector3.Distance(transform.position, path[0].position) >= updateInterval) {
            // get rotation of first body segment towards head
            if (bodySegments.Count > 0) {
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * (transform.position - bodySegments[0].transform.position);
                Quaternion rotation = Quaternion.LookRotation(forward:Vector3.forward, upwards:rotatedVectorToTarget);
                path.Add(new Marker(transform.position, rotation));
            }
        }

        // update body segments
        for (int i = 0; i < bodySegments.Count; i++) {
            GameObject segment = bodySegments[i];
            int markerIdx = path.Count - ((i+1)*gap) - 1;
            markerIdx = Mathf.Max(markerIdx, 0);
            
            // update position + rotation
            segment.transform.position = path[markerIdx].position;
            Transform sprite = segment.transform.Find("sprite");
            sprite.transform.rotation = path[markerIdx].rotation;
        }

        // remove unnecessary markers
        if (path[0].position != bodySegments[bodySegments.Count-1].transform.position) {
            path.RemoveAt(0);
        }
    }

    // Adds a body segment to the snake
    void AddBodySegment() {
        // Instantiate a new body segment
        GameObject segment = Instantiate(bodyPrefab, transform.position, transform.rotation);

        // Set the segment as a child of the snake
        segment.transform.parent = transform;

        // Add the segment to the list of body segments
        bodySegments.Add(segment);
    }
}