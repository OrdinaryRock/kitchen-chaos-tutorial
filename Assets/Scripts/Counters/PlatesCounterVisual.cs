using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList = new List<GameObject>();

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject topmostPlateVisualGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(topmostPlateVisualGameObject);
        Destroy(topmostPlateVisualGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, spawnPosition);
        float offsetY = 0.1f * plateVisualGameObjectList.Count;
        plateVisualTransform.localPosition = new Vector3(0, offsetY, 0);
        GameObject plateVisualGameObject = plateVisualTransform.gameObject;
        plateVisualGameObjectList.Add(plateVisualGameObject);

    }
}
