using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public NodeBase nodeBase;
    public Transform target;
    public Vector3 direction;
    public float damage;
    public float speed;
    public float moveDelay = 1.5f;
    public bool isMove;
    public bool isDead;

    private void Start()
    {
        moveDelay += Time.time;
        isMove = false;
        isDead = false;
    }

    private void Update()
    {
        if (!isDead && moveDelay <= Time.time)
        {
            transform.position += direction * speed * Time.deltaTime * GameManager.instance.gameTime;
            isMove = true;
        }
    }

    public void Set(NodeBase nodeBase, Transform target, float damage, float speed)
    {
        this.nodeBase = nodeBase;
        this.target = target;
        this.transform.LookAt(target);
        this.direction = target.position - this.transform.position;
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
            isDead = true;
            Destroy(this.gameObject, 0.5f);
            AudioManager.instance.mainAudioSource.PlayOneShot(GameManager.instance.player.attackSound);
            GameManager.instance.enemy.GetDamage(nodeBase, damage);
        }
    }
}
