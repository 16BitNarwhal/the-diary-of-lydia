using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    
    private static AudioSource audioSource;

    public static void LoadScene(string sceneName)
    {
        audioSource = Player.instance.GetComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Assets/Audio/Book Page Turn 12.mp3");
        audioSource.PlayOneShot(clip, 1.0f);

        Debug.Log("Loading scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
