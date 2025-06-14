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
        finalStatPanel.SetActive(false);
    }

    private void OpenFinalStatPanel()
    {
        finalStatPanelAnimator.Show();
    }

    private void CloseFinalStatPanel()
    {
        finalStatPanelAnimator.Hide();
    }
}
