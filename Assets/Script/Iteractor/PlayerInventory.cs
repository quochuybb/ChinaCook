using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    [SerializeField] private Transform holdPoint;
    private KitchenObject _kitchenObject;
    private GameObject _currentObjectInHand;
    public bool HasItem() => _kitchenObject != null;
    public void PickUpItem(KitchenObject item, GameObject itemInstance)
    {
        _kitchenObject = item;
        _currentObjectInHand = itemInstance;
        if (_currentObjectInHand.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            netObj.TrySetParent(holdPoint, false);

        }
        else
        {
            _currentObjectInHand.transform.SetParent(holdPoint, false);
        }
        _currentObjectInHand.transform.localPosition = new Vector3(0f, 0.5f, 1f);
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
    public KitchenObject GetCurrentItem()
    {
        return _kitchenObject;
    }
    public GameObject GetCurrentItemInHand()
    {
        return _currentObjectInHand;
    }


    public void DestroyItemInHand()
    {
        if (_currentObjectInHand != null)
        {
            if (_currentObjectInHand.TryGetComponent<NetworkObject>(out NetworkObject netObj))
            {
                DestroyItemServerRpc(new NetworkObjectReference(netObj));
            }

            _kitchenObject = null;
            _currentObjectInHand = null;
        }
    }

    [ServerRpc]
    private void DestroyItemServerRpc(NetworkObjectReference netObjReference)
    {
        if (netObjReference.TryGet(out NetworkObject netObj))
        {
            netObj.Despawn(true);
        }
    }
}
