using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour {

    public static Status _instance;
    private Canvas statusCanvas;

    private Text atkText;
    private Text defText;
    private Text spdText;
    private Text pointRemainText;
    private Text abilitySummaryText;

    private Button atkButton;
    private Button defButton;
    private Button spdButton;

    private void Awake()
    {
        _instance = this;
        statusCanvas = this.GetComponent<Canvas>();

        atkText = transform.Find("StatusImage").Find("AtkText").GetComponent<Text>();
        defText= transform.Find("StatusImage").Find("DefText").GetComponent<Text>();
        spdText= transform.Find("StatusImage").Find("SpdText").GetComponent<Text>();
        pointRemainText= transform.Find("StatusImage").Find("PointRemainText").GetComponent<Text>();
        abilitySummaryText= transform.Find("StatusImage").Find("AbilitySummaryText").GetComponent<Text>();

        atkButton = transform.Find("StatusImage").Find("AtkButton").GetComponent<Button>();
        defButton= transform.Find("StatusImage").Find("DefButton").GetComponent<Button>();
        spdButton= transform.Find("StatusImage").Find("SpdButton").GetComponent<Button>();

    }

    private void Update()
    {
        UpdateStatus();
    }

    //更新状态面板的显示
    public void UpdateStatus()
    {
        atkText.text = PlayerStatus._instance.attack + "+" + PlayerStatus._instance.attack_plus;
        defText.text = PlayerStatus._instance.def + "+" + PlayerStatus._instance.def_plus;
        spdText.text = PlayerStatus._instance.speed + "+" + PlayerStatus._instance.speed_plus;

        pointRemainText.text = PlayerStatus._instance.point_remain.ToString();

        abilitySummaryText.text = "攻击：" + PlayerStatus._instance.sum_attack + " 防御：" + PlayerStatus._instance.sum_def + " 速度：" + PlayerStatus._instance.sum_speed;

        if(PlayerStatus._instance.point_remain>0)
        {
            
            atkButton.image.enabled = true;
            defButton.image.enabled = true;
            spdButton.image.enabled = true;
        }
        else
        {
            atkButton.image.enabled = false;
            defButton.image.enabled = false;
            spdButton.image.enabled = false;
        }
    }

    public void OnAtkPlusButtonClick()
    {
        bool success = PlayerStatus._instance.GetPoint();
        if(success)
        {
            PlayerStatus._instance.attack_plus++;
            UpdateStatus();
                
        }
    }
    public void OnDefPlusButtonClick()
    {
        bool success = PlayerStatus._instance.GetPoint();
        if (success)
        {
            PlayerStatus._instance.def_plus++;
            UpdateStatus();

        }
    }
    public void OnSpdPlusButtonClick()
    {
        bool success = PlayerStatus._instance.GetPoint();
        if (success)
        {
            PlayerStatus._instance.speed_plus++;
            UpdateStatus();

        }
    }


    //显示状态栏
    public void ShowStatus()
    {
        statusCanvas.enabled = true;
    }

    //隐藏状态栏
    public void HideStatus()
    {
        statusCanvas.enabled = false;
    }
}
