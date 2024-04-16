using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    AudioClip deathSFX;
    public int startingHealth = 100;
    private int currentHealth;
    private GameController gameController;
    Animator animator;

    // Main reason why we need awake is because animator gets created too late 
    // with start, which breaks if we need to take damage as soon as we spawn.
    void Awake()
    {
        /* could do something for getting enemy type based on tag here to determine health amount */
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // if health is 0 or less we die
        if (currentHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            animator.SetBool("isAlive", false);
            GameController.AddMoney(10);
            GameController.AddScore(10);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
