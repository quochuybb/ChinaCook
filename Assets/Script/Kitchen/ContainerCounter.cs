using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObject containerPrefab;

    public override void Interact(PlayerInventory inventory)
    {
        if (!inventory.HasItem())
        {
            GameObject itemInHand = Instantiate(containerPrefab.objectPrefab);
            
            inventory.PickUpItem(containerPrefab, itemInHand);
            Debug.Log("Picked up Item " + containerPrefab.objectName);
        }
    }
}
