using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 무기 스킬의 종류를 정의하는 열거형(enum)입니다.
/// 무기마다 하나의 스킬 타입을 가질 수 있으며, 해당 타입에 따라 스킬 로직이 결정됩니다.
/// </summary>
public enum WeaponSkillType
{
    None,
    Sashimi,
    Cleaver,
    Legend
}
/// <summary>
/// 무기 정보를 담고 있는 ScriptableObject 데이터 클래스입니다.
/// 무기의 이름, 설명, 아이콘, 강화 스탯, 스킬 타입 등을 정의합니다.
/// 게임 내 무기 생성 및 정보 접근에 사용됩니다.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Weapon/Item")]
public class WeaponDataSO : ScriptableObject
{
    public string id;   //id
    
    public string weaponName;     // 이름
    
    public string description;      //설명
    
    public Sprite icon;     //아이콘
    
    public List<WeaponEnhancementData> enhancementTable;        //무기 강화시 이용하는 스탯테이블
    
    public WeaponSkillType skillType; //무기별 스킬타입
}
