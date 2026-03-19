using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Kitchen/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName; 
    public List<KitchenObject> ingredientList; 
}