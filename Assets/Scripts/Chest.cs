using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject healthpack;

    IEnumerator DestroyChest()
    {
        Instantiate(healthpack, transform.position, transform.rotation);
        yield return new WaitForSeconds(.3f);
        Destroy(this.gameObject);
    }

    public void Interact()
    {
        StartCoroutine("DestroyChest");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
