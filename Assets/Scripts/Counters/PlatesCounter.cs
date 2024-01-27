using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateSO;
    private float spawnPlateTimer = 0f;
    private float timeBetweenSpawns = 4f;
    private int numberOfStoredPlates = 0;
    private int maximumNumberOfStoredPlates = 4;

    private void Update()
    {
        if(numberOfStoredPlates < maximumNumberOfStoredPlates) spawnPlateTimer += Time.deltaTime;

        if(spawnPlateTimer >= timeBetweenSpawns)
        {
            spawnPlateTimer = 0;
            if(numberOfStoredPlates < maximumNumberOfStoredPlates)
            {
                numberOfStoredPlates++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(numberOfStoredPlates > 0)
            {
                numberOfStoredPlates--;
                KitchenObject.SpawnKitchenObject(plateSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
