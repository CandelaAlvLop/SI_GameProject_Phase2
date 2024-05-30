using UnityEngine;
using UnityEngine.UI;

public class RotationTarget : MonoBehaviour
{
    
    public float rotationSpeed;
    public GameObject pivotPizza;

    //-- Pizza elements --
    public GameObject pizzaUncooked;

    private float activationDelay = 60.0f;
    private float timer = 0.0f;

    public TextMesh timerText;

    private PizzaRotation pizzaRotation;

    // Reference to PlacePlayer script
    public PlacePlayer placePlayerScript;


    private void Start()
    {
        pizzaRotation = FindObjectOfType<PizzaRotation>();
    }

    void Update()
    {

        Vector3 playerToPizza = pivotPizza.transform.position - transform.position;

        // Calculate the angle between the direction from the player to the pizza and the forward direction of the player
        float angle = Vector3.SignedAngle(transform.forward, playerToPizza, Vector3.up);

        //If we start the cooking phase we show in the screen a target that the players must follow 
        if (placePlayerScript != null && placePlayerScript.startCooking)
        {           

            RotateObject(angle *rotationSpeed* Time.deltaTime);
           
            timer += Time.deltaTime;
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));

            if (timer >= activationDelay)
            {
                timerText.text = "WELL DONE";

                //stop the target rotation and we make it dissapear
                rotationSpeed = 0f;

                //Stop Pizza Rotation
                pizzaRotation.shouldRotate = false;

            }
            
        }
    }

  

    //---- Function to rotate the object around the pivot ----
    void RotateObject(float angle)
    {
        // Calculate pivot to object vector
        Vector3 pivotToObject = transform.position - pivotPizza.transform.position;

        // Rotate the object around the pivot
        transform.RotateAround(pivotPizza.transform.position, Vector3.up, angle);

      
    }



}
