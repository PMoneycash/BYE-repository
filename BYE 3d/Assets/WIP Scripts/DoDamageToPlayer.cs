using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageToPlayer : MonoBehaviour
{

    public PlayerHealth playerHealth;
    private int damage = 40;
    public float damageInterval = 2.5f;
    public float damageTimer = 0f;
    public bool playerInContact = false;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInContact = true;
            playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null )
            {
                playerHealth.TakeDamage(damage);
            }

            damageTimer = 0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerInContact && playerHealth != null)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(damage);
                damageTimer = 0f;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInContact = false;
            playerHealth = null;
        }
    }

}
