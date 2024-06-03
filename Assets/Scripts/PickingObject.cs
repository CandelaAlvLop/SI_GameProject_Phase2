using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IngredientCounter
{
    // Couter of ingredients
    public static int totalIngredientsPlaced = 0;
    public static int totalIngredients = 17;
}

public class PickingObject : MonoBehaviour
{
    //List of possible ingredients
    public List<GameObject> ingredientPrefabs;

    // Vertical offset for ingredient position
    public float y;

    // Current picked up ingredient
    public GameObject ingredientInstance;

    //Auxiliar Variables for the movement of the player
    public float rotationSpeed = 30f;
    public float moveSpeed = 5f;

    // Margins for position and rotation matching with patterns
    public float posMargin = 4f;
    public float rotMargin = 30f;

    public GameObject hand;

    // Audio clip for dropping sound
    public AudioClip dropSound;
    private AudioSource audioSource;

    // Array to track remaining patterns for each ingredient type
    private int[] remainingPatterns;

    private void Start()
    {
        // Initialize the audio source component and remaining patterns
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1.0f;
        InitializeRemainingPatterns();
    }

    private void InitializeRemainingPatterns()
    {
        // Assuming there are 6 types of ingredients
        remainingPatterns = new int[6];

        // Initialize the pattern counts based on the number of patterns in the scene
        remainingPatterns[0] = GameObject.FindGameObjectsWithTag("MushroomPattern").Length;
        remainingPatterns[1] = GameObject.FindGameObjectsWithTag("OlivePattern").Length;
        remainingPatterns[2] = GameObject.FindGameObjectsWithTag("BaconPattern").Length;
        remainingPatterns[3] = GameObject.FindGameObjectsWithTag("OnionPattern").Length;
        remainingPatterns[4] = GameObject.FindGameObjectsWithTag("CheesePattern").Length;
        remainingPatterns[5] = GameObject.FindGameObjectsWithTag("TomatoPattern").Length;
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

        // Check if the player already has an ingredient, a valid crate index was found, and there are remaining patterns for that ingredient
        if (ingredientInstance == null && crateIdx != -1 && remainingPatterns[crateIdx] > 0)
        {
            GameObject prefab = ingredientPrefabs[crateIdx];
            Vector3 spawnPosition = new Vector3(0, hand.transform.position.y - y, 0);
            Quaternion spawnRotation = (crateIdx == 0 || crateIdx == 2) ? Quaternion.Euler(90, 0, 0) : Quaternion.identity;

            // Instantiate the ingredient prefab at the calculated position and rotation
            ingredientInstance = Instantiate(prefab, transform.position, spawnRotation, transform);
            ingredientInstance.transform.localPosition = spawnPosition;
        }
    }


    //------- Functions to drop automatically when matching pattern -------
    void FixedUpdate()
    {
        if (ingredientInstance != null) //Check player has an ingredient
        {
            GetPatternPositions(); // Check for matching patterns and drop the ingredient if a match is found
        }
    }


    //--- Check for Matching Patterns and Drop Ingredient ---
    private void GetPatternPositions()
    {
        // These methods check if the current ingredient matches any of the patterns on the pizza
        CheckAndDropIngredient("Tomato", "TomatoPattern", 5);
        CheckAndDropIngredient("Cheese", "CheesePattern", 4);
        CheckAndDropIngredient("Olive", "OlivePattern", 1);
        CheckAndDropIngredient("Onion", "OnionPattern", 3);
        CheckAndDropIngredient("Bacon", "BaconPattern", 2);
        CheckAndDropIngredient("Mushroom", "MushroomPattern", 0);
    }


    //--- Check and Drop Ingredient at Matching Pattern ---
    private void CheckAndDropIngredient(string ingredientName, string patternTag, int patternIndex)
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
                    remainingPatterns[patternIndex]--;
                    return; // Exit the method once the ingredient is dropped
                }
            }
        }
    }


    //---- Check if Ingredient Position Matches Pattern position ----
    private bool PositionMatch(Vector3 patternPosition)
    {
        Vector2 ingredientPos = new Vector2(ingredientInstance.transform.position.x, ingredientInstance.transform.position.z);
        Vector2 patternPos = new Vector2(patternPosition.x, patternPosition.z);
        float distance = Vector2.Distance(ingredientPos, patternPos); //Compute the distance between the ingredient and the pattern
        Debug.Log($"Position check: distance = {distance}, margin = {posMargin}");
        return distance <= posMargin;
    }

    //---- Check if Ingredient Rotation Matches Pattern ----
    private bool RotationMatch(Quaternion patternRotation)
    {
        float angle = Quaternion.Angle(ingredientInstance.transform.rotation, patternRotation);
        Debug.Log($"Rotation check: angle = {angle}, margin = {rotMargin}");
        return angle <= rotMargin;
    }


    //---- Drop Ingredient and detach it from the player ----
    private void DropIngredientAtPattern(Transform patternTransform)
    {
        if (ingredientInstance != null)
        {
            ingredientInstance.transform.SetParent(null); //Detach from Parent
            ingredientInstance.transform.position = patternTransform.position;
            ingredientInstance.transform.rotation = patternTransform.rotation;
            ingredientInstance = null;

            IngredientCounter.totalIngredientsPlaced++; // Increment the count of placed ingredients
            Debug.Log("Ingredients placed: " + IngredientCounter.totalIngredientsPlaced);

            // Load sound dropped element
            if (audioSource != null && dropSound != null)
            {
                audioSource.PlayOneShot(dropSound);
            }

            Debug.Log("Ingredient dropped at pattern position.");

            CheckingCount(IngredientCounter.totalIngredientsPlaced);
        }

    }


    // ----- CHECKING COUNT INGREDIENTS ----
    void CheckingCount(int count)
    {
        // Check if all ingredients have been placed
        if (IngredientCounter.totalIngredientsPlaced == IngredientCounter.totalIngredients)
        {
            // All ingredients placed, do something (e.g., end game)
            Debug.Log("All ingredients placed!");

            // Changing of Scene
            SceneManager.LoadScene("Cook");
        }
    }


    //------ OTHER FUNCTIONS ------

    //------ Handle Player Movement Up and Down (Auxiliary Function to test some behaviours) ------
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }


    //---- Searches for an ingredient in a list based on its name and returns its index ---
    public int GetIngredientIndexByName(string name)
    {
        // Search for an ingredient in the list based on its name and return its index
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