using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Weapon/Item")]
public class WeaponData : ScriptableObject
{
    public string id;   //id
    
    public string name;     // 이름
    
    public string description;      //설명
    
    public Sprite icon;     //아이콘
    
    public List<WeaponEnhancementData> enhancementTable;        //무기 강화시 이용하는 테이블
}
