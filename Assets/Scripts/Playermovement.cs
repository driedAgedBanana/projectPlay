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
        UpdateAnimation();
        CheckSittingAnimation();
    }

    void HandleMovementInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        HandleRunningInput(xInput, yInput);

        float currentSpeed = isRunning ? speed * runSpeedMultiplier : speed;

        if (Mathf.Abs(xInput) > Mathf.Abs(yInput))
        {
            MoveHorizontally(xInput);
        }
        else
        {
            MoveVertically(yInput);
        }

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
            IncrementIdleTimer();
        }
    }

    void MoveHorizontally(float xInput)
    {
        transform.Translate(Vector2.right * xInput * GetCurrentSpeed() * Time.deltaTime);

        FlipSprite(xInput);
    }

    void MoveVertically(float yInput)
    {
        Vector2 moveInput = new Vector2(0, yInput);
        rb.velocity = moveInput.normalized * GetCurrentSpeed();
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
    }

    float GetCurrentSpeed()
    {
        return isRunning ? speed * runSpeedMultiplier : speed;
    }

    void FlipSprite(float xInput)
    {
        transform.localScale = new Vector3(xInput > 0 ? -0.8f : 0.8f, 0.8f, 0.8f);
    }

    void UpdateAnimation()
    {
        animator.SetBool("nekoWalk", IsWalking());
        animator.SetBool("nekoWalkUp", IsMovingUp());
        animator.SetBool("nekoWalkDown", IsMovingDown());

        animator.SetBool("NekoDashup", IsDashing() && IsMovingUp());
        animator.SetBool("NekoDashDown", IsDashing() && IsMovingDown());
        animator.SetBool("NekoDash", IsDashing());

        animator.SetBool("Die", IsDead());
    }

    bool IsWalking()
    {
        return Input.GetAxis("Horizontal") != 0;
    }

    bool IsMovingUp()
    {
        return Input.GetAxis("Vertical") > 0;
    }

    bool IsMovingDown()
    {
        return Input.GetAxis("Vertical") < 0;
    }

    bool IsDashing()
    {
        return IsDashingUp() || IsDashingDown() || IsDashingSideways();
    }

    bool IsDashingUp()
    {
        return Input.GetAxis("Vertical") != 0 && isRunning;
    }

    bool IsDashingDown()
    {
        return Input.GetAxis("Vertical") != 0 && isRunning;
    }

    bool IsDashingSideways()
    {
        return Input.GetAxis("Horizontal") != 0 && isRunning;
    }

    bool IsDead()
    {
        return currentHealth <= 0;
    }

    void CheckSittingAnimation()
    {
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

    void IncrementIdleTimer()
    {
        idleTimer += Time.deltaTime;
    }

    public void TakingDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player Health: " + currentHealth);

        if (IsDead())
        {
            StartCoroutine(PlayerDeath());
        }
    }

    IEnumerator PlayerDeath()
    {
        speed = 0;
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator PlaySittingAnimations()
    {
        animator.SetBool("NekoBeginSitting", true);
        yield return new WaitForSeconds(0.5f);
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
