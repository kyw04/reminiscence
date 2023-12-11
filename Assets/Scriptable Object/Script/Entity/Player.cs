using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : EntityBase
{
    public Pattern[] patterns;
    public Vector3 bulletSpawnBoxPos;
    public Vector3 bulletSpawnBoxSize;

    bool IsAttack = false;
    public void Attack(NodeBase nodeBase, float damage)
    {
        //animator.Play("Attack");
        animator.SetTrigger("IsAttack");
        StartCoroutine(SpawnBullet(nodeBase, damage));
    }

    public IEnumerator SpawnBullet(NodeBase nodeBase, float damage)
    {
        Debug.Log("spawn bullet");
        float x = UnityEngine.Random.Range(-bulletSpawnBoxSize.x / 2, bulletSpawnBoxSize.x / 2);
        float y = UnityEngine.Random.Range(-bulletSpawnBoxSize.y / 2, bulletSpawnBoxSize.y / 2);
        float z = UnityEngine.Random.Range(-bulletSpawnBoxSize.z / 2, bulletSpawnBoxSize.z / 2);
        float speed = UnityEngine.Random.Range(1.5f, 3.0f);
        Vector3 randomPos = new Vector3(x, y, z);

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.75f));

        Bullet newBullet = Instantiate(nodeBase.AttackParticle, bulletSpawnBoxPos + transform.position + randomPos, Quaternion.identity).GetComponent<Bullet>();
        newBullet.Set(nodeBase, GameManager.instance.enemy.transform, damage, speed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bulletSpawnBoxPos + transform.position, bulletSpawnBoxSize);
    }
    public override void Death()
    {

        GameManager.instance.EndBattle(false);
        
    }
    public override void GetDamage(NodeBase attackerNodeBase, float damage)
    {
        GameStateManager.Instance.health -= nodeBase.GetTotalDamage(attackerNodeBase, damage);
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


    protected override void Start()
    {
        base.Start();

        maxHealth = GameStateManager.Instance.maxHealth;
        health = GameStateManager.Instance.health;

        if (GameStateManager.Instance.playerLoseMdoe) health = 1f;
        else
        {
            
        }
    }
}
