using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponSkillType
{
    None,
    Sashimi,
    Cleaver,
    Legend
}

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
