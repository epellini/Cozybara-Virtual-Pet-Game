using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public const float MaxValue = 100f; // Maxvalue of the % in the bars

    // UI Elements
    public Image[] needBars;        // Assign UI images for hunger, fun, energy, dirtyness in the Inspector
    public TMP_Text[] needTexts;    // Assign TMP Text components for hunger, fun, energy, dirtyness in the Inspector
    public Image[] needIcons;       // Assign UI images for icons associated with each need in the Inspector
    public Image cappyHappyImage;   // Assign UI image for Cappy's happy state in the Inspector
    public Image cappySadImage;     // Assign UI image for Cappy's sad state in the Inspector
    public Text gameOverText;       // Assign UI Text component for game over text in the Inspector

    // Need Variables
    private float[] needs = new float[4]; // Index 0: hunger, 1: fun, 2: energy, 3: dirtyness

    private void Start()
    {
        InitializeUI();
        UpdateUI();
    }

    private void Update()
    {
        UpdateNeeds();
        CheckNeeds();
        UpdateUI();
    }

    private void InitializeUI()
    {
        // Initialize the initial values of needs
    for (int i = 0; i < needs.Length; i++)
    {
        needs[i] = MaxValue; // Set all needs to their maximum value (100%) initially
    }

    // Hide all need icons initially (These icons will act like alerts to let player know Cappy is hungry, etc.)
    for (int i = 0; i < needIcons.Length; i++)
    {
        needIcons[i].CrossFadeAlpha(0, 0.001f, true);
    }

    // Make Cappy happy from the start
    cappySadImage.gameObject.SetActive(false);

       // Set initial UI values (e.g., text) if needed
    for (int i = 0; i < needTexts.Length; i++)
    {
        needTexts[i].text = "100%";
    }

    }

    // Here we set how long it takes Cappy to be hungry, bored, etc.
    // The lower the number, the longer it takes for Cappy to be hungry, bored, etc
     public float[] decreaseRates = new float[] { 0.0f, 0.0f, 0.0f, 0.0f }; // Adjust these rates as needed. (Order: Hunger, Bored, Energy, Cleanliness)

    private void UpdateNeeds()
    {
        for (int i = 0; i < needs.Length; i++)
        {
            needs[i] -= decreaseRates[i] * Time.deltaTime;
            if (needs[i] < 0)
            {
                needs[i] = 0;
            }
        }
    }

    private void CheckNeeds()
    {
        // Check each need and update UI accordingly
    for (int i = 0; i < needs.Length; i++)
    {
        if (needs[i] <= 50)
        {
            // Show the need icon (e.g., make it visible)
            needIcons[i].CrossFadeAlpha(1, 0.05f, true);
        }
        else
        {
            // Hide the need icon (e.g., make it invisible)
            needIcons[i].CrossFadeAlpha(0, 0.05f, true);
        }
    }

    // Check if Cappy is happy or sad based on specific conditions
    if (needs[0] <= 60 || needs[1] <= 30 || needs[2] <= 20 || needs[3] <= 40)
    {
        // Cappy is sad
        cappyHappyImage.gameObject.SetActive(false);
        cappySadImage.gameObject.SetActive(true);
    }
    else
    {
        // Cappy is happy
        cappySadImage.gameObject.SetActive(false);
        cappyHappyImage.gameObject.SetActive(true);
    }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < needBars.Length; i++)
        {
            float ratio = needs[i] / MaxValue;
            needBars[i].rectTransform.localScale = new Vector3(ratio, 1, 1);
            needTexts[i].text = (ratio * 100).ToString("0") + '%';
        }
    }

    public void PerformAction(int actionIndex)
    {
    switch (actionIndex)
    {
        case 0: // Feed action
            needs[0] += 10; // Increase hunger
            break;

        case 1: // Play action
            needs[1] += 10; // Increase fun
            break;

        case 2: // Sleep action
            needs[2] += 10; // Increase energy
            break;

        case 3: // Clean action
            needs[3] += 10; // Decrease dirtyness
            break;

        // Add more cases for additional actions as needed

        default:
            // Handle unknown action or no action
            break;
    }

    // Ensure the needs values stay within the valid range
    for (int i = 0; i < needs.Length; i++)
    {
        if (needs[i] > MaxValue)
        {
            needs[i] = MaxValue;
        }
    }
    }
}