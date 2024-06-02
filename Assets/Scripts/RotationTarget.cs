using UnityEngine;
using UnityEngine.UI;

public class RotationTarget : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject pivotPizza;
    public GameObject pizzaUncooked;
    public TextMesh timerText;
    public PlacePlayer placePlayerScript;

    public AudioClip wellDoneClip;
    //public AudioClip timerTickClip;

    private float activationDelay = 30.0f;
    private float timer = 0.0f;
    private PizzaRotation pizzaRotation;
    private AudioSource audioSource; // Reference to the AudioSource component
    private bool wellDoneAudioPlayed = false;
    //private bool timerAudioPlaying = false;



    private void Start()
    {
        pizzaRotation = FindObjectOfType<PizzaRotation>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        Vector3 playerToPizza = pivotPizza.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, playerToPizza, Vector3.up);

        if (placePlayerScript != null && placePlayerScript.startCooking)
        { 
            RotateObject(angle * rotationSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));
            //PlayTimerTickAudio();


            if (timer >= activationDelay)
            {
                timerText.text = "WELL DONE";
                rotationSpeed = 0f;
                pizzaRotation.shouldRotate = false;

                PlayWellDoneAudio();
            }

        }
    }

    void RotateObject(float angle)
    {
        Vector3 pivotToObject = transform.position - pivotPizza.transform.position;
        transform.RotateAround(pivotPizza.transform.position, Vector3.up, angle);

        // Play the audio clip
        //if (!audioSource.isPlaying) // Check if the audio is not already playing
        //{
        //    audioSource.Play();
        //}
    }

    void PlayWellDoneAudio()
    {
        AudioSource audioSource = FindObjectOfType<AudioSource>();
        if (!wellDoneAudioPlayed && wellDoneClip != null)
        {
            audioSource.clip = wellDoneClip;
            audioSource.Play();
            wellDoneAudioPlayed = true;
        }
    }

    //void PlayTimerTickAudio()
    //{
    //    AudioSource audioSource = FindObjectOfType<AudioSource>();
    //    if (timerTickClip != null)
    //    {
    //        audioSource.clip = timerTickClip;
    //        audioSource.loop = true; 
    //        audioSource.Play();
    //        timerAudioPlaying = true;
    //    }
    //}
}
