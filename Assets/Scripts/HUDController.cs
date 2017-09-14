﻿using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    playerController pc;

    [Header("Stamina")]
    //public Image staminaBar;
    //public Image staminaUsed;
    public Slider staminaBar;

    [Header("Health")]
    public Slider healthBar;

    // Use this for initialization
    void Start ()
    {
        pc = playerController.instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Change stamina bar display to represent remaining stamina
        //staminaBar.fillAmount = pc.stamina / pc.maxStamina;
        //staminaUsed.fillAmount = 1.0f - staminaBar.fillAmount;
        staminaBar.value = pc.stamina / pc.maxStamina;

        // Change health bar display to represent remaining stamina
        healthBar.value = pc.health / pc.maxHealth;
    }
}
