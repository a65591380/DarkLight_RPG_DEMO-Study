using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : MonoBehaviour {

    public static EquipUI _instance;

    public GameObject equipItemPrefab;
    private Canvas equipmentCanvas;

    private GameObject head;
    private GameObject armor;
    private GameObject r_hand;
    private GameObject l_hand;
    private GameObject shoe;
    private GameObject accessory;

    private PlayerStatus playerStatus;

    private void Awake()
    {
        _instance = this;
        equipmentCanvas = this.GetComponent<Canvas>();

        head = transform.Find("EquipImage").Find("HeadImage").gameObject;
        armor = transform.Find("EquipImage").Find("ArmorImage").gameObject;
        r_hand = transform.Find("EquipImage").Find("RHandImage").gameObject;
        l_hand = transform.Find("EquipImage").Find("LHandImage").gameObject;
        shoe = transform.Find("EquipImage").Find("ShoeImage").gameObject;
        accessory = transform.Find("EquipImage").Find("AccessoryImage").gameObject;

        playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }

    //是否能装备物品
    public bool isEquip(int id)
    {
        ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoByID(id);
        if(equipInfo.type!=ObjectType.Equip)
        {
            return false;
        }

        if(playerStatus.career==PlayerCareer.Magician)
        {
            if (equipInfo.careerType == CareerType.Swordman)
                return false;
        }
        if (playerStatus.career == PlayerCareer.Swordman)
        {
            if (equipInfo.careerType == CareerType.Magician)
                return false;
        }
        return true;
    }

    //装备物品
    public void Equip(int id)
    {
        ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoByID(id);
        GameObject parent = null;
        switch (equipInfo.equipType)
        {
            case EquipType.Headgear:
                parent = head;
                break;
            case EquipType.Armor:
                parent = armor;
                break;
            case EquipType.R_Hand:
                parent = r_hand;
                break;
            case EquipType.L_Hand:
                parent = l_hand;
                break;
            case EquipType.Shoe:
                parent = shoe;
                break;
            case EquipType.Accessory:
                parent = accessory;
                break;
        }
        EquipItem equipItem = parent.GetComponentInChildren<EquipItem>();
        if (equipItem != null) //当前部位有装备
        {
            Debug.Log("有装备");
            int currentEquipID = equipItem.equipID;
            equipItem.SetEquipItem(id);
            InventoryManger._instance.GetItemID(currentEquipID);
        }
        else //当前部位没有穿戴装备
        {

            Debug.Log("无装备");
            Instantiate(equipItemPrefab, parent.transform);
            equipItem = parent.GetComponentInChildren<EquipItem>();
            equipItem.SetEquipItem(id);
        }
    }
    public void ShowEquip()
    {
        equipmentCanvas.enabled = true;
    }
    public void HideEquip()
    {
        equipmentCanvas.enabled = false;
    }
}
