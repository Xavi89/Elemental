using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private Vector3 rotation = new Vector3(0,1,0);
    private float speed = 200f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if(playerInventory != null)
        {
            playerInventory.EnergyCollected();
            Destroy(gameObject, 10f);
        }
    }
    private void Update() {
        transform.Rotate( rotation * speed * Time.deltaTime);
    }
}
