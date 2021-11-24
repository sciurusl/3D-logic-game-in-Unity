using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int health;
    public int physicalDamage;
    public int currentHealth;
    public float attackSpeed; 
    public bool canBeDamaged = true;
    public bool canAttack = true;
    public bool isWeakened = false;
    public float weakness;
    public float damageMultiplier = 1;
    public float healingMultiplier = 1;
    


    public Animator anim;

    public event Action<float> OnHealthChanged = delegate { };
    public event Action<float> OnEnergyChanged = delegate { };

    /// <summary>
    /// Manages attacks performed on Entity 
    /// </summary>
    /// <param name="damage"></param>
    public void BeAttacked(int damage)
    {
        if (canBeDamaged)
        {
            Debug.Log("obj: " + gameObject.ToString() + " currentHealth should have been: " + (currentHealth - damage));
            if (isWeakened)
                ModifyHealth(-(int)(damage * weakness));
            else
                ModifyHealth(-(int)(damage * damageMultiplier));
            Debug.Log("obj: " + gameObject.ToString() + " currentHealth: " + currentHealth);
            if (currentHealth <= 0 && gameObject != null)
            {
                BeDestroyed();
            }
        }
    }

    /// <summary>
    /// Changes health based on the amount
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyHealth(int amount)
    {
        currentHealth += (int)(amount * healingMultiplier);
        if (currentHealth > health)
        {
            currentHealth = health;
        }
        OnHealthChanged((float)currentHealth / (float)health);
    }


    /// <summary>
    /// Destroys entity when dies
    /// </summary>
    public virtual void BeDestroyed()
    {
      
        Debug.Log(transform.name + " was destroyed.");
        StopAllCoroutines();
        gameObject.SetActive(false);        
    }

    /// <summary>
    /// Destroys entity when its current health is bellow  or equal to zero
    /// </summary>
    private void Update()
    {
     
        if (currentHealth <= 0)
        {
            this.BeDestroyed();
        }
    }


}
