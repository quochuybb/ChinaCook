
using Unity.Netcode;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject itemOnTable;
    private GameObject itemOnTableInstance;

    public override void Interact(PlayerInventory player)
    {
        Debug.Log("Clear Counter");
        if (itemOnTable == null && player.HasItem())
        {
            var (droppedSO, droppedInstance) = player.DropItem();
            
            itemOnTable = droppedSO;
            itemOnTableInstance = droppedInstance;
            if (itemOnTableInstance.TryGetComponent<NetworkObject>(out NetworkObject netObj))
            {
                netObj.TrySetParent(counterTopPoint, false);

            }
            else
            {
                itemOnTableInstance.transform.SetParent(counterTopPoint, false);
            }
            itemOnTableInstance.transform.localPosition = new Vector3(0,1.0f,0);
        }
        else if (itemOnTable != null && !player.HasItem())
        {
            player.PickUpItem(itemOnTable, itemOnTableInstance);
            
            itemOnTable = null;
            itemOnTableInstance = null;
        }
    }
}