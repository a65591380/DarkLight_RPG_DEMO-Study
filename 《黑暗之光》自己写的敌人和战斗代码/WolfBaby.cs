using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;
public enum WolfState
{
    Idle,
    Walk,
    Attack,
    Death,
    TakeDamage
}
public class WolfBaby : MonoBehaviour {

    public int exp = 20;
    public WolfSpawn spawn;
    public Transform attack_target;
    public Transform temp;
    public WolfState state = WolfState.Idle;

    //巡逻計時器
    public float time = 1f;
    public float timer = 0f;
    public bool isFirst = true;
    public int hp = 100;
    public int attack = 10;
    public float attack_distanceMax = 5;
    public float attack_distanceMin = 2;

    public float miss_rate = 0.2f;      //躲避几率

    
    public string anim_Attack1;     //普通攻击
    public string anim_Attack2;     //重击
    private string anim_attackNow;

    //攻击计时器
    public float timer_attack = 0;
    public float attack_rate = 2;

    //动画
    public string anim_Idle;
    public string anim_Death;
    public string anim_Walk;
    public string anim_current;
    public AudioClip miss_clip;
    private AudioSource sound;
    private Animation anim;
    private CharacterController charControl;
    private Renderer render;
    private TextMesh miss_text;
    private TextMesh damage_text;
    private PlayableDirector playable;
    private Color normal_color;

    private Vector3 pos;
    private void Start()
    {
        attack_target = GameObject.FindWithTag(Tags.player).transform;
        temp = attack_target;
        anim = this.GetComponent<Animation>();
        anim_current = anim_Idle;
        charControl = this.GetComponent<CharacterController>();
        render = this.GetComponentInChildren<Renderer>();
        normal_color = render.material.color;
        miss_text = transform.Find("MissText").GetComponent<TextMesh>();
        damage_text = transform.Find("DamageText").GetComponent<TextMesh>();
        sound = this.GetComponent<AudioSource>();
        playable = this.GetComponentInChildren<PlayableDirector>();

        pos = transform.position;
    }
    // Update is called once per frame
    void Update () {
        if (state == WolfState.Death)
        {
            anim.CrossFade(anim_Death);
        }
        else if (state == WolfState.Idle)  //巡逻
        {
            anim.CrossFade(anim_current);

            if (anim_current == anim_Walk)
            {
                charControl.SimpleMove(transform.forward * 1);
            }
            timer += Time.deltaTime;
            if (timer >= time)
            {
                timer = 0;
                RandomState();
            }
        }
        else if (state == WolfState.Attack)
        {
            if(PlayerStatus._instance.State != PlayerState.Death)
            {
                Attack();
            }
            else
            {
                attack_target = null;
                state = WolfState.Idle;
            }
        }
    }

    //随机切换状态
    public void RandomState()
    {
        int value = Random.Range(0, 2);
        if(value==0)
        {
            anim_current = anim_Idle;
        }
        else
        {
            //当Idle切换到Walk时行走
            if(anim_current!=anim_Walk)
            {
                transform.Rotate(transform.up * Random.Range(0, 360));
            }
            anim_current = anim_Walk;
        }
    }

    //受伤
    public void TakeDamage(int attack)
    {
        attack_target = temp;
        state = WolfState.Attack;
        if(state==WolfState.Death)
        {
            return;
        }
        float value = Random.Range(0f, 1f);
        if(value<miss_rate)
        {
            sound.clip = miss_clip;
            sound.Play();
            StartCoroutine((ShowMissText()));
        }
        else
        {
            this.hp -= attack;
            StartCoroutine(ShowDamageText(attack));
            StartCoroutine(ShowBodyRed());
            if (hp <= 0)
            {
                state = WolfState.Death;
                spawn.MinusNum();
                PlayerStatus._instance.GetExp(exp);     //获得经验
                if(BarNpc._instance.isInTask)
                {
                    BarNpc._instance.killCount++;
                }
               
                PlayerBattle._instance.isInBattle = false;
                Destroy(this.gameObject, 1);
            }
        }
    }

    //随机切换攻击状态
    public void RandomAttackState()
    {
        float value = Random.Range(0, 1f);
        if(value<=0.1)  //休息
        {
            anim_attackNow = anim_Idle;
        }
        else if(value>=0.7f)    //重击
        {
            anim_attackNow = anim_Attack2;
        }
        else  //普通攻击
        {
            anim_attackNow = anim_Attack1;
        }
    }

    //追踪与攻击
    public void Attack()
    {
        if(attack_target!=null)
        {
            float distance = Vector3.Distance(transform.position, attack_target.position);
            if(distance>attack_distanceMax)
            {
                attack_target = null;
            }
            else if(distance<=attack_distanceMin)
            {
                
                if(isFirst)
                {
                    RandomAttackState();
                    if (anim_attackNow == anim_Attack1)
                    {
                        PlayerStatus._instance.TakeDamage(attack);

                    }
                    else if (anim_attackNow == anim_Attack2)
                    {
                        PlayerStatus._instance.TakeDamage((int)(attack * 1.2f));
                    }
                    anim.CrossFade(anim_attackNow);
                  
                    isFirst = false;
                }
                
                //攻击
                state = WolfState.Attack;
                timer_attack += Time.deltaTime;
                if (timer_attack >= attack_rate)
                {
                    
                    timer_attack = 0;
                    RandomAttackState();
                    if (anim_attackNow == anim_Attack1)
                    {
                        PlayerStatus._instance.TakeDamage(attack);

                    }
                    else if (anim_attackNow == anim_Attack2)
                    {
                        PlayerStatus._instance.TakeDamage((int)(attack * 1.2f));
                    }
                    anim.CrossFade(anim_attackNow);
                    anim.CrossFadeQueued(anim_Idle);
                }
            }
            else
            {
                //追踪
                transform.LookAt(attack_target);
                charControl.SimpleMove(transform.forward * 1.5f);
                anim.CrossFade(anim_Walk);
                isFirst = true;
            }

        }
        else
        {
            anim_current = anim_Idle;
            state = WolfState.Idle;
        }
    }
    //显示Miss特效
    IEnumerator ShowMissText()
    {
        miss_text.text ="Miss";
        Color color1 = Color.black;
        color1.a = 0;
        Color color2 = Color.black;
        color2.a = 255;
        miss_text.color = Color.Lerp(color1, color2, Time.deltaTime*1 );
        playable.Play();
        yield return new WaitForSeconds(2);
        miss_text.color = color1;
    }

    //显示受伤数字
    IEnumerator ShowDamageText(int damage)
    {
        damage_text.text = "-"+damage;
        Color color1 = Color.red;
        color1.a = 0;
        Color color2 = Color.red;
        color2.a = 255;
        miss_text.color = Color.Lerp(color1, color2, Time.deltaTime * 1);
        playable.Play();
        yield return new WaitForSeconds(2);
        miss_text.color = color1;
    }

    //身体变红
    IEnumerator ShowBodyRed()
    {
        render.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        render.material.color = normal_color;
    }

    public void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            CursorManager._instance.SetAttack();
        }

    }

    private void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }
}
