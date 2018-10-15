using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour {
    public static WeaponItem _instance;

    private int id;
    public int Id
    {
        get
        {
            return id;
        }
    }
    private Image icon_image;
    private Text name_text;
    private Text type_text;
    private Text des_text;
    private Text price_text;
    private int price;

    public Text promptText;
    public PlayableDirector promptAnim;
    private void Awake()
    {
        _instance = this;
        icon_image = transform.Find("IconBGImage/IconImage").GetComponent<Image>();
        name_text = transform.Find("NameImage/Text").GetComponent<Text>();
        type_text = transform.Find("TypeImage/Text").GetComponent<Text>();
        des_text = transform.Find("DescribeImage/Text").GetComponent<Text>();
        price_text = transform.Find("PriceImage/Text").GetComponent<Text>();

        promptAnim = GameObject.FindGameObjectWithTag("WeaponPrompt").GetComponent<PlayableDirector>();
        promptText = promptAnim.GetComponentInChildren<Text>();
    }

    public void GetInfoByID(int id)
    {
        this.id = id;
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoByID(id);

        icon_image.sprite = Resources.Load<Sprite>(info.icon_name);
        name_text.text = info.name;
        type_text.text = info.style;
        if(info.atk>0)
        {
            des_text.text = "+" + info.atk + "ATK";
        }
        if(info.def>0)
        {
            des_text.text = "+" + info.def + "DEF";
        }
        if(info.spd>0)
        {
            des_text.text = "+" + info.spd + "SPD";
        }
        price_text.text = info.price_buy + "G";
        price = info.price_buy;
    }

    public void OnBuyButtonClick()
    {
        Debug.Log("BUY");
        if (PlayerStatus._instance.coin >= price)
        {
            InventoryManger._instance.GetItemID(id);
            PlayerStatus._instance.GainCoin(-price);
            promptText.text = "购买成功";
            promptAnim.Play();
        }
        else
        {
            promptText.text = "金币不够";
            promptAnim.Play();
        }
    }

    //隐藏商店
    public void HideWeaponShop()
    {
        promptAnim = GameObject.FindGameObjectWithTag("WeaponPrompt").GetComponent<PlayableDirector>();
        if (promptAnim.time==0)
        WeaponNpc._instance.weaponShopCanvas.SetActive(false);
    }
}
