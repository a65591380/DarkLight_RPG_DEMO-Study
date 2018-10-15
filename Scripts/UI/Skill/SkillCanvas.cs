using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCanvas : MonoBehaviour {
    public static SkillCanvas _instance;

    private Canvas skillCanvas;
    private PlayerStatus playerStatus;
    

    public Skill[] skillList;
    public int[] magicianSkillList;
    public int[] swordmanSkillList;

    private void Awake()
    {
        _instance = this;
        skillCanvas = this.GetComponent<Canvas>();
    }
    private void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
        int[] idList = null;
        switch(playerStatus.career)
        {
            case PlayerCareer.Magician:
                idList = magicianSkillList;
                break;
            case PlayerCareer.Swordman:
                idList = swordmanSkillList;
                break;
        }

        int i = 0;
        //设置技能信息
        foreach (int id in idList)
        {
            skillList[i].SetSkillByID(id);
            i++;
        }
    }
    public void ShowSkill()
    {
        skillCanvas.enabled = true;
        //激活技能
        foreach (Skill skill in skillList)
        {
            skill.ActiveSkill();
        }
        
    }
    public void HideSkill()
    {
        skillCanvas.enabled = false;
    }
}
