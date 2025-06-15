using UnityEngine;
using UnityEngine.UI;

public class FinalStatUIController : MonoBehaviour
{
    [SerializeField] private Button finalStatButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject finalStatPanel;
    [SerializeField] private UIAnimator finalStatPanelAnimator;

    private void Awake()
    {
        finalStatButton.onClick.AddListener(OpenFinalStatPanel);
        backButton.onClick.AddListener(CloseFinalStatPanel);
    }

    private void OpenFinalStatPanel()
    {
        SFXManager.Instance.Play(SFXType.UIShow);
        finalStatPanelAnimator.Show();
    }

    private void CloseFinalStatPanel()
    {
        SFXManager.Instance.Play(SFXType.UIShow);
        finalStatPanelAnimator.Hide();
    }
}
