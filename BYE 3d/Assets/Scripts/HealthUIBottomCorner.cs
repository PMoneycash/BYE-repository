using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUIBottomCorner : MonoBehaviour
{

    public PlayerHealth playerHealth;
    public TMP_Text healthText;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = playerHealth.currentHealth + "/" + playerHealth.maxHealth;
        }
    }
}
