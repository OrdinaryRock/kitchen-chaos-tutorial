using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnInteract;


    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(!HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
                // Invoking this event will play our door open and close animation
                OnInteract?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        else
        {
            if(!HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                if(player.GetKitchenObject() is PlateKitchenObject)
                {
                    PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else if(GetKitchenObject() is PlateKitchenObject)
                {
                    PlateKitchenObject plateKitchenObject = GetKitchenObject() as PlateKitchenObject;
                    if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }
}
