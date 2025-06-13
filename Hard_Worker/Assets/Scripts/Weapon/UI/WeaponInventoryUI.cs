using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryUI : MonoBehaviour
{
    public Transform slotContainer; // SlotContainer
    public GameObject slotPrefab;

    public void RenderInventory()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var data in WeaponInventory.Instance.GetAllWeaponData())
        {
            Debug.Log(data.name);
            var owned = WeaponInventory.Instance.GetWeapons().Find(w => w.GetData().id == data.id);
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            WeaponSlot slot = slotObj.GetComponent<WeaponSlot>();
            slot.SetupSlot(data, owned);
        }
    }
}
