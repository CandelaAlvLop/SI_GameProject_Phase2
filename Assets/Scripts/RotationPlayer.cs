using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlayer : MonoBehaviour
{
    private Quaternion initialRotation;

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
        //Descomentar cuando vamos a usarlo en tracker
        connector.enableRotation = true;

        //Comentar cuando vamos a usar el tracker       
        //transform.Rotate(Vector3.forward * rotationAmount);
       
    }

    public void ResetRotation()
    {
        transform.rotation = initialRotation;
        connector.enableRotation = false;
    }
}
