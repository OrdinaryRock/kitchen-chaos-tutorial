using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
    }

    private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == clearCounter)
        {
            ShowVisualGameObject();
        }
        else
        {
            HideVisualGameObject();
        }
    }

    private void ShowVisualGameObject()
    {
        visualGameObject.SetActive(true);
    }

    private void HideVisualGameObject()
    {
        visualGameObject.SetActive(false);
    }
}
