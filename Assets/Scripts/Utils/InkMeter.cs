using UnityEngine;
using UnityEngine.UI;

public class InkMeter : MonoBehaviour
{
    public Image inkMeter; // Reference to the ink meter UI
    public float maxInk = 100f; // Maximum ink capacity
    public float currentInk; // Current ink level
    public float refillRate = 5f; // Ink refilling rate per second
    public float inkConsumption = 10f; // Ink consumed per shot

    void Start()
    {
        currentInk = maxInk; // Initialize ink to maximum
    }

    void Update()
    {
        // Refill ink over time
        if (currentInk < maxInk)
        {
            currentInk += refillRate * Time.deltaTime;
            currentInk = Mathf.Clamp(currentInk, 0, maxInk);
        }

        // Update the UI
        UpdateInkMeter();
    }

    // Call this method when the player shoots
    public void ConsumeInk()
    {
        if (currentInk >= inkConsumption)
        {
            currentInk -= inkConsumption;
            currentInk = Mathf.Clamp(currentInk, 0, maxInk);

            // Update the UI
            UpdateInkMeter();
        }
        else
        {
            Debug.Log("Not enough ink!");
        }
    }

    void UpdateInkMeter()
    {
        if (inkMeter != null)
        {
            inkMeter.fillAmount = currentInk / maxInk; // Update the UI fill amount
        }
    }
}
