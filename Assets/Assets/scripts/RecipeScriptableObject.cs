using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/* SUMMARY:
 * This class represents the data object associated with a successful Combination. 
 * 
 * It contains data detailing the 2 different Tuples, containing the ItemData types and integer amounts necessary to combine and create
 * an output Tuple of a given ItemData type and integer amount.
 * 
 * Inherits from ScriptableObject so that new Combination recipes can be easily created without the need for a recipe database,
 * from within the Unity inspector, and automatically populate into a folder for easy asset management.
 * 
 * TODO:
 * 1) Create Recipes
 * 2) Figure out how to package multiple output ItemData effectively
 */

[CreateAssetMenu(menuName = "Recipe")]
public class RecipeScriptableObject : ScriptableObject
{
    // Output object
    [SerializeField]
    public ItemData output;

    // Input Objects
    [SerializeField]
    public ItemData item1;
    [SerializeField]
    public ItemData item2;
}
