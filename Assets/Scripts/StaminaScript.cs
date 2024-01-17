using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaScript : MonoBehaviour
{
    public Slider staminaBar;

    private int maxStamina = 100;
    private int currentStamina;

    // for recharging
    private Coroutine rechargeCoroutine;
    public float rechargeDelay = 2f;
    public float rechargeRate = 30f;
    public float staminaConsumptionRate = 4f; // Adjust this value to slow down stamina consumption during running

    public static StaminaScript instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;

        // Start the coroutine for stamina recharge
        rechargeCoroutine = StartCoroutine(RechargeStamina());
    }

    public void UseStamina(int amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            // Stop the previous coroutine if it's running
            if (rechargeCoroutine != null)
            {
                StopCoroutine(rechargeCoroutine);
            }

            // Start the coroutine again
            rechargeCoroutine = StartCoroutine(RechargeStamina());
        }
        else
        {
            Debug.Log("Not enough Stamina");
        }
    }

    IEnumerator RechargeStamina()
    {
        // Wait for a few seconds from the rechargeDelay before recharging the stamina
        yield return new WaitForSeconds(rechargeDelay);

        // Recharge the stamina up to its maximum value
        while (currentStamina < maxStamina)
        {
            float rechargeAmount = rechargeRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina + Mathf.CeilToInt(rechargeAmount), maxStamina);
            staminaBar.value = currentStamina;
            yield return null;
        }
    }

    public bool HasEnoughStamina()
    {
        return currentStamina > 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("sceneManager"))
        {
            Destroy(other.gameObject);
        }
    }
}
