using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerDownHandler,IPointerUpHandler
{
    public static InventoryItem _instance;
    public ObjectType itemType;

    private float counter = 0f;     //计算摁住时间
    private bool isPress = false;   //是否摁住鼠标
    private int numItem = 1;        //Item数量
    public int NumItem
    {
        get
        {
            return numItem;
        }
        set
        {
            numItem = value;
        }
    }         
    private int idItem = 0;         //Item的ID
    public int IdItem
    {
        get
        {
            return idItem;
        }
        set
        {
            idItem = value;
        }
    }

    private Text numText;       //  Item 数量的text
    private Image itemImage;      //Item UI 图片
    private Transform originalSlot;
    private GameObject parent;
    private GameObject item;
    private float x_item;
    private float y_item;
    private Vector2 itemSize;
    private bool isDragging = false;    //默认设为false,否则OnPointerEnter每帧都会调用，会有bug
    
    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }
    private int mp;
    public int Mp
    {
        get
        {
            return mp;
        }
        set
        {
            mp = value;
        }
    }
    /// <summary>
    /// 添加CanvasGroup组件，在物品拖动时blocksRaycasts设置为false;
    /// 让鼠标的Pointer射线穿过Item物体检测到UI下层的物体信息
    /// </summary>
    private CanvasGroup itemCanvasGroup;

    public void Awake()
    {
        //由于在实例化初始阶段就要获取，因此放在Awake里
        //在Start（）中会报空指针
        itemImage = this.GetComponent<Image>();
        numText = this.GetComponentInChildren<Text>();

        _instance = this;
    }
    private void Start()
    {
        itemCanvasGroup = this.GetComponent<CanvasGroup>();
        item = this.transform.gameObject;
        x_item = item.GetComponent<Image>().GetPixelAdjustedRect().width;    //Image的初始长宽
        y_item = item.GetComponent<Image>().GetPixelAdjustedRect().height;

        parent = GameObject.FindGameObjectWithTag("SlotGrid");
    }

    //减去物品数量
    public void ItemNumMinus(int num = 1)
    {
        numItem -= num;
        numText.text = numItem.ToString();
        if(numItem<1)
        {
            Destroy(this.gameObject);
        }
    }

    //增加物品数量并更新text显示
    public void ItemNumPlus(int num = 1)
    {
        numItem+=num;
        numText.text = numItem.ToString();
    }

    //设置物品具体信息
    //1.根据ID得到物品信息（GetObjectInfoByID(id)方法）
        //2.设置相应的ID号 、图片 、 数量text
    public void SetItemInfo(int id,int count=1)
    {
        ObjectInfo itemInfo = ObjectsInfo._instance.GetObjectInfoByID(id);
        this.idItem = id;       //设置id
        itemImage.sprite = Resources.Load<Sprite>(itemInfo.icon_name);
        numItem = count;
        numText.text = numItem.ToString();
        itemType = itemInfo.type;

        
        hp = itemInfo.hp;
        mp = itemInfo.mp;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("this = " + eventData.pointerCurrentRaycast.gameObject.name);
        //当鼠标在最外层时（移出背包，Canvas外）
        //让物品回到原位
        if (eventData.pointerCurrentRaycast.depth==0 && isDragging==true)
        {
            SetOriginalPos(this.gameObject);
            return;
        }

       
        //Debug.Log(eventData.pointerCurrentRaycast.depth);
        string objectTag = eventData.pointerCurrentRaycast.gameObject.tag;
        
        //Item的拖放逻辑
        if (objectTag!=null && isDragging==true)
        {
            
            if (objectTag == Tags.InventorySlot)   //如果是空格子，则放置Item
            {
                SetCurrentSlot(eventData);
            }
            else if (objectTag == Tags.InventoryItem)      //交换物品
            {
                SwapItem(eventData);
            }
            else   //如果都不是则返回原位
            {
                SetOriginalPos(this.gameObject);
            }
        }

        //设置物品到快捷键
        if (itemType == ObjectType.Drug && eventData.pointerCurrentRaycast.gameObject.name == "KeyImage" && isDragging == true)
        {
            Transform currentSlot = eventData.pointerCurrentRaycast.gameObject.transform;
            currentSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("UIMask");
            currentSlot.GetComponentInParent<SkillHotKey>().type = HotKeyType.Drug;
            this.transform.SetParent(currentSlot);
            //如果只是transform position，图片会默认在左上角顶点处的Anchor
            //因此这里用anchoredPosition让Item图片填充满Slot
            this.GetComponent<RectTransform>().anchoredPosition = currentSlot.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    //把Item回归到原来位置
    public void SetOriginalPos(GameObject gameobject)
    {
        gameobject.transform.SetParent(originalSlot);
        gameobject.GetComponent<RectTransform>().anchoredPosition = originalSlot.GetComponent<RectTransform>().anchoredPosition;
        itemCanvasGroup.blocksRaycasts = true;
    }
    
    //交换两个物体
    //由于拖放中，正被拖放的物体没有Block RayCast
    //具体思路：
        //1.记录当前射线照射到的物体（Item2）
        //2.获取Item2的parent的位置信息，并把item1放过去
        //3.把Item2放到Item1所在的位置
    public void SwapItem(PointerEventData eventData)
    {
        GameObject targetItem = eventData.pointerCurrentRaycast.gameObject;

        //当两个物品相等时，数量相加
        int targetItemID = targetItem.GetComponent<InventoryItem>().IdItem;
        int targetItemNum = targetItem.GetComponent<InventoryItem>().NumItem;
        if (this.idItem == targetItemID)
        {
            ItemNumPlus(targetItemNum);
            Destroy(targetItem);
        }

        //下面这两个方法不可颠倒，否则执行顺序不一样会出bug
        //BUG：先把Item2放到了Item1的位置，此时Item1得到的位置信息是传递后的Item2的（原本Item1的位置）
        //因此会把Item1也放到Item2下，变成都在原本Item1的Slot内
        SetCurrentSlot(eventData);      
        SetOriginalPos(targetItem);
    }

    //设置Item到当前鼠标所在的Slot
    public void SetCurrentSlot(PointerEventData eventData)
    {
        //如果Slot为空
        if (eventData.pointerCurrentRaycast.gameObject.tag==Tags.InventorySlot)
        {
            Transform currentSlot= eventData.pointerCurrentRaycast.gameObject.transform;
            this.transform.SetParent(currentSlot);
            //如果只是transform position，图片会默认在左上角顶点处的Anchor
            //因此这里用anchoredPosition让Item图片填充满Slot
            this.GetComponent<RectTransform>().anchoredPosition = currentSlot.GetComponent<RectTransform>().anchoredPosition;
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == Tags.InventoryItem)
        {
            Transform currentSlot = eventData.pointerCurrentRaycast.gameObject.transform.parent;
            this.transform.SetParent(currentSlot);
            this.GetComponent<RectTransform>().anchoredPosition = currentSlot.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerPress);
        originalSlot = this.GetComponent<Transform>().parent;       //每次拖拽开始前记录初始位置
        isDragging = true;
        itemCanvasGroup.blocksRaycasts = true;
        Transform parentHUD = GameObject.FindGameObjectWithTag("HUD").transform;
        if(InventoryManger._instance.isOpen)
        {
            item.transform.SetParent(parent.transform, false);
        }
        else
        {
            item.transform.SetParent(parentHUD.transform, false);
        }
       
        

        // 将item设置到当前UI层级的最下面（最表面，防止被同一层级的UI覆盖）
        item.transform.parent.SetAsLastSibling();
       
       
        //Item的UI图片自适应改变大小
        item.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x_item);
        item.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y_item);
    }

    public void OnDrag(PointerEventData eventData)
    {
       
        itemCanvasGroup.blocksRaycasts = false;
        DragPos(eventData);
        Debug.Log(eventData.pointerPress);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnPointerEnter(eventData);
        itemCanvasGroup.blocksRaycasts = true;
        isDragging = false;
    }

    //获取鼠标当前位置，并赋给item
    private void DragPos(PointerEventData eventData)
    {
        RectTransform RectItem = item.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(item.transform as RectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            RectItem.position = globalMousePos;
        }
    }

    //摁住1秒后显示文本信息
    private void Update()
    {
        //Image的长宽需要在update里更新一下
        //防止游戏中改变窗口大小后，拖拽时物品Image出现bug，比例失调
        x_item = item.GetComponent<Image>().GetPixelAdjustedRect().width;    
        y_item = item.GetComponent<Image>().GetPixelAdjustedRect().height;

        if (Input.GetMouseButton(0)&&isPress)
        {
            counter += Time.deltaTime;
            if(counter>=1)
            {
                ShowItemDetail._instance.ShowDetail(idItem);
            }
        }
    }

    //装备
    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        Debug.Log("Down");
        if (Input.GetMouseButtonDown(1) && itemType == ObjectType.Equip && EquipUI._instance.isEquip(idItem))
        {
            EquipUI._instance.Equip(idItem);
            ItemNumMinus();
        }
    }
    //释放鼠标后隐藏文本
    public void OnPointerUp(PointerEventData eventData)
    {
        counter = 0;
        isPress = false;
        ShowItemDetail._instance.HideDetail();
    }
}
