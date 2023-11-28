using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityBase : MonoBehaviour
{
    public Image hpbar;
    public Animator animator;
    public NodeBase nodeBase;
    public float maxHealth = 100;
    public float health = 100;
    public bool isDead;
    
    protected virtual void Start()
    {
        isDead = false;
        animator = GetComponent<Animator>();
        HealthImageUpdate();
    }

    public void GetDamage(NodeBase attackerNodeBase, float damage)
    {
        health -= nodeBase.GetTotalDamage(attackerNodeBase, damage);
        Debug.Log($"GetDamage.. current health: {health}");
        if (health < 0)
        {
            health = 0;
            isDead = true;
            Death();
            GameManager.instance.gameState = GameState.End;
        }

        HealthImageUpdate();
    }
    public void HealthImageUpdate()
    {
        hpbar.fillAmount = health / maxHealth;
    }

    public virtual void Death() { }
}
