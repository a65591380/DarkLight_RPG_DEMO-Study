using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
//脚本放在需要跟随的物体上
public class HPFollowTarget_Player : MonoBehaviour {
    //声明你需要用到的变量  
    public GameObject HpPrefab;   //panel,其下还有slider和text等UI
    private GameObject HpPrefab_new;
    private Transform HP_Parent;
    private Vector3 PlayerPosition;
    private PlayerAttack state;
    private Text text;
    private PlayableDirector director;
    void Start()
    {
        state = this.GetComponent<PlayerAttack>();
        //获取放HP血条的父物体（ Canvas ）
        HP_Parent = GameObject.FindWithTag("HP").transform;
        //把游戏物体的世界坐标转换为屏幕坐标  
        PlayerPosition = Camera.main.WorldToScreenPoint(transform.position);
        //创建一个Clone血条图片  
        HpPrefab_new = Instantiate(HpPrefab, PlayerPosition, Quaternion.identity);
        //设置血条的父物体  
        HpPrefab_new.transform.SetParent(HP_Parent);
        text = HpPrefab_new.GetComponentInChildren<Text>();
        director =HpPrefab_new.GetComponentInChildren<PlayableDirector>();
    }


    void Update()
    {
  
        //每帧都去执行使血条跟随物体 
        if(HpPrefab_new != null)
        PHFollowEnemy();
    }
    //血条放置到Canvas另一个Plane中 并跟随物体移动  
    void PHFollowEnemy()
    {
        //把物体坐标转换为屏幕坐标，修改偏移量  
        PlayerPosition = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 50, 0);
        HpPrefab_new.transform.position = PlayerPosition;
    }

    //播放miss文本动画
    public void MissTextControl()
    {
        text.color = Color.gray;
        text.text = "Miss";
        director.Play();
    }
    //播放伤害文本动画
    public void DamageTextControl(int damage)
    {
        text.color = Color.red;
        text.text = "-"+damage;
        director.Play();
    }
} 