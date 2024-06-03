using UnityEngine;
using UnityEngine.UI;

public class RotationTarget : MonoBehaviour
{
    public float rotationSpeed;  //Speed of rotation
    public GameObject pivotPizza; //Pivot point for rotation (Pizza Uncooked GameObject)
    public TextMesh timerText; //UI text for the timer
    public PlacePlayer placePlayerScript; //Reference to PlacePlayer script


    // --- Audio Part ---
    public AudioClip wellDoneClip; //Audio clip "well done"
    public AudioSource timerAudioSource; //Audio source for the timer

    private float activationDelay = 27.0f; // Duration of the timer before "Well Done" message
    private float timer = 0.0f; // Timer to track 
    private PizzaRotation pizzaRotation; // Reference to PizzaRotation script
    private AudioSource audioSource; // Reference to the AudioSource component
    private bool wellDoneAudioPlayed = false; // Ensure audio plays once



    private void Start()
    {
        pizzaRotation = FindObjectOfType<PizzaRotation>(); // Find the object PizzaRotation
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    void Update()
    {
        // Calculate the angle between the target (white point) and the pizza
        Vector3 playerToPizza = pivotPizza.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, playerToPizza, Vector3.up);

        // Check if the cooking process has started
        if (placePlayerScript != null && placePlayerScript.startCooking)
        {
            // Rotate the target around the pizza
            RotateObject(-1*angle * rotationSpeed * Time.deltaTime);

            // Update the timer
            timer += Time.deltaTime;
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));


            if (timer >= activationDelay)
            {
                // Display "WELL DONE" and stop rotation
                timerText.text = "WELL DONE";
                rotationSpeed = 0f;
                pizzaRotation.shouldRotate = false;

                // Play the "Well Done" audio and stop the timer audio
                PlayWellDoneAudio();
                timerAudioSource.Stop();
            }

        }
    }

    // --- Rotation of the target around the pivotPizza ---
    void RotateObject(float angle)
    {
        Vector3 pivotToObject = transform.position - pivotPizza.transform.position;
        transform.RotateAround(pivotPizza.transform.position, Vector3.up, angle);

    }


    // --- Play the "Well Done" audio clip ---
    void PlayWellDoneAudio()
    {
        if (!wellDoneAudioPlayed && wellDoneClip != null)
        {
            audioSource.clip = wellDoneClip;
            audioSource.Play();
            wellDoneAudioPlayed = true;
        }
    }


}
