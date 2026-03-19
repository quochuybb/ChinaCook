using Unity.Netcode;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(PlayerInventory player)
    {
        if (player.HasItem())
        {
            if (player.GetCurrentItemInHand().TryGetComponent<BowlKitchenObject>(out BowlKitchenObject plate))
            {
                DeliverPlateServerRpc(plate.NetworkObjectId);
                
                player.DestroyItemInHand();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverPlateServerRpc(ulong plateNetworkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(plateNetworkObjectId, out NetworkObject plateNetObj))
        {
            if (plateNetObj.TryGetComponent<BowlKitchenObject>(out BowlKitchenObject plate))
            {
                DeliveryManager.instance.DeliverRecipe(plate);
            }
        }
    }
}