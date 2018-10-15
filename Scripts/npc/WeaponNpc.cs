using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponNpc : CustomNpc {

    public static WeaponNpc _instance;
    public GameObject weaponShopCanvas;
    public Toggle sword_toggle;
    public GameObject swordBag;

    private void Awake()
    {
        _instance = this;
    }
    //显示武器商店
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {

            weaponShopCanvas.SetActive(true);
            sword_toggle.isOn = true;
            swordBag.SetActive(true);
        }
    }
}
