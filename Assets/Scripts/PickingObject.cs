using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingObject : MonoBehaviour
{
    public List<GameObject> ingredientPrefabs;  //List of possible ingredients
    public float y;
    public GameObject ingredientInstance;

    public RotationPlayer playerRotation; // Referencia al script de rotación del jugador
    public float rotationSpeed = 30f;
    public float moveSpeed = 5f; // Speed at which the player moves down


    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (ingredientInstance != null)
        {
            // Temporary key to increase player rotation on the X axis
            if (Input.GetKey(KeyCode.R))
            {
                playerRotation.SetRotation(rotationSpeed * Time.deltaTime);
            }



            // Check if the player's rotation in the Z axis is greater than 45 degrees
            if (transform.rotation.eulerAngles.z > 45f)
            {
                // Remove the ingredient from the player
                ingredientInstance.transform.parent = null;
                StartCoroutine(DelayedFall(ingredientInstance));
                ingredientInstance = null; // Set the ingredientInstance to null after dropping it in this way we can take another ingredient

                // Reset the player's rotation to the initial rotation
                playerRotation.ResetRotation();

            }
        }
    }

    IEnumerator DelayedFall(GameObject ingredient) //Corutine to make the ingredient fall
    {
        // Wait for 1 seconds
        yield return new WaitForSeconds(1);

        // Adding force to make the ingrendient fall
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        string crateName = collision.gameObject.name;
        int crateIdx = -1;

        switch (crateName)
        {
            case "Crate Mushroom":
                crateIdx = 0;
                break;
            case "Crate Olives":
                crateIdx = 1;
                break;
            case "Crate Bacon":
                crateIdx = 2;
                break;
            case "Crate Onions":
                crateIdx = 3;
                break;
            case "Crate Cheese":
                crateIdx = 4;
                break;
            case "Crate Tomatoes":
                crateIdx = 5;
                break;
        }

        // Check if the player already has an ingredient
        if (ingredientInstance == null && crateIdx != -1)
        {
            if (crateIdx == 0 || crateIdx == 2)
            {
                ingredientInstance = Instantiate(ingredientPrefabs[crateIdx], transform.position, Quaternion.Euler(90, 0, 0), transform);
            }
            else
            {
                ingredientInstance = Instantiate(ingredientPrefabs[crateIdx], transform.position, Quaternion.identity, transform);
            }

            ingredientInstance.transform.localPosition = new Vector3(0, y, 0);

        }

        if (ingredientInstance == null)
        {
            if (collision.gameObject.tag == "Ingredient")
            {
                ingredientInstance = collision.gameObject;
                ingredientInstance = Instantiate(ingredientPrefabs[crateIdx], transform.position, Quaternion.identity, transform);
                ingredientInstance.transform.localPosition = new Vector3(0, y, 0);
            }

        }
    }
}