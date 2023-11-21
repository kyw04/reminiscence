using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase
{
    public GameObject bulletPrefab;
    public Pattern[] patterns;

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
}
