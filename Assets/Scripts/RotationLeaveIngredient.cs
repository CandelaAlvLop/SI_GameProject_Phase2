using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLeaveIngredient : MonoBehaviour
{
    public Quaternion initialRotation;

    private PluginConnector connector;

    public bool player2;

    void Start()
    {
        connector = FindObjectOfType<PluginConnector>();    
        initialRotation = transform.rotation;
    }

    //---- Methods to control and reset rotation ----
    public void SetRotation(float rotationAmount)
    {
        connector.enableRotation = true;

        //transform.Rotate(Vector3.forward * rotationAmount);
       
    }

    public void ResetRotation()
    {
        transform.rotation = initialRotation;
        connector.enableRotation = false;
    }
}
