using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarNpc : CustomNpc {

    public GameObject QuestImage;
    private Text quest_text;
    public static BarNpc _instance;
    public GameObject questCanvas;

    public GameObject okButton;
    public GameObject acceptButton;
    public GameObject cancelButton;
    public GameObject completeButton;
    public Text questText;
    public int coinBonus = 1000;
    public bool isQuestCompleted = false;
    public int killCount=0;
    public int questCount = 2;
    public bool isInTask=false;
    private PlayerStatus playerStatus;

    private void Start()
    {
        quest_text = QuestImage.GetComponentInChildren<Text>();
        _instance = this;
        playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
    }
    //当鼠停留在GUIElement或碰撞器上每一帧调用此方法
    
    private void OnMouseOver()
    {
        //IsPointerOverGameObject()判断当前鼠标停留的物体是否被UI覆盖
        if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        {
            ShowQuest();
        }
    }

    private void Update()
    {
        ShowQuestImage();
    }
    //显示任务
    //一；若任务达成后显示完成按钮
    //二；若任务未达成显示进度面板
    void ShowQuest()
    {
        questCanvas.SetActive(true);
        //任务中（完成，未递交）
        if (isInTask && killCount>=questCount)
        {
            killCount = questCount;
            isQuestCompleted = true;
            QuestProgress();
        }
        else
        {
            isQuestCompleted = false;
        }
        //任务中（完成&递交）
        if (isInTask && isQuestCompleted)
        {
            acceptButton.SetActive(false);
            cancelButton.SetActive(false);
            okButton.SetActive(false);
            completeButton.SetActive(true);
        }
        else if(isInTask)
        {
            okButton.SetActive(true);
            acceptButton.SetActive(false);
            cancelButton.SetActive(false);
            completeButton.SetActive(false);
            QuestProgress();
        }
        else
        {
            okButton.SetActive(false);
            acceptButton.SetActive(true);
            cancelButton.SetActive(true);
            completeButton.SetActive(false);
            QuestProgress();
        }
       
    }

    //取消按钮
    //1.隐藏任务面板
   public void  OnCancelButtonClick()
    {
        questCanvas.SetActive(false);
    }

    //接受按钮
    //1.点击后隐藏 接受 & 取消 按钮
    //2.激活Ok按钮
    //3.显示任务进度
    public void OnAcceptButtonClick()
    {
        isInTask = true;
        okButton.SetActive(true);
        acceptButton.SetActive(false);
        cancelButton.SetActive(false);
        QuestProgress();
    }

    //Ok按钮
    //1.隐藏任务面板
    public void OnOKButtonClick()
    {
        questCanvas.SetActive(false);
    }

    //完成按钮
    //1.任务面板隐藏
    //2.获得奖励
    //3.击杀数归0；
    //再次显示任务；
    public void OnCompleteButtonClick()
    {
        isInTask = false;
        isQuestCompleted = false;
        questCanvas.SetActive(false);
        playerStatus.GainCoin(coinBonus);
        killCount = 0;
        ShowQuest();
    }

    //任务进度
    void QuestProgress()
    {
        if(isInTask)
        {
            questText.text = "\n任务：\n已击败" + killCount + "/" + questCount + "只小狼\n\n奖励：\n1000金币";
        }
        else
        {
            questText.text= "\n任务：\n击败" + questCount + "只小狼\n\n奖励：\n1000金币";
        }
    }

    //任务进度提示面板
    public void ShowQuestImage()
    {
        if(isInTask)
        {
            QuestImage.SetActive(true);
            if (killCount < questCount)
            {
                quest_text.color = Color.white;
            }
            else
            {
                killCount = questCount;
                quest_text.color = Color.yellow;
            }
            
            quest_text.text = killCount + "/" + questCount + "小狼";
        }
        else
        {
            QuestImage.SetActive(false);
        }
        
    }
}
