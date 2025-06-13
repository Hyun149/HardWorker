using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StatUIManager : 모든 StatUI 항목을 일괄 초기화하는 매니저 클래스
/// - 각 StatUI 컴포넌트에 StatType과 PlayerStat 참조를 전달
/// </summary>
public class StatUIManager : MonoBehaviour
{
    [Header("연결된 시스템")]
    [SerializeField] private PlayerStat playerStat;  // 능력치 계산 및 저장을 담당하는 메인 시스템

    [Header("스탯 UI 초기화")]
    [InspectorName("Stat UIs")]
    [SerializeField] private List<StatUI> statUIs;   // 씬 내 등록된 각 StatUI 오브젝트들

    // ================================
    // 🔁 시작 시 모든 StatUI를 초기화
    // ================================
    private void Start()
    {
        foreach (var ui in statUIs)
        {
            ui.Initialize(ui.StatType, playerStat);  // 각 UI에 타입과 시스템 연결
        }
    }

    /// <summary>
    /// 🔄 모든 StatUI를 새로 갱신
    /// </summary>
    public void RefreshAllUI()
    {
        for (int i = 0; i < statUIs.Count; i++)
        {
            statUIs[i].RefreshUI();
        }
    }

    public void InitializeAll()
    {
        foreach (var ui in statUIs)
        {
            ui.Initialize(ui.StatType, playerStat);
        }
    }
}
