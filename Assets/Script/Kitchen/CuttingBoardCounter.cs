
using Unity.Netcode;
using UnityEngine;

public class CuttingBoardCounter : BaseCounter
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private CuttingRecipe[] cuttingRecipes;
    private int cuttingProgress;
    private KitchenObject itemOnTable;
    private GameObject itemOnTableInstance;

    public override void Interact(PlayerInventory player)
    {
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
    public override void Cut(PlayerInventory player)
    { 
        if (itemOnTable != null && !player.HasItem() && HasRecipeWithInput(itemOnTable))
        {
            CutObjectServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CutObjectServerRpc()
    {
        if (itemOnTable == null || !HasRecipeWithInput(itemOnTable)) return;

        cuttingProgress++;
        

        CuttingRecipe cuttingRecipe = GetCuttingRecipeWithInput(itemOnTable);

        if (cuttingProgress >= cuttingRecipe.cuttingProgressMax)
        {
            KitchenObject outputKitchenObject = cuttingRecipe.output;

            if (itemOnTableInstance.TryGetComponent<NetworkObject>(out NetworkObject oldNetObj))
            {
                oldNetObj.Despawn(); 
            }
            
            GameObject slicedInstance = Instantiate(outputKitchenObject.objectPrefab); 
            
            if (slicedInstance.TryGetComponent<NetworkObject>(out NetworkObject newNetObj))
            {
                newNetObj.Spawn(true);
            }

            itemOnTable = outputKitchenObject;
            itemOnTableInstance = slicedInstance;
            
            newNetObj.TrySetParent(counterTopPoint, false);
            itemOnTableInstance.transform.localPosition = new Vector3(0, 1.0f, 0);

            cuttingProgress = 0; 
        }
    }
    private bool HasRecipeWithInput(KitchenObject inputKitchenObject)
    {
        return GetCuttingRecipeWithInput(inputKitchenObject) != null;
    }

    private CuttingRecipe GetCuttingRecipeWithInput(KitchenObject inputKitchenObject)
    {
        foreach (CuttingRecipe recipe in cuttingRecipes)
        {
            if (recipe.input == inputKitchenObject)
            {
                return recipe;
            }
        }
        return null;
    }
}
