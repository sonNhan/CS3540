using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    int currentHealth;
    Animator animator;
    void Start()
    {
        /* could do something for getting enemy type based on tag here to determine health amount */
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int dam) 
    {
        currentHealth -= dam;

        // if health is 0 or less we die
        if (currentHealth <= 0)
        {
            animator.SetBool("isAlive", false);
        }
    }
}
