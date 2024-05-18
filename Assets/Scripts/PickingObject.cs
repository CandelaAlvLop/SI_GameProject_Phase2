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

    public GameObject hand;


    void Start()
    {

    }

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



    //------- Corutine to make the ingredient fall ------------
    IEnumerator DelayedFall(GameObject ingredient) 
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

        // Disable the isTrigger temporarily to allow for physics interaction
        Collider collider = ingredient.GetComponent<Collider>();

        if(collider != null) collider.isTrigger = false;

        // Wait for 1 second before enabling trigger again
        yield return new WaitForSeconds(1);

        // Re-enable the Collider to allow picking it up again and isKinematic in this way the ingredient doesn't fall
        if (collider != null) collider.isTrigger = true;
     
        if (rb != null) rb.isKinematic = true;
        
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

        //--------- Check if the player already has an ingredient --------
        if (ingredientInstance == null && crateIdx != -1)
        {
            GameObject prefab = ingredientPrefabs[crateIdx];
            Vector3 spawnPosition = new Vector3(0, hand.transform.position.y - y, 0);
            Quaternion spawnRotation = (crateIdx == 0 || crateIdx == 2) ? Quaternion.Euler(90, 0, 0) : Quaternion.identity;
            ingredientInstance = Instantiate(prefab, transform.position, spawnRotation, transform);
            ingredientInstance.transform.localPosition = spawnPosition;


        }

        PickingAgainObject(collision);

    }


    //----- Function to pick up an object again -----
    private void PickingAgainObject(Collider collision)
    {
        if (ingredientInstance == null)
        {

            if (collision.gameObject.CompareTag("Ingredient"))
            {
                int index = GetIngredientIndexByName(collision.gameObject.name);

                print(index);

                // Make the colliding object a child of the player
                ingredientInstance = collision.gameObject;
                ingredientInstance.transform.SetParent(transform);

                // Reset position and rotation of the ingredient to match the player
                Quaternion spawnRotation = (index == 0 || index == 2) ? Quaternion.Euler(90, 0, 0) : Quaternion.identity;
                ingredientInstance.transform.localRotation = spawnRotation;
                ingredientInstance.transform.localPosition = new Vector3(0, hand.transform.position.y - y, 0);


            }
        }
    }

    public int GetIngredientIndexByName(string name)
    {
        for (int i = 0; i < ingredientPrefabs.Count; i++)
        {
            // Utilizza Contains per verificare se il nome del prefab contiene la stringa desiderata
            if (name.Contains(ingredientPrefabs[i].name))
            {
                return i;
            }
        }
        return -1;
    }


}