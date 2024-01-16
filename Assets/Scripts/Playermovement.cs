using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float runSpeedMultiplier = 2f;
    private bool isRunning = false;
    private TrailRenderer tr;
    private StaminaScript staminaScript;

    public PlayerDir MyDir;

    // For health
    public int maxHealth = 10;
    public int currentHealth;

    // For animation
    private Animator animator;
    private Rigidbody2D rb;

    // Time variables for sitting idle
    private float idleTimer = 0f;
    public float idleTimeThreshold = 10f;
    private bool isSitting = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        staminaScript = StaminaScript.instance;
        tr.emitting = false;
        currentHealth = maxHealth;
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
                idleTimer = 0f; // Reset the idle timer when the player is active
                tr.emitting = true;
            }
            else
            {
                // If there is not enough stamina, stop running
                isRunning = false;
                tr.emitting = false;
            }
        }
        else
        {
            // Increment the idle timer when the player is not active
            idleTimer += Time.deltaTime;
            tr.emitting = false;

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
                transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
            }
            // Keep the sprite facing left when moving to the left
            else if (xInput < 0)
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }
        else
        {
            // Move the player using Rigidbody2D velocity
            Vector2 moveInput = new Vector2(xInput, yInput);
            rb.velocity = moveInput.normalized * currentSpeed;
        }

        //set dir for attack script
        if(xInput > 0)
        {
            MyDir = PlayerDir.Right;
        }
        else if(xInput < 0)
        {
            MyDir = PlayerDir.Left;
        }
        else if(yInput > 0)
        {
            MyDir = PlayerDir.Up;
        }
        else if(yInput < 0)
        {
            MyDir = PlayerDir.Down;
        }

        // Set animation
        animator.SetBool("nekoWalk", xInput != 0);
        animator.SetBool("nekoWalkUp", yInput > 0);
        animator.SetBool("nekoWalkDown", yInput < 0);

        // Dashing animation
        animator.SetBool("NekoDashup", yInput != 0 && isRunning);
        animator.SetBool("NekoDashDown", yInput != 0 && isRunning);
        animator.SetBool("NekoDash", xInput != 0 && isRunning);

        // Check for sitting animation
        if (!isRunning)
        {
            if (idleTimer >= idleTimeThreshold && !isSitting)
            {
                StartCoroutine(PlaySittingAnimations());
                isSitting = true;
            }
            else if (idleTimer < idleTimeThreshold)
            {
                isSitting = false;
            }
        }
    }

    public void TakingDamage(int amount)
    {
        currentHealth -= amount;

        // Log the health for testing purposes
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            // Player death logic can be added here
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator PlayerDeath()
    {
        speed = 0;
        // Play death animation
        animator.SetBool("Die", true);

        // Wait for the duration of the death animation
        yield return new WaitForSeconds(1.5f);

        // Additional actions after death animation can be added here
        // For now, let's destroy the player GameObject
        Destroy(gameObject);
    }

    IEnumerator PlaySittingAnimations()
    {
        // Play the "NekoBeginSitting" animation
        animator.SetBool("NekoBeginSitting", true);
        yield return new WaitForSeconds(0.5f);

        // Play the "NekoIsSitting" animation
        animator.SetBool("NekoBeginSitting", false);
        animator.SetBool("NekoIsSitting", true);
    }

    public enum PlayerDir
    {
        Up,
        Down,
        Left,
        Right,
    }
}
