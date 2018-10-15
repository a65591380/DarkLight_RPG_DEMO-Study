using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonGroup : MonoBehaviour {

    //1.游戏数据的保存和场景数据的传递用PlayPrefs
    //2.场景分类
        //2.1 开始场景
        //2.2 角色选择界面
        //2.3 游戏实际场景

    public void NewGame()
    {
        PlayerPrefs.SetInt("SaveData", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        PlayerPrefs.SetInt("SaveData", 1);
        PlayerStatus._instance.coin = PlayerPrefs.GetInt("Coin");
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

	
}
