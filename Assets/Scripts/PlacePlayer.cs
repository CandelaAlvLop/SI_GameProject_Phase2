using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayer : MonoBehaviour
{
    public Transform player1;  // Referencia al primer jugador
    public Transform player2;  // Referencia al segundo jugador
    public GameObject mark1;
    public GameObject mark2;
    public float placementRadius = 1.0f;  // Radio de aceptación para colocación

    private bool isPlayer1Placed = false;
    private bool isPlayer2Placed = false;

    public bool startCooking = false;


    public GameObject Text_Lets_Cook;

    private PluginConnector connector;

    public AudioClip newBackgroundMusic; // New clip audio for the timer
    private AudioSource backgroundMusicAudio;


    void Start()
    {
        Text_Lets_Cook.SetActive(true);
        
        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusic");
        backgroundMusicAudio = backgroundMusicObject.GetComponent<AudioSource>();


        connector = FindObjectOfType<PluginConnector>();
    }


    void Update()
    {
        if (!isPlayer1Placed)
        {
            isPlayer1Placed = CheckPlacement(player1, mark1.transform, "Player 1");
        }

        if (!isPlayer2Placed)
        {
            isPlayer2Placed = CheckPlacement(player2, mark2.transform, "Player 2");
        }

        if (isPlayer1Placed && isPlayer2Placed)
        {
            OnPlayersPlaced();
        }
    }

    bool CheckPlacement(Transform player, Transform mark, string playerName)
    {
        float distance = Vector3.Distance(player.position, mark.position);
        Debug.Log($"{playerName} distance to mark: {distance}");
        if (distance <= placementRadius)
        {
            Debug.Log($"{playerName} is correctly placed at {mark.name}");
            
            return true;
        }
        return false;
    }


    void OnPlayersPlaced()
    {
        Debug.Log("Both players are correctly placed!");

        //Set to False because the cook part start
        mark1.SetActive(false);
        mark2.SetActive(false);

        //player1.position = mark1.transform.position;
        //player2.position = mark2.transform.position;
        Text_Lets_Cook.SetActive(false);
        startCooking = true;

        connector.enableRotation = true;

        if (backgroundMusicAudio != null && newBackgroundMusic != null)
        {
            backgroundMusicAudio.volume = 1;
            backgroundMusicAudio.clip = newBackgroundMusic;
            backgroundMusicAudio.Play(); // Avvia la riproduzione della nuova clip audio
            
        }

    }




}

    