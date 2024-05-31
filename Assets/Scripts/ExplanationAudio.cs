using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found on this GameObject");
        }
    }
}
