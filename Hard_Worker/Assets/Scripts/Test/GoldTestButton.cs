using UnityEngine;
using UnityEngine.UI;

public class GoldTestButton : MonoBehaviour
{
    [SerializeField] private Button addButton;

    private void Awake()
    {
        addButton.onClick.AddListener(() => GoldManager.Instance.AddGold(100000));
    }
}
