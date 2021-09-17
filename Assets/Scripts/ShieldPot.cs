using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("something is inside me uwu");
            collision.transform.GetComponent<PlayerController>().ShieldMe();
            Destroy(this.gameObject);
        }
    }
}
