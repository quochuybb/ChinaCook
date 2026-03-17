using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObject containerPrefab;

    public override void Interact(PlayerInventory inventory)
    {
        Debug.Log("Interact Container");
        if (!inventory.HasItem())
        {
            GameObject itemInHand = Instantiate(containerPrefab.objectPrefab);
            if (itemInHand.TryGetComponent<NetworkObject>(out NetworkObject netObj))
            {
                netObj.Spawn(true); 
            }
            inventory.PickUpItem(containerPrefab, itemInHand);
            Debug.Log("Picked up Item " + containerPrefab.objectName);
        }
    }
}
