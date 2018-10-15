using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour {

    public static InventoryManger _instance;
    /// <summary>
    ///无法获取还未实例化的物体（目前不知道具体方法）
    /// 因此用public来获取物体的Prefab
    /// </summary>
    public InventoryItem item;
    public List<GameObject> inventorySlotList = new List<GameObject>();
    public Text coinText;

    private Canvas inventory;        //获取到背包，实现隐藏/显示功能,tonggu
    private int count;
    bool isFull = false;    //判断背包是否满
    public bool isOpen =false;    //判断背包是否打开，更具状态设置物品拖放时的层级设置
    public void Awake()
    {
        //获取到背包的Canvas组件来控制背包开关
        //如果整个Gamobject设为UnActive则当背包关闭时无法设置物品信息（#Null# InventoryItem._instance.SetItemInfo(id)）
        inventory = this.GetComponent<Canvas>();
        inventory.enabled = false;
        _instance = this;
    }

    //模拟拾取
    // Update is called once per frame
    void Update()
    {
        coinText.text = PlayerStatus._instance.coin.ToString();
        if (Input.GetKeyDown(KeyCode.X))
        {
            int randomID = Random.Range(1001, 1004);
            GetItemID(randomID);
        }
    }
    
    /// <summary>
    /// 通过ID号实例化Item
    /// </summary>
    /// <param name="id"> Item的ID号 </param>
    public void GetItemID(int id,int count=1)
    {
        //思路
            //1.先查找所有格子找是否有物体
                //1.1 如果有且ID相等 -> 数量+1，hasFoundItem设为true，跳出循环
            //2.如果没找到相应物体，再次遍历所有格子
                //2.1如果有空格子，实例化新物体
                //2.2否则，背包满了

        bool hasFoundItem = false;
        foreach (GameObject temp in inventorySlotList)
        {
            InventoryItem slotChild = temp.GetComponentInChildren<InventoryItem>();
            if (slotChild != null && slotChild.IdItem == id)
            {
                slotChild.ItemNumPlus(count);
                isFull = false;
                hasFoundItem = true;
                Debug.Log(">>>> Add Num");
                break;
            }
        }
        
        if(hasFoundItem==false)
        {
            foreach (GameObject temp in inventorySlotList)
            {
                InventoryItem slotChild = temp.GetComponentInChildren<InventoryItem>();
                if (slotChild == null && count < inventorySlotList.Count)
                {
                    Instantiate(item, temp.transform);
                    InventoryItem._instance.SetItemInfo(id,count);
                    count++;
                    Debug.Log(">>>> Instantiate");
                    isFull = false;
                    break;
                }
                else    //(slotChild!=null && slotChild.IdItem != id && && count < inventorySlotList.Count)
                {
                    isFull = true;
                    Debug.Log("This slot has item : " + "slotChild.id = " + slotChild.IdItem + "  slot = " + temp.name);
                    Debug.Log("New ID = " + id);
                    Debug.Log(">>> next loop");
                }
            }
        }
        //背包满了
        if (isFull)
        {
            Debug.Log("bag full");
        }
    }

    //显示背包
    public void ShowInventory()
    {
        inventory.enabled = true;
        isOpen = true;
    }

    //隐藏背包
    public void HideInventory()
    {
        inventory.enabled = false;
        isOpen = false;
    }
}
