using System;
using System.Collections.Generic;
using UnityEngine;

public class BowlCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct IngredientToGameObject
    {
        public KitchenObject kitchenObjectSO;
        public GameObject gameObject; 
    }

    [SerializeField] private BowlKitchenObject bowlKitchenObject;
    [SerializeField] private List<IngredientToGameObject> ingredientGameObjectList;

    private void Start()
    {
        bowlKitchenObject.OnIngredientAdded += BowlKitchenObject_OnIngredientAdded;

        foreach (IngredientToGameObject item in ingredientGameObjectList)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void BowlKitchenObject_OnIngredientAdded(KitchenObject addedSO)
    {
        foreach (IngredientToGameObject item in ingredientGameObjectList)
        {
            if (item.kitchenObjectSO == addedSO)
            {
                item.gameObject.SetActive(true);
                break;
            }
        }
    }
}