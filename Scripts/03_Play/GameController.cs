using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject magicianPrefab;
    public GameObject swordmanPrefab;
    private void Awake()
    {
        int index= PlayerPrefs.GetInt("SelectedPlayer");       //角色
        string name = PlayerPrefs.GetString("Name");        //名字

        if(index==0)
        {
            Instantiate(magicianPrefab);
        }
        else if(index==1)
        {
            Instantiate(swordmanPrefab);
        }
        PlayerStatus._instance.player_name = name;
    }

    private void Update()
    {
        
    }
}
