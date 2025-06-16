
using System;
using UnityEngine;
/// <summary>
/// Pool을 관리하는 interface입니다.
/// </summary>
public interface IPoolable
{
    void Init(Action<GameObject> returnAction); // 초기화
    void OnSpawn(); 
    void OnDespawn(); 
}
