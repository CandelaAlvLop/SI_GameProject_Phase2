using UnityEngine;
using UnityEngine.UI;

public class RotationMovement : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
    public GameObject pivotPizza;
    public GameObject pizzaCooked;

    private bool rotationComplete = false;
    private float activationDelay = 20.0f;
    private float timer = 0.0f;

    private PlacePlayer placePlayerScript;  // Referencia al script PlacePlayer

    public TextMesh timerText;


    private void Start()
    {
        pizzaCooked.SetActive(false);

        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            placePlayerScript = gameManager.GetComponent<PlacePlayer>();
        }
    }

    void Update()
    {

        Vector3 playerToPizza = pivotPizza.transform.position - transform.position;

        // Calculate the angle between the direction from the player to the pizza and the forward direction of the player
        float angle = Vector3.SignedAngle(transform.forward, playerToPizza, Vector3.up);


        if (placePlayerScript.startCooking)
        {
            //// Check for left arrow key press
            //if (Input.GetKey(KeyCode.K))
            //{
            //    RotateObject(-angle * Time.deltaTime);
            //}
            // Check for right arrow key press
           if (Input.GetKey(KeyCode.L))
            {
                RotateObject(angle * Time.deltaTime);
            }

            if (rotationComplete)
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
}
