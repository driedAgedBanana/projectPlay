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
    public float rechargeRate = 200f; // Updated variable name for consistency

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
            currentStamina += (int)(rechargeRate * Time.deltaTime);
            staminaBar.value = currentStamina;
            yield return null;
        }

        // Ensure the currentStamina doesn't exceed maxStamina
        currentStamina = Mathf.Min(currentStamina, maxStamina);

        // Reset the coroutine reference when the recharge is complete
        rechargeCoroutine = null;
    }

    public bool HasEnoughStamina()
    {
        return currentStamina > 0;
    }
}
