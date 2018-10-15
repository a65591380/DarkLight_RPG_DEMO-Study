using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridLayout : MonoBehaviour {

    public Style equipStyle;
    public GameObject weaponPrefab;

    //根据类型在相应背包实例化物品
    // Use this for initialization
    void Start () {

        foreach (int id in WeaponShop._instance.itemIDList)
        {
            ObjectInfo info = ObjectsInfo._instance.GetObjectInfoByID(id);
            InstantiateWeaponItem(id, info);
        }
    }

    //实例化装备
    public void InstantiateWeaponItem(int id, ObjectInfo info)
    {
        if (equipStyle == info.equipStyle)
        {
            Instantiate(weaponPrefab, this.transform);
            WeaponItem._instance.GetInfoByID(id);
        }
    }

    
}
