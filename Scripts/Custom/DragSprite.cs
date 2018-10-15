using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragSprite : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
        m_DraggingIcon = new GameObject(this.name);
        //放到指定路径
        m_DraggingIcon.transform.SetParent(canvas.transform, false);

        //Move the transform to the end of the local transform list.
        //Puts the panel to the front as it is now the last UI element to be drawn.
        m_DraggingIcon.transform.SetAsLastSibling();

        //给新GameObject添加<Image>组件
        var image = m_DraggingIcon.AddComponent<Image>();
        //把当前脚本所挂载的物体的图片赋给新GameObject
        image.sprite = GetComponent<Image>().sprite;
        image.SetNativeSize();

        m_DraggingPlane = canvas.transform as RectTransform;


        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcon != null)
            SetDraggedPosition(data);
        if(data.pointerCurrentRaycast.gameObject.name=="KeyImage")
        {

        }
        Debug.Log(data.pointerCurrentRaycast.gameObject);
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcon != null)
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
}
