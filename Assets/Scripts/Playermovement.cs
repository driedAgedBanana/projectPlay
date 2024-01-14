using System.Collections;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed;
    public float runSpeedMultiplier = 2f; // Adjust this value to set the running speed multiplier
    private bool isRunning = false;
    [SerializeField] private TrailRenderer tr;
    private StaminaScript staminaScript;

    //For health
    public int health;
    public int maxHealth = 10;

    // For animation
    private Animator animator;

    private Rigidbody2D rb;

    // Time variables for sitting idle
    private float idleTimer = 0f;
    public float idleTimeThreshold = 10f; // Adjust this value to set the idle time threshold

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        staminaScript = StaminaScript.instance;
        tr.emitting = false;
        health = maxHealth;
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

        if (Input.GetKey(KeyCode.F))
        {
            StartCoroutine(playerIsAttacking());
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


        // Set animation
        animator.SetBool("nekoWalk", xInput != 0);
        animator.SetBool("nekoWalkUp", yInput > 0);
        animator.SetBool("nekoWalkDown", yInput < 0);


        // Dashing animation
        animator.SetBool("NekoDashup", yInput != 0 && isRunning);
        animator.SetBool("NekoDashDown", yInput != 0 && isRunning);
        animator.SetBool("NekoDash", xInput != 0 && isRunning);

        if (!isRunning && idleTimer >= idleTimeThreshold)
        {
            StartCoroutine(PlaySittingAnimations());
        }

        
        //// Sitting animation
        //animator.SetBool("NekoBeginSitting", !isRunning && idleTimer >= idleTimeThreshold);
        //animator.SetBool("NekoIsSitting", !isRunning && idleTimer >= idleTimeThreshold);


    }

    public void TakingDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            Destroy(tr); 
            tr = null;
        }
    }

    IEnumerator playerIsAttacking()
    {
        speed = 0;
        animator.SetBool("isAttacking", true);

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
        {
            animator.SetTrigger("NekoAtk");
        }
        else
        {
            // Player is moving vertically or standing still
            if (yInput > 0)
            {
                //Player moving up
                animator.SetTrigger("NekoAtkUp");
            }
            else if (xInput < 0)
            {
                //Player moving down
                animator.SetTrigger("NekoAtkFront");
            }
        }

        yield return new WaitForSeconds(1);
        speed = 5;
        animator.SetBool("isAttacking", false);
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
}
