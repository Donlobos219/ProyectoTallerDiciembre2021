using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CraftingRecipe/baseRecipe")]
public class CraftingRecipes : Item
{
    public Item result;

    public Ingredient[] ingredients;

    private bool CanCraft()
    {
        foreach(Ingredient ingredient in ingredients)
        {
            bool containsCurrentIngredient = Inventory.instance.ContainsItem(ingredient.item, ingredient.amount);

            if (!containsCurrentIngredient)
            {
                return false;
            }
        }

        return true;
    }

    private void RemoveIngredientsFromInventory()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            Inventory.instance.RemoveItems(ingredient.item, ingredient.amount);
        }
    }
    public override void Use()
    {
        if(CanCraft())
        {
            RemoveIngredientsFromInventory();

            Inventory.instance.AddItem(result);
            Debug.Log("Created an Item");
        }
        else
        {
            Debug.Log("You dont have enough ingredients to craft: " + result.name);
        }
    }

    [System.Serializable]
    public class Ingredient
    {
        public Item item;
        public int amount;
    }
}
