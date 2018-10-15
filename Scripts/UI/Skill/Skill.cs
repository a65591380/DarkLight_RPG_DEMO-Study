using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SkillHotKey skill_hotKey;
    private CanvasGroup skill_canvasGroup;

    public int skill_id;
    private SkillInfo skill_info;
    private Image icon_image;
    private Text name_text;
    private Text type_text;
    private Text describe_text;
    private Text mp_text;

    private void Awake()
    {
        icon_image = transform.Find("IconImage").GetComponent<Image>();
        name_text = transform.Find("NameImage/Text").GetComponent<Text>();
        type_text= transform.Find("TypeImage/Text").GetComponent<Text>();
        describe_text= transform.Find("DescribeImage/Text").GetComponent<Text>();
        mp_text= transform.Find("MPImage/Text").GetComponent<Text>();

        //技能未激活
        skill_canvasGroup = GetComponent<CanvasGroup>();
    }
    //设置技能栏信息描述
    public void SetSkillByID(int id)
    {
        this.skill_id = id;
        skill_info = SkillsInfo._instance.GetSkillInfoById(id);
        
        icon_image.sprite = Resources.Load<Sprite>(skill_info.icon_name);
        name_text.text = skill_info.name;
        switch(skill_info.applyType)
        {
            case ApplyType.Passive:
                type_text.text = "增益";
                break;
            case ApplyType.Buff:
                type_text.text = "增强";
                break;
            case ApplyType.SingleTarget:
                type_text.text = "单体";
                break;
            case ApplyType.MultiTarget:
                type_text.text = "群体";
                break;
        }
        describe_text.text = skill_info.des;
        mp_text.text = "<color=#4169e1>"+skill_info.mp+"MP"+"</color>";
    }


    //根据角色等级激活技能
    public void ActiveSkill()
    {
        if(skill_info.level<=PlayerStatus._instance.grade)
        {
            skill_canvasGroup.alpha = 1;
            skill_canvasGroup.blocksRaycasts = true;
        }
        else  //未激活
        {
            skill_canvasGroup.alpha = 0.5f;
            skill_canvasGroup.blocksRaycasts = false;
        }
    }

    /// 拖拽功能（开始）
    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //找到有Canvas组件的物体
        var canvas = FindInParents<Canvas>(gameObject);

        if (canvas == null)
            return;

        //We have clicked something that can be dragged.
        // What we want to do is create an icon for this.
        //给实例化的新GameObject命名
        m_DraggingIcon = new GameObject("New");
        //放到指定路径
        m_DraggingIcon.transform.SetParent(canvas.transform, false);

        //Move the transform to the end of the local transform list.
        //Puts the panel to the front as it is now the last UI element to be drawn.
        m_DraggingIcon.transform.SetAsLastSibling();

        //给新GameObject添加<Image>组件
        var image = m_DraggingIcon.AddComponent<Image>();
        var newIcon= m_DraggingIcon.AddComponent<CanvasGroup>();
        newIcon.blocksRaycasts = false;
        //把当前脚本所挂载的物体的图片赋给新GameObject
        image.sprite = transform.Find("IconImage").GetComponent<Image>().sprite;
        image.SetNativeSize();

        m_DraggingPlane = canvas.transform as RectTransform;


        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_DraggingIcon != null)
            SetDraggedPosition(eventData);
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
    }

    private void SetDraggedPosition(PointerEventData data)
    {

        if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    //销毁图片，设置技能信息
    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcon != null)
            Destroy(m_DraggingIcon);

        //当icon图片销毁后，鼠标指针会检测到当前鼠标指针下方的物体
        //然后给相应热键设置技能信息
        if (eventData.pointerCurrentRaycast.gameObject.name == "KeyImage")
        {
            skill_hotKey = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SkillHotKey>();
            skill_hotKey.SetHotKeySkill(skill_id);
        }
        
        //未做功能
        //快捷键下方是药品则无法摆放
        
    }
    //BUG:拖拽时切出屏幕，icon图标依然会留在游戏内
    //此方法当游戏切出时会调用
    private void OnApplicationPause(bool pause)
    {
        Destroy(m_DraggingIcon);
    }
    //实现不断往相应的上层parent查找所需组件
    //Component: Base class for everything attached to GameObjects.
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        //如果go为null，返回null
        if (go == null) return null;


        //查找go身上相应组件（Canvas）
        //找到后返回comp
        var comp = go.GetComponent<T>();
        if (comp != null)
            return comp;

        //查找t的parent
        //循环查找，不断往上层找parent，直到找到相应组件（Canvas）
        Transform t = go.transform.parent;
        while (t != null && comp == null)       //t有上层parent && 第1步里未找到组件
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    /// 结束拖拽
}
