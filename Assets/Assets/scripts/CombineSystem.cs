using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/* SUMMARY:
 * This class manages all of the functionality for testing and outputting combinations of ItemData. 
 * 
 * This class is not designed to be attached to any GameObject, and instead functions as a helper within the InventoryUI class,
 * as all combination work is completed within the Inventory UI Manager.
 * 
 * At the moment, the system only supports combinations involving 2 different instances of ItemData. 
 * 3 instances introduces many more cases and checks, which for the scope of the game would not be worth implementing.
 */

public class CombineSystem
{
    [SerializeField]
    private List<RecipeScriptableObject> recipeList;

    // Construct and load all RecipeScriptableObjects into the CombineSystem class
    public CombineSystem()
    {
        // Full list of all the recipe names to be loaded
        List<string> recipeNames = new List<string>()
        {
            "BatteryRecipe",
            "BlueDyeRecipe",
            "MakeupPenRecipe"
        };

        // Create and populate the list of Recipes
        recipeList = new List<RecipeScriptableObject>();
        foreach (string recipeName in recipeNames)
        {
            RecipeScriptableObject asset = Resources.Load<RecipeScriptableObject>("Recipes/" + recipeName);
            if (asset != null)
            {
                recipeList.Add(asset);
            }
        }
        
    }

    public int CheckRecipeCount()
    {
        return recipeList.Count;
    }

    // Check fulfillment of and then return the output of a given Recipe
    public ItemData GetRecipeOutput(List<ItemData> items)
    {
        // CURRENTLY DOES NOT SUPPORT AMOUNT-SPECIFIC RETURNS

        foreach (RecipeScriptableObject recipe in recipeList)
        {
            // Check Case 1: recipe item1 --> input item1 && recipe item2 --> input item2
            if (Matches(recipe.item1, items[0]) && Matches(recipe.item2, items[1]))
            {
                return recipe.output;
            }

            // Check Case 2 (if necessary): recipe item1 --> input item2 && recipe item2 --> input item1
            if (Matches(recipe.item1, items[1]) && Matches(recipe.item2, items[0]))
            {
                return recipe.output;
            }
        }
        return null;
    }

    private bool Matches(ItemData recipeItem, ItemData inputItem)
    {
        if (recipeItem == inputItem)
        {
            return true;
        }
        return false;
    }
}
