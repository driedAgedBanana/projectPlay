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
    private int currentHealth;

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
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        animator.SetFloat("X", xInput);
        animator.SetFloat("Y", yInput);

        if(xInput != 0 || yInput != 0)
        {
            animator.SetBool("nekoWalk", true);
        }
        else
        {
            animator.SetBool("nekoWalk", false);
        }

        HandleRunningInput(xInput, yInput);
        animator.SetBool("NekoDash", isRunning);

        float currentSpeed = isRunning ? speed * runSpeedMultiplier : speed;

        Vector2 MoveInput = new Vector2(xInput, yInput);
        rb.velocity = MoveInput.normalized * currentSpeed;

        SetPlayerDirection(xInput, yInput);
    }

    void HandleRunningInput(float xInput, float yInput)
    {
        if (Input.GetKey(KeyCode.LeftShift) && staminaScript != null && staminaScript.HasEnoughStamina())
        {
            isRunning = true;
            staminaScript.UseStamina(1);
            idleTimer = 0f;
            tr.emitting = true;
        }
        else
        {
            isRunning = false;
            tr.emitting = false;
        }
    }

    void SetPlayerDirection(float xInput, float yInput)
    {
        if (xInput > 0)
            MyDir = PlayerDir.Right;
        else if (xInput < 0)
            MyDir = PlayerDir.Left;
        else if (yInput > 0)
            MyDir = PlayerDir.Up;
        else if (yInput < 0)
            MyDir = PlayerDir.Down;

        float XScale = transform.localScale.x;
        if (MyDir == PlayerDir.Right)
        {
            XScale = -0.8f;
        }
        else if (MyDir == PlayerDir.Left)
        {
            XScale = 0.8f;
        }

        transform.localScale = new Vector3(XScale, 0.8f, 0.8f);
    }

    public void TakingDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);
    }

    IEnumerator PlayerDeath()
    {
        speed = 0;
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        Destroy(tr);
    }

    public enum PlayerDir
    {
        Up,
        Down,
        Left,
        Right,
    }
}
