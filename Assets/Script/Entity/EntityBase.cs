using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityBase : MonoBehaviour
{
    public Image hpbar;
    public Animator animator;
    public float maxHealth = 100;
    public float health = 100;
    public bool isDead;
    
    protected virtual void Start()
    {
        isDead = false;
        animator = GetComponent<Animator>();
        HealthImageUpdate();
    }

    public void GetDamage(float value)
    {
        health -= value;
        if (health < 0)
        {
            health = 0;
            isDead = true;
            GameManager.instance.gameState = GameState.End;
        }

        HealthImageUpdate();
    }
    public void HealthImageUpdate()
    {
        hpbar.fillAmount = health / maxHealth;
    }
}
