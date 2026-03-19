using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;

public class BowlKitchenObject : NetworkBehaviour
{
    [SerializeField] private List<KitchenObject> validIngredientSOList;

    private List<KitchenObject> kitchenObjectSOList = new List<KitchenObject>();

    public event Action<KitchenObject> OnIngredientAdded;

    public bool TryAddIngredient(KitchenObject ingredientSO)
    {
        if (!validIngredientSOList.Contains(ingredientSO)) return false;

        if (kitchenObjectSOList.Contains(ingredientSO)) return false;

        AddIngredientServerRpc(ingredientSO.name);
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(string soName)
    {
        AddIngredientClientRpc(soName);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(string soName)
    {
        KitchenObject addedSO = validIngredientSOList.Find(so => so.name == soName);
        if (addedSO != null)
        {
            kitchenObjectSOList.Add(addedSO);
            
            OnIngredientAdded?.Invoke(addedSO);
        }
    }
    public List<KitchenObject> GetIngredientList()
    {
        return kitchenObjectSOList;
    }
}