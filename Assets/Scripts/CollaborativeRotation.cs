using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollaborativeRotation : MonoBehaviour {


    //void OnTriggerStay(Collider other)
    //{
    //    // Check if the object triggering the event has the tag "Player"
    //    if (other.CompareTag("Player"))
    //    {
    //        // Check if the object is also in contact with another object tagged as "Player"
    //        if (other.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds))
    //        {
    //            transform.Rotate(Vector3.up, 120f);
    //        }

    //    }

    //}
    public float rotationSpeed = 30f; // Rotation speed in degrees per second

    private HashSet<string> playersInTrigger = new HashSet<string>();

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            playersInTrigger.Add(other.name);
            Debug.Log("Player entered: " + other.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            playersInTrigger.Remove(other.name);
            Debug.Log("Player exited: " + other.name);
        }
    }

    void Update()
    {
        // Perform rotation if both players are within the trigger
        if (playersInTrigger.Contains("Player1") && playersInTrigger.Contains("Player2"))
        {

            if (transform.name.Contains("Mushroom") || transform.name.Contains("Bacon"))
            {
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            }
            else
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            }
        }
    }
}
