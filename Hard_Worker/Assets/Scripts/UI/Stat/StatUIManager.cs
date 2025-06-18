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
    
    /// <summary>
    /// 스탯UI 켜질때 자동 갱신
    /// </summary>
    private void Start()
    {
        foreach (var ui in statUIs)
        {
            ui.Initialize(ui.StatType, playerStat);  // 각 UI에 타입과 시스템 연결
        }
    }

    /// <summary>
    /// 모든 StatUI의 내용을 최신 상태로 갱신합니다.
    /// - 주로 외부에서 능력치 변경이 발생했을 때 호출됩니다.
    /// </summary>
    public void RefreshAllUI()
    {
        for (int i = 0; i < statUIs.Count; i++)
        {
            statUIs[i].RefreshUI();
        }
    }

    /// <summary>
    /// 모든 StatUI를 수동으로 초기화합니다.
    /// - StatType이나 PlayerStat이 런타임에 변경되었을 경우 재초기화에 사용합니다.
    /// </summary>
    public void InitializeAll()
    {
        foreach (var ui in statUIs)
        {
            ui.Initialize(ui.StatType, playerStat);
        }
    }
}
