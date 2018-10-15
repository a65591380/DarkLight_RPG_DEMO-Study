using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayer : MonoBehaviour {
    
    public GameObject[] PlayerPrefabs;
    public Text nameText;
    private GameObject[] PlayerGameObjects;
    private int index = 0;
    private int length;

	// Use this for initialization
	void Start () {
        length = PlayerPrefabs.Length;
        PlayerGameObjects = new GameObject[length];
        for(int i=0;i<length;i++)
        {
            PlayerGameObjects[i] = GameObject.Instantiate(PlayerPrefabs[i], transform.position, transform.rotation);
        }
        ShowPlayer();
	}
	
    //显示当前选择的角色
	void ShowPlayer()
    {
        PlayerGameObjects[index].SetActive(true);
        for(int i=0;i<length; i++)
        {
            if(i !=index)
            {
                PlayerGameObjects[i].SetActive(false);
            }
        }
    }
    
    //点击下一按钮切换角色
    public void OnNextButtonClick()
    {
        index++;
        index %= length;
        ShowPlayer();
    }

    //点击上一按钮切换角色
    public void OnPrevButtonClick()
    {
        index--;
        if(index<0)
        {
            index = length - 1;
        }
        ShowPlayer();
    }

    //点击Ok按钮后的操作
    public void OnOKButtonClick()
    {
        if(nameText.text!="")
        {
            PlayerPrefs.SetInt("SelectedPlayer", index);    //存储选择的角色
            PlayerPrefs.SetString("Name", nameText.text);   //存储角色名
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            //加载下一场景
        }
        else
        {
            Debug.Log("请输入名字");
        }
    }
}
