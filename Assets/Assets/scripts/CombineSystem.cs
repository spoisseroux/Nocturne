using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* SUMMARY:
 * This class manages all of the functionality for testing and outputting combinations of ItemData. 
 * 
 * This class is not designed to be attached to any GameObject, and instead functions as a helper within the InventoryUI class,
 * as all combination work is completed within the Inventory UI Manager.
 * 
 * FIGURE OUT HOW TO INITIALIZE THE LIST OF RECIPES
 * 
 * At the moment, the system only supports combinations involving 2 different instances of ItemData. 
 * 3 instances introduces many more cases and checks, which for the scope of the game would not be work implementing.
 * 
 * TODO:
 * 1) Test functionality
 */

public class CombineSystem
{
    [SerializeField] private List<RecipeScriptableObject> recipeList;

    public CombineSystem()
    {
        recipeList = new List<RecipeScriptableObject>();
    }

    // Constructor for the CombineSystem class
    public CombineSystem(List<RecipeScriptableObject> recipes)
    {
        recipeList = recipes;
    }

    // Check fulfillment of and then return the output of a given Recipe
    public Tuple<ItemData, int> GetRecipeOutput(List<Tuple<ItemData, int>> items)
    {
        // How to return a leftover amount in an item with more than recipe requirements?
        // Could just simplify and say the max amount you find in the level is the exact amount needed for a recipe

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

    private bool Matches(Tuple<ItemData, int> recipeItem, Tuple<ItemData, int> inputItem)
    {
        if (recipeItem.Item1 == inputItem.Item1)
        {
            if (recipeItem.Item2 <= inputItem.Item2)
            {
                return true;
            }
        }
        return false;
    }
}
