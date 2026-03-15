using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    private KitchenObject _kitchenObject;
    private GameObject _currentObjectInHand;
    public bool HasItem() => _kitchenObject != null;
    public void PickUpItem(KitchenObject item, GameObject itemInstance)
    {
        _kitchenObject = item;
        _currentObjectInHand = itemInstance;
        
        _currentObjectInHand.transform.SetParent(holdPoint);
        _currentObjectInHand.transform.localPosition = Vector3.zero;
        _currentObjectInHand.transform.localRotation = Quaternion.identity;
    }

    public (KitchenObject item, GameObject itemInstance) DropItem()
    {
        KitchenObject droppedSO = _kitchenObject;
        GameObject droppedInstance = _currentObjectInHand;

        _kitchenObject = null;
        _currentObjectInHand = null;

        return (droppedSO, droppedInstance);
    }
}
