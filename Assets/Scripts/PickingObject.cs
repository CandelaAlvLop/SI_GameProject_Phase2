using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IngredientCounter
{
    // Contador global de ingredientes
    public static int totalIngredientsPlaced = 0;
    public static int totalIngredients = 17;
}

public class PickingObject : MonoBehaviour
{
    public List<GameObject> ingredientPrefabs;  //List of possible ingredients
    public float y;
    public GameObject ingredientInstance;

    public RotationPlayer playerRotation; // Reference to script de RotationPlayer
    
    //Auxiliar Variables for the movement of the player
    public float rotationSpeed = 30f;
    public float moveSpeed = 5f; 

    //Variables for pattern Matching
    public float posMargin = 4f;
    public float rotMargin = 30f;

    public GameObject hand;

    // Audio clip for dropping sound
    public AudioClip dropSound;
    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1.0f;
    }



    //------ Instantiate an ingredient when triggered by a crate ------
    void OnTriggerEnter(Collider collision)
    {

        string crateName = collision.gameObject.name;
        int crateIdx = -1;

        // Determine the index of the crate based on its name
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

        //Check if the player already has an ingredient and a valid crate index was found
        if (ingredientInstance == null && crateIdx != -1)
        {
            GameObject prefab = ingredientPrefabs[crateIdx];
            Vector3 spawnPosition = new Vector3(0, hand.transform.position.y - y, 0);
            Quaternion spawnRotation = (crateIdx == 0 || crateIdx == 2) ? Quaternion.Euler(90, 0, 0) : Quaternion.identity;
            //Instantiate the ingredient prefab at the calculated position and rotation
            ingredientInstance = Instantiate(prefab, transform.position, spawnRotation, transform);
            ingredientInstance.transform.localPosition = spawnPosition;

        
        }

        //PickingAgainObject(collision);

    }


    //------- Functions to drop automatically when matching pattern -------
    void FixedUpdate()
    {
        if (ingredientInstance != null) //Check player has an ingredient
        {
            GetPatternPositions();
        }
    }

    private void GetPatternPositions()
    {
        CheckAndDropIngredient("Tomato", "TomatoPattern");
        CheckAndDropIngredient("Cheese", "CheesePattern");
        CheckAndDropIngredient("Olive", "OlivePattern");
        CheckAndDropIngredient("Onion", "OnionPattern");
        CheckAndDropIngredient("Bacon", "BaconPattern");
        CheckAndDropIngredient("Mushroom", "MushroomPattern");
    }


    private void CheckAndDropIngredient(string ingredientName, string patternTag)
    {
        if (ingredientInstance != null && ingredientInstance.transform.name.Contains(ingredientName))
        {
            GameObject[] patterns = GameObject.FindGameObjectsWithTag(patternTag);
            foreach (GameObject pattern in patterns)
            {
                if (PositionMatch(pattern.transform.position))/* && RotationMatch(pattern.transform.rotation)*/
                { 
                    DropIngredientAtPattern(pattern.transform); // Drop ingredient if it matches position x, z, and rotation
                    pattern.SetActive(false);
                    return; // Exit the method once the ingredient is dropped
                }
            }
        }

    }

    private bool PositionMatch(Vector3 patternPosition)
    {
        Vector2 ingredientPos = new Vector2(ingredientInstance.transform.position.x, ingredientInstance.transform.position.z);
        Vector2 patternPos = new Vector2(patternPosition.x, patternPosition.z);
        float distance = Vector2.Distance(ingredientPos, patternPos); //Compute the distance between the ingredient and the pattern
        Debug.Log($"Position check: distance = {distance}, margin = {posMargin}");
        return distance <= posMargin;
    }

    private bool RotationMatch(Quaternion patternRotation)
    {
        float angle = Quaternion.Angle(ingredientInstance.transform.rotation, patternRotation);
        Debug.Log($"Rotation check: angle = {angle}, margin = {rotMargin}");
        return angle <= rotMargin;
    }


    private void DropIngredientAtPattern(Transform patternTransform)
    {
        if (ingredientInstance != null)
        {
            ingredientInstance.transform.SetParent(null); //Detach from Parent
            ingredientInstance.transform.position = patternTransform.position;
            ingredientInstance.transform.rotation = patternTransform.rotation;
            ingredientInstance = null;

            IngredientCounter.totalIngredientsPlaced = 17;
            //DebuIngredientCounter.g.Log("Ingredients placed: " + IngredientCounter.totalIngredientsPlaced);

            // Reproduce el sonido de ca�da
            if (audioSource != null && dropSound != null)
            {
                audioSource.PlayOneShot(dropSound);
            }

            // Check if all ingredients have been placed
            if (IngredientCounter.totalIngredientsPlaced == IngredientCounter.totalIngredients)
            {
                // All ingredients placed, do something (e.g., end game)
                Debug.Log("All ingredients placed!");

                SceneManager.LoadScene("Cook");
            }

            Debug.Log("Ingredient dropped at pattern position.");
        }

    }
    




    //------ OTHER FUNCTIONS ------

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }


    //------- Drop Ingredient ----------
    private void DropIngredient()
    {
        // Remove the ingredient from the player
        ingredientInstance.transform.parent = null;
        StartCoroutine(DelayedFall(ingredientInstance));
        ingredientInstance = null; // Set the ingredientInstance to null after dropping it in this way we can take another ingredient
        // Reset the player's rotation to the initial rotation
        playerRotation.ResetRotation();
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

    //---- Searches for an ingredient in a list based on its name and returns its index ---
    public int GetIngredientIndexByName(string name)
    {
        for (int i = 0; i < ingredientPrefabs.Count; i++)
        {
            if (name.Contains(ingredientPrefabs[i].name))
            {
                return i;
            }
        }
        return -1;
    }




}