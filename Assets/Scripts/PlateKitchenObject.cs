using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO ingredientSO;
        public OnIngredientAddedEventArgs(KitchenObjectSO newIngredientSO)
        {
            ingredientSO = newIngredientSO;
        }
    }

    [SerializeField] private List<KitchenObjectSO> validIngredientList = new List<KitchenObjectSO>();
    private List<KitchenObjectSO> ingredientList = new List<KitchenObjectSO>();

    public bool TryAddIngredient(KitchenObjectSO newIngredientSO)
    {
        if(validIngredientList.Contains(newIngredientSO) && !ingredientList.Contains(newIngredientSO))
        {
            ingredientList.Add(newIngredientSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs(newIngredientSO));
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<KitchenObjectSO> GetIngredientList()
    {
        return ingredientList;
    }
}
