using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public KitchenObjectSO cuttingResult;
    public int numberOfCuts = 10;
    public Sprite sprite;
    public string objectName;
}
