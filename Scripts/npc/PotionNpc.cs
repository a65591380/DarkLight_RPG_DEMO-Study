using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotionNpc : CustomNpc {

    public static PotionNpc _instance;
    public GameObject potionShopCanvas;

    private void Awake()
    {
        _instance = this;
    }
    //显示药品商店
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            potionShopCanvas.SetActive(true);
        }
    }
}
