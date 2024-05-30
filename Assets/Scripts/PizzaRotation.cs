using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRotation : MonoBehaviour
{

    public float rotationSpeed = 10.0f;

    public bool shouldRotate = false;

  
    // Update is called once per frame
    void Update()
    {
        if (shouldRotate)
        {
            transform.Rotate(Vector3.up, -1*rotationSpeed * Time.deltaTime);
        }
    }
}
