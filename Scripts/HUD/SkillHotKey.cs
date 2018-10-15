using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum HotKeyType
{
    Drug,
    Skill,
    None
}
public class SkillHotKey : MonoBehaviour{

    public KeyCode keycode;
    public HotKeyType type = HotKeyType.None;
    private Image hotKey_image;
    private InventoryItem DrugItem;
    public int id;
    public SkillInfo info=null;
    private PlayerAttack attack;

    public float timer = 0;
    public bool isUse = false;

    private GameObject Mask;
    private void Start()
    {
        attack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
       hotKey_image = this.transform.Find("KeyImage").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keycode) && this.type == HotKeyType.Drug)
        {
            UseDrag();
        }
        else if (Input.GetKeyDown(keycode) && this.type == HotKeyType.Skill && !isUse)
        {
            bool isSuccess = PlayerStatus._instance.UseMp(info.mp);  //蓝是否够
            if (isSuccess)
            {
                if (info.releaseType != ReleaseType.Enemy)
                {
                    attack.UseSkill(info);
                    isUse = true;
                    AddColdMask(info);
                }
                else
                {
                    if (attack.target != null)
                    {
                        attack.UseSkill(info);
                        isUse = true;
                        AddColdMask(info);
                    }
                    else
                    {
                        Debug.Log("请确定目标");
                    }
                }
            }
        }

        if (isUse)  //技能冷却计时
        {
            Mask.GetComponent<Image>().fillAmount= (info.coldTime - timer) / info.coldTime;
            timer += Time.deltaTime;
            if (timer >= info.coldTime)
            {
                timer = 0;
                Destroy(Mask);
                isUse = false;
            }
        }
    }
    //设置技能
    public void SetHotKeySkill(int id)
    {
        bool isSeted= SkillHotKeyController._instance.CheckKey(id);
        if(!isSeted) //判断技能是否冷却
        {
            this.type = HotKeyType.Skill;
            this.id = id;
            this.info = SkillsInfo._instance.GetSkillInfoById(id);
            this.hotKey_image.sprite = Resources.Load<Sprite>(info.icon_name);
        }
       else
        {
            Debug.Log("技能冷却中");
        }
       
    }

    //设置药品信息并使用
    public void UseDrag()
    {
        DrugItem = this.transform.Find("KeyImage").GetComponentInChildren<InventoryItem>();
        if(DrugItem!=null&&DrugItem.itemType==ObjectType.Drug)
        {
         
            this.id = DrugItem.IdItem;
            int hp = DrugItem.Hp;
            int mp = DrugItem.Mp;
            //蓝药
            if (hp == 0 && PlayerStatus._instance.mp_current== PlayerStatus._instance.mp_max)
            {
                Debug.Log("蓝满了");
                return;

            }
            //血药
            if (mp == 0 && PlayerStatus._instance.hp_current == PlayerStatus._instance.hp_max)
            {
                Debug.Log("血满了");
                return;

            }
            PlayerStatus._instance.GetDrug(hp, mp);
            DrugItem.ItemNumMinus();
        }
        else
        {
            Debug.Log("无效果");
        }
    }

    //添加冷却遮罩
    public void AddColdMask(SkillInfo info)
    {
        Mask = new GameObject();
        Mask.transform.SetParent(hotKey_image.gameObject.transform);
        Image mask_image= Mask.AddComponent<Image>();

        //以下代码会让子物体比父物体小一点
        //Mask.GetComponent<RectTransform>().anchorMax = hotKey_image.rectTransform.anchorMax;
        //Mask.GetComponent<RectTransform>().anchorMin = hotKey_image.rectTransform.anchorMin;
        //Mask.GetComponent<RectTransform>().sizeDelta = hotKey_image.rectTransform.sizeDelta;
        Mask.GetComponent<RectTransform>().anchoredPosition = hotKey_image.rectTransform.anchoredPosition;
        Mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hotKey_image.GetComponent<Image>().GetPixelAdjustedRect().width);
        Mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hotKey_image.GetComponent<Image>().GetPixelAdjustedRect().height);
        mask_image.sprite = this.hotKey_image.sprite;
        mask_image.color = Color.gray;
        mask_image.type = Image.Type.Filled;
        mask_image.fillClockwise = false;
        mask_image.fillOrigin = 2;
    }
}
