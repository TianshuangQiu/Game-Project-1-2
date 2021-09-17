using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    private void Update() {
        Vector3 newPos = player.transform.position;
        newPos.z = -10;
        transform.position = newPos;
    }
}
