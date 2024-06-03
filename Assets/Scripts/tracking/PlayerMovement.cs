using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion q;
    public bool manual;

    // Audio variables for collision sound
    public AudioClip collisionSound;
    private AudioSource audioSource;

    // Flag to prevent repeated sound playing
    private bool hasPlayedSound = false;


    // Reference to the PlacePlayer script
    public PlacePlayer place;


    // Reference to the PickingObject script
    private PickingObject pickingObject;

    // Reference to the PizzaRotation script
    private PizzaRotation pizzaRotation;

    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        // Get the PickingObject component attached to the GameObject
        pickingObject = GetComponent<PickingObject>();

        // Ensure PlacePlayer reference is set
        if (place == null) place = FindObjectOfType<PlacePlayer>();
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPosition(Vector3 pos)
    {
        //swith playerIndex
        transform.position = pos;
    }

    public void setRotation(Quaternion quat)
    {
        Matrix4x4 mat = Matrix4x4.Rotate(quat);
        transform.localRotation = quat;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("TAG: " + other.tag);

        // Check if the collided object is a "Crate" and if the collision sound has not been played
        if (other.CompareTag("Crate") && !hasPlayedSound && pickingObject.ingredientInstance == null)
        {
            // Play the collision sound if available
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
               
                // Set to true to prevent repeated sound playing
                hasPlayedSound = true; 
            }
        }


        // Check if the collided object is a "Pizza" and if cooking has started
        if (other.CompareTag("Pizza") && place != null && place.startCooking)
        {
            // Get the PizzaRotation component from the collided object
            pizzaRotation = other.GetComponent<PizzaRotation>();
            if (pizzaRotation != null)
            {
                // Set the flag to start rotating the pizza
                pizzaRotation.shouldRotate = true;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exited object is a "Crate"
        if (other.CompareTag("Crate"))
        {
            // Reset the flag to allow sound playing on next collision
            hasPlayedSound = false;
        }

        // Check if the exited object is a "Pizza"
        if (other.CompareTag("Pizza") && pizzaRotation != null)
        {
            pizzaRotation.shouldRotate = false; // Stop rotating the pizza
            pizzaRotation = null; // Clear the reference to the PizzaRotation component
        }
    }


}
