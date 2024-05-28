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


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (other.CompareTag("Crate") && !hasPlayedSound)
        {
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
                hasPlayedSound = true; 
            }
        }

        if (other.CompareTag("Pizza"))
        {
            PizzaRotation pizzaRotation = other.GetComponent<PizzaRotation>();
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

        PizzaRotation pizzaRotation = other.GetComponent<PizzaRotation>();
        if (pizzaRotation != null)
        {
            pizzaRotation.shouldRotate = false;
        }
    }


}
