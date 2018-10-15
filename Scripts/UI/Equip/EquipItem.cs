using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EquipItem : MonoBehaviour,IPointerDownHandler{
    public static EquipItem _instance;

    public int equipID;

    private Image equipImage;
    private PlayerStatus playerStatus;

    private int equipAtk=0;
    private int equipDef=0;
    private int equipSpd=0;

    
    private void Awake()
    {
        _instance = this;
        equipImage = this.GetComponent<Image>();
        playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }
    
    public void SetEquipItem(int id)
    {
        ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoByID(id);
        this.equipID = id;       //设置id
        equipImage.sprite = Resources.Load<Sprite>(equipInfo.icon_name);
        UpdateProperty(id);
       
    }
    public void UpdateProperty(int id)
    {
     
            playerStatus.attack_equip -= equipAtk;
            playerStatus.def_equip -= equipDef;
            playerStatus.speed_equip -= equipSpd;

            ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoByID(id);
            equipAtk = equipInfo.atk;
            equipDef = equipInfo.def;
            equipSpd = equipInfo.spd;
            playerStatus.attack_equip += equipAtk;
            playerStatus.def_equip += equipDef;
            playerStatus.speed_equip += equipSpd;
    

    }
    //卸下装备
    public void TakeOffEquip()
    {
        playerStatus.attack_equip -= equipAtk;
        playerStatus.def_equip -= equipDef;
        playerStatus.speed_equip -= equipSpd;
        InventoryManger._instance.GetItemID(equipID);
        Destroy(this.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TakeOffEquip();
    }
}
