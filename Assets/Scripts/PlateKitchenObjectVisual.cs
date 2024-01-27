using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObjectVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        Transform correspondingVisualTransform = transform.Find(e.ingredientSO.objectName);
        if(correspondingVisualTransform != null)
        {
            correspondingVisualTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Could not find a corresponding plate visual for ingredient: " + e.ingredientSO.objectName);
        }
    }
}
