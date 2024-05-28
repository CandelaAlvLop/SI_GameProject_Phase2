using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRotation : MonoBehaviour
{

    public float rotationSpeed = 10.0f;

    public bool shouldRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRotate)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
