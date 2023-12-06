using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed;
    public float runSpeedMultiplier = 2f; // Adjust this value to set the running speed multiplier
    private bool isRunning = false;
    private StaminaScript staminaScript;

    // For animation
    private Animator animator;
    private float idleTimer = 0f;
    private float idleTimeThreshold = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        staminaScript = StaminaScript.instance;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        // Check if the shift key is held down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Checking if the stamina is enough to run
            if (staminaScript != null && staminaScript.HasEnoughStamina())
            {
                isRunning = true;
                staminaScript.UseStamina(1);
            }
            else
            {
                // If there is not enough stamina, stop running
                isRunning = false;
            }
        }
        else
        {
            isRunning = false;
        }

        // Calculate the speed based on whether the player is running or not
        float currentSpeed = isRunning ? speed * runSpeedMultiplier : speed;

        // Move the player
        if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
        {
            // Move horizontally
            transform.Translate(Vector2.right * xInput * currentSpeed * Time.deltaTime);

            // Flip the sprite when moving to the right
            if (xInput > 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            // Keep the sprite facing left when moving to the left
            else if (xInput < 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else
        {
            // Move vertically
            transform.Translate(Vector2.up * yInput * currentSpeed * Time.deltaTime);
        }

        // Set animation
        animator.SetBool("Running", xInput != 0);
        animator.SetBool("RunningUp", yInput != 0);
        animator.SetBool("RunningDown", yInput < 0);

        //player idle testing time
        if (idleTimer >= idleTimeThreshold)
        {
            animator.SetBool("PlayerSitIdle", true);
        }
        else
        {
            animator.SetBool("PlayerSitIdle", false);
        }
    }
}
