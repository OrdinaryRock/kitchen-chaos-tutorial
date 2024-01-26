using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnStoveToggled;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private float fryingProgress;
    private float maxFryingTime;

    private void Update()
    {
        if(HasKitchenObject())
        {
            fryingProgress += Time.deltaTime;
            if(maxFryingTime != 0)
            {
                float fryingProgressNormalized = fryingProgress / maxFryingTime;
                UpdateProgressBar(fryingProgressNormalized);
                if(fryingProgress >= maxFryingTime)
                {
                    fryingProgress = 0;
                    UpdateProgressBar(0);
                    if(TurnKitchenObjectIntoItsFriedResult())
                    {
                        maxFryingTime = GetKitchenObject().GetFryingTime();
                    }
                }
            }
        }
    }
    public override void Interact(Player player)
    {
        if(HasKitchenObject())
        {
            if(AttemptGiveKitchenObjectToPlayer(player))
            {
                fryingProgress = 0;
                ToggleStove();
                UpdateProgressBar(0);
            }
        }
        else
        {
            if(AttemptTakeKitchenObjectFromPlayer(player))
            {
                maxFryingTime = GetKitchenObject().GetFryingTime();
                ToggleStove();
            }
        }
    }
    private bool AttemptTakeKitchenObjectFromPlayer(Player player)
    {
        if(player.HasKitchenObject()) player.GetKitchenObject().SetKitchenObjectParent(this);
        else return false;
        return true;
    }
    private bool AttemptGiveKitchenObjectToPlayer(Player player)
    {
        if(!player.HasKitchenObject()) GetKitchenObject().SetKitchenObjectParent(player);
        else return false;
        return true;
    }
    private bool TurnKitchenObjectIntoItsFriedResult()
    {
        KitchenObjectSO fryingResultSO = GetKitchenObject().GetFryingResult();
        GetKitchenObject().DestroySelf();
        if(fryingResultSO != null) KitchenObject.SpawnKitchenObject(fryingResultSO, this);
        else return false;
        return true;
    }
    private void ToggleStove()
    {
        OnStoveToggled?.Invoke(this, EventArgs.Empty);
    }
    private void UpdateProgressBar(float normalizedValue)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(normalizedValue));
    }
}
