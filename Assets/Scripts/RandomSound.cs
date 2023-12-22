using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{

    public List<AudioClip> sounds;
    public float volume = 1.0f;
    public float minWait = 3.0f, maxWait = 5.0f;
    
    private AudioSource source;
    private float lastPlayTime = 0.0f;
    private float waitTime;
    
    void Start() {
        source = GetComponent<AudioSource>();
        if (source==null) source = gameObject.AddComponent<AudioSource>();
        waitTime = Random.Range(minWait, maxWait);
    }

    void Update() {
        if (Time.time - lastPlayTime > waitTime) {
            Debug.Log("PLAY!");
            AudioClip sound = sounds[Random.Range(0, sounds.Count)];
            source.PlayOneShot(sound, volume);
            lastPlayTime = Time.time;
        }
    }
}
