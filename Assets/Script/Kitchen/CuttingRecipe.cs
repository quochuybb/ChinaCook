using UnityEngine;

[CreateAssetMenu(fileName = "New Cutting Recipe", menuName = "Kitchen/Cutting Recipe")]
public class CuttingRecipe : ScriptableObject
{
    public KitchenObject input;     
    public KitchenObject output;    
    public int cuttingProgressMax;  
}