using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerCareer
{
    Swordman,
    Magician,
}

public class PlayerStatus : MonoBehaviour {
   

    public static PlayerStatus _instance;
    public PlayerCareer career;

    public string player_name;
    public int grade = 1;
    public int hp_max = 100;
    public int mp_max = 100;
    public int exp_levelup=100;     //升级所需经验
    public float hp_current = 100;
    public float mp_current = 100;
    public float exp_current = 0;

    public int coin = 200;
    public int attack = 20;
    public int attack_plus = 0;
    public int attack_equip = 0;
    public int def = 20;
    public int def_plus = 0;
    public int def_equip = 0;
    public int speed = 20;
    public int speed_plus = 0;
    public int speed_equip = 0;

    public int sum_attack;
    public int sum_def;
    public int sum_speed;
    public int point_remain;

    private Animation anim;

    public GameObject levelup_efx;
    private void Awake()
    {
       _instance = this;
        anim = this.GetComponent<Animation>();
    }

    private void Update()
    {
        sum_attack = attack + attack_plus + attack_equip;
        sum_def = def + def_plus + def_equip;
        sum_speed = speed + speed_plus + speed_equip;
    }
    /// <summary>
    /// 显示任务
    /// </summary>
    /// <param name="coinBouns">金币奖励</param>
    /// <returns>void</returns>
    /// <remarks>显示在哪？</remarks>
    public void GainCoin(int coinBouns)
    {
        coin += coinBouns;
    }

    /// <summary>
    /// 获得能力点
    /// </summary>
    /// <param name="point">加点</param>
    /// <returns></returns>
    public bool GetPoint(int point=1)
    {
        if(point_remain>=point)
        {
            point_remain -= point;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 治疗恢复
    /// </summary>
    /// <param name="hp">血</param>
    /// <param name="mp">蓝</param>
    public void GetDrug(int hp,int mp)
    {
        hp_current += hp;
        mp_current += mp;
        if(hp_current>=hp_max)
        {
            hp_current = hp_max;
        }
        if(mp_current>=mp_max)
        {
            mp_current = mp_max;
        }
    }

    /// <summary>
    /// 获得经验 & 升级
    /// </summary>
    /// <param name="exp">经验</param>
    public void GetExp(int exp)
    {
        exp_current += exp;
        exp_levelup = 100 + (grade-1) * 50;
        while(exp_current>=exp_levelup)
        {
            Instantiate(levelup_efx, transform.position, Quaternion.identity);
            grade++;
            point_remain += 5;
            exp_current -= exp_levelup;
            exp_levelup = 100 + (grade - 1) * 50;
        }
    }
   
    public bool UseMp(int mp)
    {
        if(mp_current>=mp)
        {
            return true;
        }
        else
        {
            Debug.Log("蓝不够");
            return false;
        }
    }
}
