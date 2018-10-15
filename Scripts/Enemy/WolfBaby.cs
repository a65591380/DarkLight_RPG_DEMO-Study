using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WolfState
{
    Idle,
    Walk,
    Attack,
    Death
}
public class WolfBaby : MonoBehaviour {

    private GameObject target;
    private GameObject temp;
    public WolfSpawn spawn;
    public WolfState state = WolfState.Idle;
    public string anim_death;
    public string anim_idle;
    public string anim_walk;
    public string anim_now;
    public string anim_attack1;
    public string anim_attack2;
    public string anim_attackNow;
    public float attack1_time;
    public float attack2_time;
    public float attack_rate=1;  //攻击速率：次/每秒
    public float crazyattack_rate = 0.3f;      //attack2触发概率
    private float attack_timer=0;
    private Animation anim;
    private CharacterController controller;
    private Renderer body_render;
    //巡逻计时器
    public float time = 1;
    public float timer = 0;

    public int attack = 10;
    public float speed=1;
    public int hp;
    public int hp_max;
    public int exp=20;
    public float miss_rate = 0.2f;

    public float red_time = 1f;
    private Color normal_color;

    private AudioSource wolf_audio;
    public AudioClip miss_clip;

    public float min_disitance = 2;
    public float max_distance = 5;
    private HPFollowTarget hpfollow;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag(Tags.player);
        hpfollow = this.GetComponent<HPFollowTarget>();
        temp = target;
        wolf_audio = this.GetComponent<AudioSource>();
        body_render = this.GetComponentInChildren<Renderer>();
        anim = this.GetComponent<Animation>();
        anim_now = anim_idle;
        controller = this.GetComponent<CharacterController>();
        normal_color = body_render.material.color;
        hp_max = hp;
    }
    private void Update()
    {
        if(state==WolfState.Death)
        {
            anim.CrossFade(anim_death);
        }
        else if(state==WolfState.Attack)
        {
            //攻击
            AutoAttack();
        }
        else  //巡逻
        {
            anim.CrossFade(anim_now);
            if(anim_now==anim_walk)
            {
                controller.SimpleMove(transform.forward * speed);
            }
            timer += Time.deltaTime;
            if(timer>=time)
            {
                timer = 0;
                RandomState();
            }
            
        }
    }

    //随机切换idle和walk状态
    public void RandomState()
    {
        int value = Random.Range(0, 2);
        if(value==0)
        {
            anim_now = anim_idle;
        }
        else
        {
            if(anim_now!=anim_walk)
            {
                transform.Rotate(transform.up * Random.Range(0, 360));
            }
            anim_now = anim_walk;
        }
    }

    //受伤
    public void TakeDamage(int attack)
    {
        if(state!=WolfState.Death)
        {
            target = temp;
            state = WolfState.Attack;
            float value = Random.Range(0f, 1f);
            if (value < miss_rate)
            {
                hpfollow.MissTextControl();
                //miss
                wolf_audio.clip = miss_clip;
                wolf_audio.Play();
            }
            else
            {
                this.hp -= attack;
                hpfollow.DamageTextControl(attack);
                hpfollow.SliderControl();
                StartCoroutine(ShowBodyRed());
                if (hp <= 0)  //死亡
                {
                    state = WolfState.Death;
                    spawn.MinusNum();
                    BarNpc._instance.killCount++;
                    PlayerStatus._instance.GetExp(exp);
                    Destroy(this.gameObject, 1);
                    
                }
            }
        }
    }

    //身体变红
    IEnumerator ShowBodyRed()
    {
        body_render.material.color = Color.red;
        yield return new WaitForSeconds(1);
        body_render.material.color = normal_color;
    }
    //随机切换攻击状态
    public void RandomAttack()
    {
        float value = Random.Range(0, 1f);
        if(value<crazyattack_rate)
        {
            anim_attackNow = anim_attack2;
        }
        else
        {
            anim_attackNow = anim_attack1;
        }
    }

    //自动攻击
    public void AutoAttack()
    {
        if (target.GetComponent<PlayerAttack>().state == PlayerState.Death)
        {
            target = null;
            state = WolfState.Idle;
            return;
        }
        if (target!=null)
        {
           
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if(distance>=max_distance)
            {
                target = null;
                state = WolfState.Idle;
            }
            else if(distance<=min_disitance)
            {
                //攻击
                attack_timer += Time.deltaTime;
                anim.CrossFade(anim_attackNow);
                if(anim_attackNow==anim_attack1)
                {
                    if(attack_timer>=attack1_time)
                    {
                        //伤害
                        
                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
                        anim_attackNow = anim_idle;
                        
                    }
                }
                else if (anim_attackNow == anim_attack2)
                {
                    if (attack_timer >= attack2_time)
                    {
                        //伤害
                        target.GetComponent<PlayerAttack>().TakeDamage(attack*2);
                        anim_attackNow = anim_idle;
                    }
                }
                if(attack_timer>=(1f/attack_rate))
                {
                    attack_timer = 0;
                    RandomAttack();
                }

            }
            else
            {
                transform.LookAt(target.transform);
                controller.SimpleMove(transform.forward * speed);
                anim.CrossFade(anim_walk);
            }
        }
    }

    private void OnMouseEnter()
    {
        CursorManager._instance.SetAttack();
    }
    private void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }
}
