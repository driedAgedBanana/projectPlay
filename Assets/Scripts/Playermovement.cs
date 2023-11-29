using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed;
    public float runSpeedMultiplier = 2f; // Adjust this value to set the running speed multiplier
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        // Check if the shift key is held down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Calculate the speed based on whether the player is running or not
        float currentSpeed = isRunning ? speed * runSpeedMultiplier : speed;

        // Move the player
        transform.Translate(Vector2.right * xInput * currentSpeed * Time.deltaTime);
        transform.Translate(Vector2.up * yInput * currentSpeed * Time.deltaTime);
    }
}
