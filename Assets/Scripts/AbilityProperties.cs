using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProperties : MonoBehaviour
{
    [SerializeField]
    ParticleSystem abilityVFX;

    Dictionary<Constants.AbilityEffect, int> abilityEffects;
    bool activated = false;
    float spellDuration;

    void Awake()
    {
        abilityEffects = new Dictionary<Constants.AbilityEffect, int>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (activated)
        {
            if (other.CompareTag("Enemy") && other != null)
            {
                EnemyHealth enemyHealthScript = other.GetComponent<EnemyHealth>();
                EnemyForcedMovement enemyMovementScript = other.GetComponent<EnemyForcedMovement>();
                foreach (Constants.AbilityEffect abilityEffect in abilityEffects.Keys)
                {
                    switch (abilityEffect)
                    {
                        case Constants.AbilityEffect.DAMAGE:
                            enemyHealthScript.TakeDamage(abilityEffects[abilityEffect]);
                            break;
                        case Constants.AbilityEffect.SLOW:
                            float enemySpeed = enemyMovementScript.speed;
                            float newSpeed = enemySpeed - abilityEffects[abilityEffect];
                            // Cannot slow the enemy down to 0
                            if (newSpeed <= 0)
                            {
                                newSpeed = 1f;
                            }
                            enemyMovementScript.speed = newSpeed;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void SetRadius(float radius)
    {
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public void AddAbilityEffect(Constants.AbilityEffect abilityEffect, int value)
    {
        abilityEffects.Add(abilityEffect, value);
    }


    public void SetSpellDuration(float duration)
    {
        spellDuration = duration;
    }

    public void Activate()
    {
        activated = true;
        ParticleSystem vfx = Instantiate(abilityVFX, transform.position + abilityVFX.transform.position, transform.rotation);
        vfx.transform.parent = transform;
        vfx.transform.localScale = transform.localScale / 10;
        Destroy(gameObject, spellDuration);
    }

}
