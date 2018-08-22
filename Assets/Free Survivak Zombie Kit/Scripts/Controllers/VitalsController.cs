using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    [Header("[Health Settings]")]
    public int maxHealth;
    public int currentHealth;
    public Image healthImage;
    public Text healthTextQty;
    public Color fullHealthColor;
    public Color emptyHealthColor;

    [Header("[Hunger Settings]")]
    public int maxHunger;
    public int currentHunger;
    public Image hungerImage;
    public Text hungerTextQty;
    public Color fullHungerColor;
    public Color emptyHungerColor;

    [Header("[Thirst Settings]")]
    public int maxThirst;
    public int currentThirst;
    public Image thirstImage;
    public Text thirstTextQty;
    public Color fullThirstColor;
    public Color emptyThirstColor;

    public void Start()
    {
        // Health start settings
        currentHealth = maxHealth;
        healthImage.fillAmount = maxHealth / 100;
        healthImage.color = fullHealthColor;
        healthTextQty.text = maxHealth.ToString();

        // Hunger start settings
        currentHunger = maxHunger;
        hungerImage.fillAmount = maxHunger / 100;
        hungerImage.color = fullHungerColor;
        hungerTextQty.text = maxHunger.ToString();

        // Thirst start settings
        currentThirst = maxThirst;
        thirstImage.fillAmount = maxThirst / 100;
        thirstImage.color = fullThirstColor;
        thirstTextQty.text = maxThirst.ToString();

    }

    private void Update()
    {
        if (currentHealth <= 0)
            currentHealth = 0;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        if (currentHunger <= 0)
            currentHunger = 0;
        if (currentHunger >= maxHunger)
            currentHunger = maxHunger;

        if (currentThirst <= 0)
            currentThirst = 0;
        if (currentThirst >= maxThirst)
            currentThirst = maxThirst;    
    }

   public void Increase(int value, string type)
    {
        if(type == "health")
        {
            ChangeSliderValue(value / 100f, "health");
            currentHealth += value;
        }

        if (type == "hunger")
        {
            ChangeSliderValue(value / 100f, "hunger");
            currentHunger += value;
        }

        if (type == "thirst")
        {
            ChangeSliderValue(value / 100f, "thirst");
            currentThirst += value;
        }
    }

    public void Decrease(int value, string type)
    {
        if (type == "health")
        {
            ChangeSliderValue(-value / 100f, "health");
            currentHealth -= value;
        }

        if (type == "hunger")
        {
            ChangeSliderValue(-value / 100f, "hunger");
            currentHunger -= value;
        }

        if (type == "thirst")
        {
            ChangeSliderValue(-value / 100f, "thirst");
            currentThirst -= value;
        }
    }

    public void ChangeSliderValue(float value, string type)
    {     
        if(type == "health")
        {
            healthImage.fillAmount += value;
            healthImage.color = Color.Lerp(emptyHealthColor, fullHealthColor, healthImage.fillAmount);
            healthTextQty.text = currentHealth.ToString();
        }

        if(type == "hunger")
        {
            hungerImage.fillAmount += value;
            hungerImage.color = Color.Lerp(emptyHungerColor, fullHungerColor, hungerImage.fillAmount);
            hungerTextQty.text = currentHunger.ToString();
        }

        if(type == "thirst")
        {
            thirstImage.fillAmount += value;
            thirstImage.color = Color.Lerp(emptyThirstColor, fullThirstColor, thirstImage.fillAmount);
            thirstTextQty.text = currentThirst.ToString();
        }     
    }
}
