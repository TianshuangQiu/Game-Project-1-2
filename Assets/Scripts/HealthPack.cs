using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private int healamount = 3;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("something is inside me uwu");
            collision.transform.GetComponent<PlayerController>().Heal(healamount);
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
