
using Unity.Netcode;
using UnityEngine;

public class TrashCanCounter : BaseCounter
{
    public override void Interact(PlayerInventory player)
    {
        if (player.HasItem())
        {
            var (droppedSO, droppedInstance) = player.DropItem();

            if (droppedInstance.TryGetComponent<NetworkObject>(out NetworkObject netObj))
            {
                DestroyObjectServerRpc(new NetworkObjectReference(netObj));
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc(NetworkObjectReference netObjReference)
    {
        if (netObjReference.TryGet(out NetworkObject netObj))
        {
            netObj.Despawn(true);
            
            Debug.Log("[Server] Đã tiêu hủy thành công một món đồ trong thùng rác!");
        }
    }

}
