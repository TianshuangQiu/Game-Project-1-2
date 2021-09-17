using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public float movespeed;
    public Transform player;

    Rigidbody2D EnemyRB;

    public float ExplosionDamage;
    public float ExplosionRadius;
    public GameObject Explosionobj;

    public float maxHealth;
    float currHealth;

    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) {
            return; 
        }

        Move();
    }

    private void Move()
    {
        Vector2 direction = player.position - transform.position;

        EnemyRB.velocity = direction.normalized * movespeed;
    }

    private void Explode()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, ExplosionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("hitting player with explosion");
                Instantiate(Explosionobj, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(ExplosionDamage);

            }
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")){
            Explode();
        }
    }

    public void TakeDamage(float value)
    {
        FindObjectOfType<AudioManager>().Play("BatHurt");

        currHealth -= value;
        Debug.Log("Health is now" + currHealth.ToString());

        if(currHealth <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().WinGame();
    }

}
