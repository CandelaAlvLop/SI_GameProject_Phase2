using UnityEngine;
using UnityEngine.UI;

public class RotationMovement : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    public GameObject pivotPizza;
    public GameObject pizzaCooked;

    private bool rotationComplete = false;
    private float activationDelay = 30.0f;
    private float timer = 0.0f;

    public TextMesh timerText;

    private PizzaRotation pizzaRotation;

    // Referencia directa al script PlacePlayer
    public PlacePlayer placePlayerScript;

    private void Start()
    {
        pizzaCooked.SetActive(false);

        Collider collider = pivotPizza.GetComponent<Collider>();
        collider.isTrigger = true;

        pizzaRotation = FindObjectOfType<PizzaRotation>();
    }

    void Update()
    {
        if (placePlayerScript != null && placePlayerScript.startCooking)
        {
            Vector3 playerToPizza = pivotPizza.transform.position - transform.position;

            // Calculate the angle between the direction from the player to the pizza and the forward direction of the player
            float angle = Vector3.SignedAngle(transform.forward, playerToPizza, Vector3.up);

            RotateObject(angle * Time.deltaTime);

            if (rotationComplete && pizzaRotation.shouldRotate)
            {
                timer += Time.deltaTime;
                timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));

                if (timer >= activationDelay)
                {
                    timerText.text = "WELL DONE";
                    pizzaCooked.SetActive(true);
                }
            }
        }
    }

    // Function to rotate the object around the pivot
    void RotateObject(float angle)
    {
        // Calculate pivot to object vector
        Vector3 pivotToObject = transform.position - pivotPizza.transform.position;

        // Rotate the object around the pivot
        transform.RotateAround(pivotPizza.transform.position, Vector3.up, angle);

        // Mark rotation as complete
        rotationComplete = true;
    }
}
