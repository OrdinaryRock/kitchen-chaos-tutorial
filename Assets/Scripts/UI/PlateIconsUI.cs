using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        foreach (Transform child in transform)
        {
            if(child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (KitchenObjectSO ingredientSO in plateKitchenObject.GetIngredientList())
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<PlateIcon>().SetIngredient(ingredientSO);
        }
    }
}
