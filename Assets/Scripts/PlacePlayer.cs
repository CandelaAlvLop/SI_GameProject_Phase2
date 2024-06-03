using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayer : MonoBehaviour
{
    public GameObject player1;  // Reference to the first player
    public GameObject player2;  // Reference to the second player

    // Reference to players mark
    public GameObject mark1;
    public GameObject mark2;
    public float placementRadius = 1.0f;  // Acceptance radius for placement


    // Flags to check if players are placed
    private bool isPlayer1Placed = false;
    private bool isPlayer2Placed = false;

    public bool startCooking = false;

    // Reference to the "Let's Cook" text
    public GameObject Text_Lets_Cook;

    //--- AUDIO PART ---
    private AudioSource backgroundMusicAudio;  // Reference to the AudioSource of the background music
    private AudioSource timerAudioSource;

    // Audio sources of the players
    private AudioSource player1AudioSource;
    private AudioSource player2AudioSource;

    public AudioClip placementClip;

    //Pizza Box Collider
    BoxCollider boxCollider;

    void Start()
    {
        Text_Lets_Cook.SetActive(true);

        // Find and get the BoxCollider component from the "Pizza Uncooked" game object
        GameObject pizza = GameObject.Find("Pizza Uncooked");
        boxCollider = pizza.GetComponent<BoxCollider>();

        //--- Audio Part ---
        // Find and get the AudioSource component from the background music object
        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusic");
        if (backgroundMusicObject != null)
        {
            backgroundMusicAudio = backgroundMusicObject.GetComponent<AudioSource>();
        }

        // Find and get the AudioSource component from the timer gameobject
        GameObject timerObject = GameObject.Find("Timer Audio");

        if (timerObject != null) timerAudioSource = timerObject.GetComponent<AudioSource>();

        // Get the AudioSource components from the player game objects
        player1AudioSource = player1.GetComponent<AudioSource>();
        player2AudioSource = player2.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if player 1 is placed
        if (!isPlayer1Placed)
        {
            isPlayer1Placed = CheckPlacement(player1.transform, mark1.transform, "Player 1", player1AudioSource);
        }

        // Check if player 2 is placed
        if (!isPlayer2Placed)
        {
            isPlayer2Placed = CheckPlacement(player2.transform, mark2.transform, "Player 2", player2AudioSource);
        }

        // If both players are placed, trigger the players placed event
        if (isPlayer1Placed && isPlayer2Placed)
        {
            OnPlayersPlaced();
        }
    }


    // ----  Check if the player is placed within the placement radius of the mark ----
    bool CheckPlacement(Transform player, Transform mark, string playerName, AudioSource playerAudioSource)
    {
        float distance = Vector3.Distance(player.position, mark.position);
        if (distance <= placementRadius)
        {
            playerAudioSource.clip = placementClip;
            playerAudioSource.Play();

            return true;
        }
        return false;
    }

    void OnPlayersPlaced()
    {
        // Deactivate the marks and the "Let's Cook" text
        mark1.SetActive(false);
        mark2.SetActive(false);
        Text_Lets_Cook.SetActive(false);

        // Set startCooking to true to start the cooking process
        startCooking = true;

        // Stop the background music if it is playing
        if (backgroundMusicAudio != null) backgroundMusicAudio.Stop();

        // Play the timer audio if it is not already playing
        if (!timerAudioSource.isPlaying) timerAudioSource.Play();
    }
}
