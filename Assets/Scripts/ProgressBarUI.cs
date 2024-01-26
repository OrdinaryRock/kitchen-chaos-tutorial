using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject progressableGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress progressableScript;

    private void Start()
    {
        progressableScript = progressableGameObject.GetComponent<IHasProgress>();
        if(progressableScript == null)
        {
            Debug.LogError("Game Object " + progressableGameObject + " does not have a component that implements IHasProgress");
            return;
        }
        progressableScript.OnProgressChanged += CuttingCounter_OnProgressChanged;
        barImage.fillAmount = 0;
        Hide();
    }
    private void CuttingCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if(barImage.fillAmount == 0)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
