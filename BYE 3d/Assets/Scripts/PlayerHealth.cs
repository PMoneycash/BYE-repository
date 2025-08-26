using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 400;
    public int currentHealth;
    private bool isDead = false;
    private PlayerMovement2DIn3D playerController;

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerMovement2DIn3D>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
