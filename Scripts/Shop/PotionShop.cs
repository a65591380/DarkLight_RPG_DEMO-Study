using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class PotionShop : MonoBehaviour {

    private GameObject confirmImage;
    private Text sumPrice;
    private InputField numInput;
    private Text promptText;
    private PlayableDirector promptAnim;

    private int id = 0;
    private int price = 0;
    private int num;

    private void Start()
    {
        promptAnim = this.GetComponentInChildren<PlayableDirector>();
        promptText = GameObject.Find("PromptImage").GetComponentInChildren<Text>();
        confirmImage = GameObject.Find("ConfirmImage");
        sumPrice =confirmImage.transform.Find("SumText").GetComponent<Text>();
        numInput = confirmImage.GetComponentInChildren<InputField>();

        confirmImage.SetActive(false);
        sumPrice.text = "<color=#ffd700ff>0</color>";
    }

    //更新确认面板内的总价显示
    private void  Update()
    {
        if(confirmImage.activeSelf&&numInput.text!="")
        num = int.Parse(numInput.text);
        if (id == 0)
            price = 0;
        else
        {
            price = ObjectsInfo._instance.GetObjectInfoByID(id).price_buy;
        }
        
        
        sumPrice.text = "<color=#ffd700ff>" + num * price + "</color>";
    }

    public void OnBuyButtonClick_ID1001()
    {
        
        ShowConfirmUI();
        id = 1001;
    }
    public void OnBuyButtonClick_ID1002()
    {
        ShowConfirmUI();
        id = 1002;
    }
    public void OnBuyButtonClick_ID1003()
    {
        ShowConfirmUI();
        id = 1003;
    }

    
    public void OnOKButtonClick()
    {
        if (numInput.text!="" && numInput.text!="0")
        {
            int total_price = num * price;
            if(PlayerStatus._instance.coin >= total_price)
            {
                PlayerStatus._instance.GainCoin(-total_price);
                InventoryManger._instance.GetItemID(id, num);
                confirmImage.SetActive(false);
                promptText.text = "购买成功";
                promptAnim.Play();
            }
            else
            {
                promptText.text = "金币不够";
                promptAnim.Play();
            }
        }
        else
        {
            promptText.text = "请输入数字";
            promptAnim.Play();
        }
    }

    
    //显示确认面板
    public void ShowConfirmUI()
    {
        
        confirmImage.SetActive(true);
        numInput.text = "0";
        sumPrice.text = "<color=#ffd700ff>0</color>";
        Update();
    }
    //隐藏商店
    public void HidePotionShop()
    {
        
        numInput.text = "0";
        sumPrice.text = "<color=#ffd700ff>0</color>";
        
        //之前的Bug：提示text播放期间关闭面板，再次打开后Text依然显示（动画强制在Paus状态）
        //在提示动画播放完后才可关闭商店
        //Stupid Way！！！
        if(promptAnim.time==0)
        {
            confirmImage.SetActive(false);
            PotionNpc._instance.potionShopCanvas.SetActive(false);
        }
    }
}
