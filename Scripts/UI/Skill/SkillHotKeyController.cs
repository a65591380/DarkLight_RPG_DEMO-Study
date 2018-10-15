using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillHotKeyController : MonoBehaviour {

    public static SkillHotKeyController _instance;

    public SkillHotKey[] hotkey;

    private void Awake()
    {
        _instance = this;
    }
    //检查技能是否已经放置&&是否已使用
    public bool CheckKey(int id)
    {
        bool isSeted = false;
        for (int i = 0; i < hotkey.Length; i++)
        {

            if (id != 0 && id == hotkey[i].id)
            {
                if(hotkey[i].isUse==true)  //已使用，无法设置技能
                {
                    isSeted = true;
                    return isSeted;
                }
                //已放置，清空原来位置信息
                hotkey[i].info = null;
                hotkey[i].id = 0;
                hotkey[i].type = HotKeyType.None;
                hotkey[i].transform.Find("KeyImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("UIMask");
            }
        }
        return isSeted;
    }
}
