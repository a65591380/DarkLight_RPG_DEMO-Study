using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//物品类型
public enum ObjectType
{
    Drug,
    Equip,
    Material
}
//装备部位
public enum EquipType
{
    Headgear,
    Armor,
    R_Hand,
    L_Hand,
    Shoe,
    Accessory
}
//装备类型
public enum Style
{
    HeavyArmor,
    Cloth,
    Sword,
    Stick,
    Shoe,
    Ring,
    Torque,
    Shield,
}
//适用职业
public enum CareerType
{
    Swordman,
    Magician,
    Common
}
public class ObjectInfo
{
    public int id;
    public string name;
    public string icon_name;
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int atk;
    public int def;
    public int spd;
    public EquipType equipType;
    public CareerType careerType;
    public Style equipStyle;      //重甲布甲类
    public string style;
}

public class ObjectsInfo : MonoBehaviour {

    public static ObjectsInfo _instance;

    private Dictionary<int, ObjectInfo> objectInfoDictionary = new Dictionary<int, ObjectInfo>();
    public TextAsset objectInfoList;

    public void Awake()
    {
        _instance = this;
        ReadInfo();
    }

   

    public ObjectInfo GetObjectInfoByID(int id)
    {
        ObjectInfo info;
        objectInfoDictionary.TryGetValue(id, out info);

        return info;
    }
    public void ReadInfo()
    {
        string text = objectInfoList.text;
        string[] strArray = text.Split('\n');

        foreach(string str in strArray)
        {
            string[] proArray = str.Split(',');
            
            ObjectInfo info = new ObjectInfo();

            int id = int.Parse(proArray[0]);
            string name = proArray[1];
            string icon_name = proArray[2];
            string str_type = proArray[3];
            ObjectType type = ObjectType.Drug;
            switch(str_type)
            {
                case "Drug":
                    type = ObjectType.Drug;
                    break;
                case "Equip":
                    type = ObjectType.Equip;
                    break;
                case "Material":
                    type = ObjectType.Material;
                    break;
            }

            info.id = id;
            info.name = name;
            info.icon_name = icon_name;
            info.type = type;

            if (type==ObjectType.Drug)
            {
                int hp = int.Parse(proArray[4]);
                int mp = int.Parse(proArray[5]);
                int price_sell = int.Parse(proArray[6]);
                int price_buy = int.Parse(proArray[7]);

                info.hp = hp;
                info.mp = mp;
                info.price_sell = price_sell;
                info.price_buy = price_buy;
            }
            if(type==ObjectType.Equip)
            {
                
                info.atk = int.Parse(proArray[4]);
                info.def=int.Parse(proArray[5]);
                info.spd = int.Parse(proArray[6]);
                int price_sell = int.Parse(proArray[9]);
                int price_buy = int.Parse(proArray[10]);
                info.price_sell = price_sell;
                info.price_buy = price_buy;
                
                string str_equiptype = proArray[7];
                switch(str_equiptype)
                {
                    case "Headgear":
                        info.equipType = EquipType.Headgear;
                        break;
                    case "Armor":
                        info.equipType = EquipType.Armor;
                        break;
                    case "LeftHand":
                        info.equipType = EquipType.L_Hand;
                        break;
                    case "RightHand":
                        info.equipType = EquipType.R_Hand;
                        break;
                    case "Shoe":
                        info.equipType = EquipType.Shoe;
                        break;
                    case "Accessory":
                        info.equipType = EquipType.Accessory;
                        break;
                }

                //在text文本中最后结尾还要加个“，”号，否则无法进入switch循环
                //原因未知，switch（）之前用debug测试输出都是能读取到字符串的
                string str_style = proArray[11];
                info.style=str_style;
                switch (str_style)
                {
                    case "重甲":
                        info.equipStyle = Style.HeavyArmor;
                        break;
                    case "布甲":
                        info.equipStyle = Style.Cloth;
                        break;
                    case "项链":
                        info.equipStyle = Style.Torque;
                        break;
                    case "戒指":
                        info.equipStyle = Style.Ring;
                        break;
                    case "剑":
                        info.equipStyle = Style.Sword;
                        break;
                    case "法杖":
                        info.equipStyle = Style.Stick;
                        break;
                    case "盾":
                        info.equipStyle = Style.Shield;
                        break;
                    case "鞋":
                        info.equipStyle = Style.Shoe;
                        break;
                    default:
                        break;
                }

                string str_careertype = proArray[8];
                switch(str_careertype)
                {
                    case "Swordman":
                        info.careerType = CareerType.Swordman;
                        break;
                    case "Magician":
                        info.careerType = CareerType.Magician;
                        break;
                    case "Common":
                        info.careerType = CareerType.Common;
                        break;
                }
            }

            objectInfoDictionary.Add(id,info);     //添加键值
        }
    }
}


