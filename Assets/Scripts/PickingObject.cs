using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingObject : MonoBehaviour
{
    public List<GameObject> ingredientPrefabs;  //List of possible ingredients
    public float y; 
    private GameObject ingredientInstance;


    void Update()
    {
        // Check if the ingredient instance exists and if the space key is pressed
        if (ingredientInstance != null && Input.GetKeyDown(KeyCode.Space))  //&& transform.position.y > 10 mueve hacia arriba (para el caso del tracker en el lab)
        {
            // Remove the ingredient from the player
            ingredientInstance.transform.parent = null;
            StartCoroutine(DelayedFall(ingredientInstance));
            ingredientInstance = null; // Set the ingredientInstance to null after dropping it in this way we can take another ingredient
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

        switch(crateName)
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
            if(crateIdx == 0 || crateIdx == 2){
                ingredientInstance = Instantiate(ingredientPrefabs[crateIdx], transform.position, Quaternion.Euler(90, 0, 0), transform);
            }else{
                ingredientInstance = Instantiate(ingredientPrefabs[crateIdx], transform.position, Quaternion.identity, transform);
            }

            ingredientInstance.transform.localPosition = new Vector3(0, y, 0); 

        }

        /*
        else if (ingredientInstance == null && collision.gameObject.CompareTag("Ingredient")) //POSIBLE IDEA NS
        {
            // Si no tiene ninguna instancia de ingredient puede volver a cogerlo (vuelve a ser hijo del player)
            ingredientInstance = collision.gameObject;
            ingredientInstance.transform.parent = transform;
            ingredientInstance.transform.localPosition = new Vector3(0, y, 0);
            Rigidbody rb = ingredientInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        */
    }
}
