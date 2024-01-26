using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private StoveCounter stoveCounter;
    private bool isActive = false;

    private void Start()
    {
        stoveCounter.OnStoveToggled += StoveCounter_OnStoveToggled;
    }

    private void StoveCounter_OnStoveToggled(object sender, System.EventArgs e)
    {
        isActive = !isActive;
        stoveOnGameObject.SetActive(isActive);
        particlesGameObject.SetActive(isActive);
    }
}
