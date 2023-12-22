using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlaySound : ActionNode
{
    public List<AudioClip> sounds;
    public float volume = .5f;

    protected override void OnStart() {
        AudioClip sound = sounds[Random.Range(0, sounds.Count)];
        context.audioSource.PlayOneShot(sound, volume);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
