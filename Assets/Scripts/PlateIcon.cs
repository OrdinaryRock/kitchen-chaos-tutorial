using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    public void SetIngredient(KitchenObjectSO ingredientSO)
    {
        icon.sprite = ingredientSO.sprite;
    }
}
