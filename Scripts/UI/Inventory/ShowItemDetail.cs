using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemDetail : MonoBehaviour {

    public static ShowItemDetail _instance;
    private Image ItemDetailImage;
    private Text ItemDetailText;
    private bool isShow = false;
    private void Awake()
    {
        _instance = this;
        ItemDetailImage = this.GetComponent<Image>();
        ItemDetailText = this.GetComponentInChildren<Text>();
        ItemDetailImage.enabled = false;
        ItemDetailText.enabled = false;
    }

  
    //显示物体文本信息
    public void ShowDetail(int id)
    {
        //文本信息位置在物品右上角
        transform.position = new Vector3(Input.mousePosition.x+InventoryItem._instance.GetComponent<RectTransform>().rect.width, Input.mousePosition.y+ InventoryItem._instance.GetComponent<RectTransform>().rect.height, 0);
        transform.SetAsLastSibling();

        ItemDetailImage.enabled = true;
        ItemDetailText.enabled = true;
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoByID(id);
        string content="";
        switch(info.type)
        {
            case ObjectType.Drug:
                content = GetDrugDetail(info);
                break;
            case ObjectType.Equip:
                content = GetEquipDetail(info);
                break;
        }
        ItemDetailText.text = content;
    }
    //隐藏文本信息
    public void HideDetail()
    {
        ItemDetailImage.enabled = false;
        ItemDetailText.enabled = false;
    }
    //设置Drug的信息
    public string GetDrugDetail(ObjectInfo info)
    {
        string str="";
        str += "名称 ：" + info.name + "\n";
        str += "恢复HP ：" + info.hp + "\n";
        str += "恢复MP ：" + info.mp + "\n";
        str += "购买价 ：" + info.price_buy + "\n";
        str += "出售价 ：" + info.price_sell + "\n";

        return str;
    }
    //设置装备信息
    public string GetEquipDetail(ObjectInfo info)
    {
        string str = "";
        str += "名称 ：" + info.name + "\n";
        switch(info.equipType)
        {
            case EquipType.Headgear:
                str += "穿戴类型 : 头盔\n";
                break;
            case EquipType.Armor:
                str += "穿戴类型 : 盔甲\n";
                break;
            case EquipType.L_Hand:
                str += "穿戴类型 : 左手\n";
                break;
            case EquipType.R_Hand:
                str += "穿戴类型 : 右手\n";
                break;
            case EquipType.Shoe:
                str += "穿戴类型 : 鞋子\n";
                break;
            case EquipType.Accessory:
                str += "穿戴类型 : 饰品\n";
                break;
        }
        switch(info.careerType)
        {
            case CareerType.Swordman:
                str += "适用职业 : 剑士\n";
                break;
            case CareerType.Magician:
                str += "适用职业 : 法师\n";
                break;
            case CareerType.Common:
                str += "适用职业 : 通用\n";
                break;
        }
        str += "攻击 : " + info.atk + "\n";
        str += "防御 : " + info.def + "\n";
        str += "速度 : " + info.spd + "\n";
        str += "购买价 ：" + info.price_buy + "\n";
        str += "出售价 ：" + info.price_sell + "\n";
        return str;
    }
}
