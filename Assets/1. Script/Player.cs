using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase
{
    public Pattern[] patterns;
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnBoxPos;
    public Vector3 bulletSpawnBoxSize;

    public void Attack(int count, float totalDamage)
    {
        if (count <= 0)
            return;

        //animator.Play("Attack");
    
        for (int i = 0; i < count; i++)
        {
            SpawnBullet(totalDamage / count);
        }
    }

    public void SpawnBullet(float damage)
    {
        Bullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        newBullet.Set(GameManager.instance.enemy.transform, damage, 10f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bulletSpawnBoxPos + transform.position, bulletSpawnBoxSize);
    }
}
