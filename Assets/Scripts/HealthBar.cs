using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healtBarText;
    Damage playerDamage;

    private void Awake() 
    {       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamage = player.GetComponent<Damage>();
    }
    
    void Start()
    {
        
        healthSlider.value = CalculateSliderPercentage(playerDamage.HP, playerDamage.MaxHP);
        healtBarText.text = "HP " + playerDamage.HP + " / " + playerDamage.MaxHP;
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnEnable() 
    {
        playerDamage.healthChangge.AddListener(OnPlayerHealthChangge);
    }
    
    private void OnDisable() 
    {
       playerDamage.healthChangge.RemoveListener(OnPlayerHealthChangge);
    }

    void Update()
    {
        
    }

    private void OnPlayerHealthChangge(int newHealth, int maxHealth)
    {
healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healtBarText.text = "HP " + newHealth + " / " + maxHealth;
    }
}
