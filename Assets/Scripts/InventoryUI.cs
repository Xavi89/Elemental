using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI energyText;

    void Start()
    {
        energyText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateEnergy(PlayerInventory playerInventory)
    {
        energyText.text = playerInventory.energy.ToString();
    }
}
