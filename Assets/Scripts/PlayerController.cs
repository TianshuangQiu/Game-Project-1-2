using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float movespeed;
    float x_input;
    float y_input;

    Rigidbody2D PlayerRB;

    public float Damage;
    public float attackspeed = 1;
    float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Vector2 currDirection;

    Animator anim;

    public float maxHealth;
    float currHealth;
    public Slider HPSlider;
    public Text txt;
    public Image img;

    private bool shielded;
    private bool enraged = false;
    private float enragedTime;
    private void Awake()
    {
        shielded = false;
        PlayerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
        HPSlider.value = currHealth / maxHealth;
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }
        if (shielded)
        {
            txt.text = "Shields: On";
        }
        else
        {
            txt.text = "Shields: Off";
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enraged = !enraged;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
        Move();
        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Interact();
        }
    }

    private void Move()
    {
        anim.SetBool("Moving", true);
        if (enraged)
        {
            img.CrossFadeAlpha(0.4f, 2f, true);
            movespeed = 10f;
            if(enragedTime > 1)
            {
                TakeDamage(1);
                enragedTime = 0f;
            }
            else
            {
                enragedTime += Time.deltaTime;
            }
        }
        else
        {
            enragedTime = 0f;
            img.CrossFadeAlpha(0f, 0f, true);
            movespeed = 2;
        }
        if (x_input > 0)
        {
            PlayerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
            
        } else if (x_input < 0)
        {
            PlayerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
        else if (y_input > 0)
        {
            PlayerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        }
        else if (y_input < 0)
        {
            PlayerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
    }

    private void Attack()
    {
        attackTimer = attackspeed;
        Debug.Log("Attacking" + currDirection);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;
        anim.SetTrigger("Attacking");
        FindObjectOfType<AudioManager>().Play("PlayerAttack");
        yield return new WaitForSeconds(hitboxtiming);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Tons of damage");
                hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
            }
            Debug.Log(hit.transform.name);
        }
        yield return new WaitForSeconds(hitboxtiming);

        isAttacking = false;
        yield return null;


    }

    public void TakeDamage(float value)
    {
        if (shielded)
        {
            shielded = false;
            return;
        }
        FindObjectOfType<AudioManager>().Play("PlayerHurt");

        currHealth -= value;
        Debug.Log("healt is now" + currHealth);

        //change UI
        HPSlider.value = currHealth / maxHealth;
        
        //die

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float value)
    {
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);
        HPSlider.value = currHealth / maxHealth;
    }

    private void Die()
    {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }

    private void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0f);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }

    public void ShieldMe()
    {
        shielded = true;
    }
}