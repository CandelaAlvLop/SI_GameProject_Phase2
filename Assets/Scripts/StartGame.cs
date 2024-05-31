using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public AudioClip introAudioClip; // The audio clip to play
    public string sceneToLoad; // The name of the scene to load
    public float audioDelay = 3f; // The delay before the audio starts

    private AudioSource audioSource;
    private BoxCollider boxCollider;

    private void Start()
    {
        // Get the AudioSource component and the BoxCollider component
        audioSource = gameObject.AddComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();

        // Initially disable the BoxCollider
        boxCollider.enabled = false;

        // Start the coroutine to play the audio after a delay
        StartCoroutine(PlayAudioAfterDelay());
    }

    private IEnumerator PlayAudioAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(audioDelay);

        // Play the audio
        audioSource.clip = introAudioClip;
        audioSource.Play();

        // Wait for the audio to finish playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Enable the BoxCollider after the audio finishes
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Change to the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
