using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject() && player.GetKitchenObject() is PlateKitchenObject)
        {
            PlateKitchenObject plate = player.GetKitchenObject() as PlateKitchenObject;
            DeliveryManager.Instance.DeliveryRecipe(plate);
            plate.DestroySelf();
        }
    }
}
