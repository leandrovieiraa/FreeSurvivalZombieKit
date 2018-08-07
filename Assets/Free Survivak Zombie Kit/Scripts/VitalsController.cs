using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;
    public int currentHealth;
    public Image healthImage;
    public Text healthTextQty;
    public Color fullHealthColor;
    public Color emptyHealthColor;

    public void Start()
    {
        currentHealth = maxHealth;

        healthImage.fillAmount = maxHealth / 100;
        healthImage.color = fullHealthColor;

        healthTextQty.text = maxHealth.ToString();
    }

    private void Update()
    {
        if (currentHealth <= 0)
            currentHealth = 0;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        if (Input.GetKey(KeyCode.Y))
        {
            Increase();
        }
         
        if (Input.GetKey(KeyCode.U))
        {
            Decrease();
        }
          
    }

    void Increase()
    {
        ChangeSliderValue(+1f / 100f);
        currentHealth += 1;
    }

    void Decrease()
    {
        ChangeSliderValue(-1f / 100f);
        currentHealth -= 1;
    }

    public void ChangeSliderValue(float value)
    {     
        Debug.Log(value);
        healthImage.fillAmount += value;
        healthImage.color = Color.Lerp(emptyHealthColor, fullHealthColor, healthImage.fillAmount);
        healthTextQty.text = currentHealth.ToString();
    }
}
