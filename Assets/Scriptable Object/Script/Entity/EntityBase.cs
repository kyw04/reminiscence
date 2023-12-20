using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityBase : MonoBehaviour
{
    public AudioClip attackSound;
    public Image[] hpbar;
    public Animator animator;
    public NodeBase nodeBase;
    public float maxHealth = 100;
    public float health = 100;
    public int power = 10;
    public float attackSpeed = 1.25f;
    public ParticleSystem attackParticle;
    public bool isDead;
    
    protected virtual void Start()
    {
        isDead = false;
        animator = GetComponent<Animator>();
        HealthImageUpdate();
    }

    public virtual void GetDamage(NodeBase attackerNodeBase, float damage)
    {
        animator.SetTrigger("Hit");
        health -= nodeBase.GetTotalDamage(attackerNodeBase, damage);
        Debug.Log($"GetDamage.. current health: {health}");
        if (health < 0)
        {
            health = 0;
            isDead = true;
            Death();
            GameManager.instance.gameState = GameState.End;
            AugmentActive.instance.AugmentExecute(Augment.ActionType.SceneEnd);
        }

        HealthImageUpdate();
    }
    public void GetDamage(float damage)
    {
        animator.SetTrigger("Hit");
        health -= damage;
        Debug.Log($"GetDamage.. current health: {health}");
        if (health < 0)
        {
            health = 0;
            isDead = true;
            Death();
            GameManager.instance.gameState = GameState.End;
            AugmentActive.instance.AugmentExecute(Augment.ActionType.SceneEnd);
        }

        HealthImageUpdate();
    }
    public virtual void HealthImageUpdate()
    {
        foreach (Image _hpbar in hpbar)
        {
            _hpbar.fillAmount = health / maxHealth;
        }
    }

    public virtual void Death() {  }

    public IEnumerator PlayAttackSound(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("죽어 김유한");
        AudioManager.instance.mainAudioSource.PlayOneShot(attackSound);
    }
}
