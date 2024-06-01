using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayer : MonoBehaviour
{
    public GameObject player1;  // Reference to the first player
    public GameObject player2;  // Reference to the second player
    public GameObject mark1;
    public GameObject mark2;
    public float placementRadius = 1.0f;  // Acceptance radius for placement

    private bool isPlayer1Placed = false;
    private bool isPlayer2Placed = false;

    public bool startCooking = false;
    public GameObject Text_Lets_Cook;


    //--- AUDIO PART ---
    private AudioSource backgroundMusicAudio;  // Reference to the AudioSource of the background music
    public AudioClip cookingMusicClip;

    // Audio sources of the players
    private AudioSource player1AudioSource;
    private AudioSource player2AudioSource;

    public AudioClip placementClip;

    void Start()
    {
        Text_Lets_Cook.SetActive(true);

        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusic");
        if (backgroundMusicObject != null)
        {
            backgroundMusicAudio = backgroundMusicObject.GetComponent<AudioSource>();
        }


        player1AudioSource = player1.GetComponent<AudioSource>();
        player2AudioSource = player2.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isPlayer1Placed)
        {
            isPlayer1Placed = CheckPlacement(player1.transform, mark1.transform, "Player 1", player1AudioSource);

        }

        if (!isPlayer2Placed)
        {
            isPlayer2Placed = CheckPlacement(player2.transform, mark2.transform, "Player 2", player2AudioSource);

        }

        if (isPlayer1Placed && isPlayer2Placed)
        {
            OnPlayersPlaced();
        }
    }

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
        mark1.SetActive(false);
        mark2.SetActive(false);
        Text_Lets_Cook.SetActive(false);
        startCooking = true;

        // Stop the background music
        if (backgroundMusicAudio != null && cookingMusicClip != null)
        {
            backgroundMusicAudio.clip = cookingMusicClip;
            backgroundMusicAudio.volume = 1;
            backgroundMusicAudio.Play();
        }


    }
}
