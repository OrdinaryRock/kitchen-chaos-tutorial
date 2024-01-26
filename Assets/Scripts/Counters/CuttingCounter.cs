using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private int cuttingProgress = 0;
    private int maxCuts = 0;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                maxCuts = GetKitchenObject().GetNumberOfCuts();                
            }
        }
        else
        {
            if(!player.HasKitchenObject())
            {
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0));
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject())
        {
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            if(cuttingProgress >= GetKitchenObject().GetNumberOfCuts())
            {
                cuttingProgress = 0;
                KitchenObjectSO cuttingResultSO = GetKitchenObject().GetCuttingResult();
                GetKitchenObject().DestroySelf();
                if(cuttingResultSO != null)
                {
                    KitchenObject.SpawnKitchenObject(cuttingResultSO, this);
                    maxCuts = GetKitchenObject().GetNumberOfCuts();
                }
            }
            float cuttingProgressNormalized = (float) cuttingProgress / maxCuts;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(cuttingProgressNormalized));
        }
    }
}
