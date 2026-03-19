using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public static DeliveryManager instance;

    [Header("Dữ liệu")]
    [SerializeField] private List<Recipe> recipeListSO; 

    private List<Recipe> waitingRecipeSOList = new List<Recipe>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (IsServer)
        {
            SpawnNewOrder();
        }
    }

    private void SpawnNewOrder()
    {
        Recipe randomRecipe = recipeListSO[Random.Range(0, recipeListSO.Count)];
        waitingRecipeSOList.Add(randomRecipe);
        Debug.Log($"[ĐƠN HÀNG MỚI]: Khách đang chờ món {randomRecipe.recipeName}");
    }

    public void DeliverRecipe(BowlKitchenObject bowl)
    {
        List<KitchenObject> plateIngredients = bowl.GetIngredientList();
        bool isCorrect = false;

        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            Recipe waitingRecipe = waitingRecipeSOList[i];

            if (waitingRecipe.ingredientList.Count == plateIngredients.Count)
            {
                bool plateMatchesRecipe = true;

                foreach (KitchenObject recipeIngredient in waitingRecipe.ingredientList)
                {
                    if (!plateIngredients.Contains(recipeIngredient))
                    {
                        plateMatchesRecipe = false;
                        break;
                    }
                }

                if (plateMatchesRecipe)
                {
                    isCorrect = true;
                    waitingRecipeSOList.RemoveAt(i);
                    break;
                }
            }
        }

        // 4. In kết quả
        if (isCorrect)
        {
            Debug.Log("==== ĐÚNG RỒI! GIAO HÀNG THÀNH CÔNG! +100 ĐIỂM ====");
            SpawnNewOrder(); 
        }
        else
        {
            Debug.Log("==== SAI RỒI! KHÁCH KHÔNG GỌI MÓN NÀY! TRỪ ĐIỂM ====");
        }
    }
}