using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int energy {get; private set;}

    public UnityEvent<PlayerInventory> OnEnergyCollected;

    public void EnergyCollected()
    {
        energy ++;
        OnEnergyCollected.Invoke(this);
    }
}
