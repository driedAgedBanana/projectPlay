using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float speed;
    public float runSpeedMultiplier = 2f; // Adjust this value to set the running speed multiplier
    private bool isRunning = false;
    [SerializeField] private TrailRenderer tr;
    private StaminaScript staminaScript;

    // For animation
    private Animator animator;

    // Time variables for sitting idle
    private float idleTimer = 0f;
    public float idleTimeThreshold = 10f; // Adjust this value to set the idle time threshold

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        staminaScript = StaminaScript.instance;
        tr.emitting = false;
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
            isRunning = false;
            // Increment the idle timer when the player is not active
            idleTimer += Time.deltaTime;
            tr.emitting = false;
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
            // Move vertically
            transform.Translate(Vector2.up * yInput * currentSpeed * Time.deltaTime);
        }

        // Set animation
        animator.SetBool("nekoWalk", xInput != 0);
        animator.SetBool("nekoWalkUp", yInput != 0);
        animator.SetBool("nekoWalkDown", yInput < 0);
        animator.SetBool("NekoDashup", yInput != 0 && isRunning);
        animator.SetBool("NekoDashDown", yInput < 0 && isRunning);
        animator.SetBool("NekoDash", xInput != 0 && isRunning);
    }
}
