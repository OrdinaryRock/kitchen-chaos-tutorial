using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public KitchenObjectSO GetCuttingResult()
    {
        return kitchenObjectSO.cuttingResult;
    }

    public KitchenObjectSO GetFryingResult()
    {
        return kitchenObjectSO.fryingResult;
    }

    public int GetNumberOfCuts()
    {
        return kitchenObjectSO.numberOfCuts;
    }

    public float GetFryingTime()
    {
        return kitchenObjectSO.fryingTime;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null) this.kitchenObjectParent.ClearKitchenObject();
        this.kitchenObjectParent = kitchenObjectParent;
        if(kitchenObjectParent.HasKitchenObject() && kitchenObjectParent.GetKitchenObject() != this)
        {
            Debug.Log("Parent is already full!!!");
        }
        else
        {
            this.kitchenObjectParent.SetKitchenObject(this);
            transform.parent = kitchenObjectParent.GetKitchenObjectSpawnPosition();
            transform.localPosition = Vector3.zero;
            transform.forward = Vector3.forward;
        }
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
