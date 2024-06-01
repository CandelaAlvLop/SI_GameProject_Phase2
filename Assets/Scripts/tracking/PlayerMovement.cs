using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion q;
    public bool manual;

    public AudioClip collisionSound;
    private AudioSource audioSource;
    private bool hasPlayedSound = false;

    public PlacePlayer place;

    private PickingObject pickingObject;

    private PizzaRotation pizzaRotation;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (other.CompareTag("Crate") && !hasPlayedSound && pickingObject.ingredientInstance == null)
        {
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
                hasPlayedSound = true; 
            }
        }

        //Debug.Log(place.startCooking);
        if (other.CompareTag("Pizza") && place != null && place.startCooking)
        {
            pizzaRotation = other.GetComponent<PizzaRotation>();
            if (pizzaRotation != null)
            {
                pizzaRotation.shouldRotate = true;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            hasPlayedSound = false;
        }

        if (other.CompareTag("Pizza") && pizzaRotation != null)
        {
            pizzaRotation.shouldRotate = false;
            pizzaRotation = null;
        }
    }


}
