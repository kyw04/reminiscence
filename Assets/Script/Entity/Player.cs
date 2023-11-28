using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase
{
    public Pattern[] patterns;
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnBoxPos;
    public Vector3 bulletSpawnBoxSize;

    public void Attack(NodeBase nodeBase, float damage)
    {
        //animator.Play("Attack");
        SpawnBullet(nodeBase, damage);
    }

    public void SpawnBullet(NodeBase nodeBase, float damage)
    {
        Bullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        newBullet.Set(nodeBase, GameManager.instance.enemy.transform, damage, 10f);
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
}
