using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    private int currentHealth;
    private GameController gameController;
    Animator animator;
    void Start()
    {
        /* could do something for getting enemy type based on tag here to determine health amount */
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        gameController = GameObject.Find("LevelManager").GetComponent<GameController>();
    }

    public void TakeDamage(int dam)
    {
        currentHealth -= dam;

        // if health is 0 or less we die
        if (currentHealth <= 0 && animator.GetBool("isAlive"))
        {
            animator.SetBool("isAlive", false);
            gameController.AddMoney(10);
            gameController.AddScore(10);
            Debug.Log($"Enemy died. Current money: {gameController.GetMoney()}");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
