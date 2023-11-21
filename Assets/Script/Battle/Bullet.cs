using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public Vector3 direction;
    public float damage;
    public float speed;
    public bool isMove;

    private void Start()
    {
        isMove = false;
    }

    private void Update()
    {
        if (isMove)
        {
            transform.position += direction * speed * Time.deltaTime * GameManager.instance.gameTime;
        }
    }

    public void Set(Transform target, float damage, float speed)
    {
        this.target = target;
        this.transform.LookAt(target);
        this.direction = this.transform.position - target.position;
        this.damage = damage;
        this.speed = speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isMove)
            return;

        isMove = false;
        if (collision.transform.GetComponent<Enemy>())
        {
            GameManager.instance.enemy.GetDamage(damage);
        }
    }
}
