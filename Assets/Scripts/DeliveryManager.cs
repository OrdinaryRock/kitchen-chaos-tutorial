using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> pendingRecipeSOList = new List<RecipeSO>();
    private int maximumPendingRecipes = 4;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if(pendingRecipeSOList.Count < maximumPendingRecipes)
            {
                RecipeSO newRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                pendingRecipeSOList.Add(newRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        // For each of our pending recipes
        for(int i = 0; i < pendingRecipeSOList.Count; i++)
        {
            RecipeSO recipeSO = pendingRecipeSOList[i];
            if(recipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetIngredientList().Count)
            {
                bool plateMatchesRecipe = true;
                // For each ingredient in pending recipe
                foreach(KitchenObjectSO ingredient in recipeSO.kitchenObjectSOList)
                {
                    bool ingredientMatched = false;
                    // For each ingredient on plate
                    foreach(KitchenObjectSO plateIngredient in plateKitchenObject.GetIngredientList())
                    {
                        if(ingredient == plateIngredient)
                        {
                            ingredientMatched = true;
                            break;
                        }
                    }
                    if(ingredientMatched)
                    {
                        plateMatchesRecipe = true;
                    }
                }
                if(plateMatchesRecipe)
                {
                    pendingRecipeSOList.Remove(recipeSO);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        Debug.Log("Nobody asked for that");
    }

    public List<RecipeSO> GetPendingRecipeSOList()
    {
        return pendingRecipeSOList;
    }
}
