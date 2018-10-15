using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour {

	public void OnStatusButtonClick()
    {
        Status._instance.ShowStatus();
    }

    public void OnSkillButtonClick()
    {
        SkillCanvas._instance.ShowSkill();
    }

    public void OnEquipButtonClick()
    {
        EquipUI._instance.ShowEquip();
    }

    public void OnBagButtonClick()
    {
        InventoryManger._instance.ShowInventory();
    }

    public void OnSettingButtonClick()
    {
        SettingCanvas._instance.ShowSetting();
    }
}
