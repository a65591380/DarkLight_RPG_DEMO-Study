using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
//脚本放在需要跟随的物体上
public class HPFollowTarget : MonoBehaviour {
    //声明你需要用到的变量  
    public GameObject HpPrefab;   //panel,其下还有slider和text等UI
    private GameObject HpPrefab_new;
    private Transform HP_Parent;
    private Vector3 EnemySceenPosition;

    private WolfBaby wolfBaby;
    private Slider slider;
    private Text text;
    private PlayableDirector director;
    void Start()
    {
        wolfBaby = this.GetComponent<WolfBaby>();
        //获取放HP血条的父物体（ Canvas ）
        HP_Parent = GameObject.FindWithTag("HP").transform;
        //把游戏物体的世界坐标转换为屏幕坐标  
        EnemySceenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //创建一个Clone血条图片  
        HpPrefab_new = Instantiate(HpPrefab, EnemySceenPosition, Quaternion.identity);
        //设置血条的父物体  
        HpPrefab_new.transform.SetParent(HP_Parent);
        slider = HpPrefab_new.GetComponentInChildren<Slider>();
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
        EnemySceenPosition = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 50, 0);
        HpPrefab_new.transform.position = EnemySceenPosition;
        if (wolfBaby.state == WolfState.Death)
        {
            Destroy(this.HpPrefab_new);
        }
    }
    public void MissTextControl()
    {
        text.color = Color.gray;
        text.text = "Miss";
        director.Play();
    }

    public void DamageTextControl(int damage)
    {
        text.color = Color.red;
        text.text = "-" + damage;
        director.Play();
    }
    public void SliderControl()
    {
        slider.value = (float)wolfBaby.hp / (float)wolfBaby.hp_max;
    }
} 