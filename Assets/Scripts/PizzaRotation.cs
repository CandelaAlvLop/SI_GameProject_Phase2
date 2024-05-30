using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRotation : MonoBehaviour
{
    public bool shouldRotate = false;
    public float rotationSpeed = 10f;

    //New Materials of the Pizza
    public Material tomatoPizzaMaterial;
    public Material breadPizzaMaterial;

    //Original Materials of the Pizza
    private Material defaultTomatoMaterial;
    private Material defaultBreadMaterial;
    private Renderer defaultRenderer;

    private float colorTransitionDuration = 20.0f;
    private float colorTransitionTimer = 0.0f;

    private void Start()
    {
        // Get the default materials and the renderer of the "default" child
        defaultRenderer = transform.Find("default").GetComponent<Renderer>();
        defaultBreadMaterial = defaultRenderer.materials[0]; // Bread is the first material
        defaultTomatoMaterial = defaultRenderer.materials[1]; // Tomato is the second material


    }

    // Update is called once per frame
    void Update()
    {
        if (shouldRotate)
        {

            // Rotate the pizza
            transform.Rotate(Vector3.up, -1 * rotationSpeed * Time.deltaTime);

            // Execute color transition
            if (colorTransitionTimer < colorTransitionDuration)
            {
                colorTransitionTimer += 1/Time.deltaTime;

                // Calculate transition completion ratio
                float t = colorTransitionTimer / colorTransitionDuration;

                // Interpolate material colors with slower transition speed
                defaultRenderer.materials[0].color = Color.Lerp(defaultBreadMaterial.color, breadPizzaMaterial.color, t*0.1f);
                defaultRenderer.materials[1].color = Color.Lerp(defaultTomatoMaterial.color, tomatoPizzaMaterial.color, t*0.1f);

            }
            else
            {
                // Set new materials directly after transition
                defaultRenderer.materials[0] = tomatoPizzaMaterial;
                defaultRenderer.materials[1] = breadPizzaMaterial;
            }
        }
        else
        {
            // Reset transition timer if rotation stops
            colorTransitionTimer = 0.0f;
        }
    }
}
