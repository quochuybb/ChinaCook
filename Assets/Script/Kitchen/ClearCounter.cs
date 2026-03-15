
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject itemOnTable;
    private GameObject itemOnTableInstance;

    public override void Interact(PlayerInventory player)
    {
        if (itemOnTable == null && player.HasItem())
        {
            var (droppedSO, droppedInstance) = player.DropItem();
            
            itemOnTable = droppedSO;
            itemOnTableInstance = droppedInstance;
            itemOnTableInstance.transform.SetParent(counterTopPoint);
            itemOnTableInstance.transform.localPosition = Vector3.zero;
        }
        else if (itemOnTable != null && !player.HasItem())
        {
            player.PickUpItem(itemOnTable, itemOnTableInstance);
            
            itemOnTable = null;
            itemOnTableInstance = null;
        }
    }
}