using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBattle : MonoBehaviour {

    public static PlayerBattle _instance;
    private AudioSource weapon_audio;
    private CharacterController controller;
    private Animation anim;
    public GameObject attack_effect;
    public bool isInBattle = false;
    public bool isFirstAttack = true;
    public float attack_distance = 5;
    public Transform attack_target;
    

    public string attackNow;
    public string currentAnim;
    public float critical_rate = 0.2f;
    public float attack_rate = 2;
    public float timer = 0;

    private void Awake()
    {
        _instance = this;
        weapon_audio = this.GetComponent<AudioSource>();
        controller = this.GetComponent<CharacterController>();
        anim = this.GetComponent<Animation>();
    }
    void Update () {
        if(PlayerStatus._instance.State!=PlayerState.Death)
        {
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //把点击位置转换成射线
                RaycastHit hitInfo;
                bool isCollider = Physics.Raycast(ray, out hitInfo);
                if (isCollider && hitInfo.collider.tag == Tags.Enemy)
                {
                    isInBattle = true;
                    attack_target = hitInfo.transform;

                    float distance = Vector3.Distance(transform.position, attack_target.position);
                    if (distance <= attack_distance)
                    {
                        PlayerStatus._instance.State = PlayerState.Attack;
                    }
                    else
                    {
                        PlayerStatus._instance.State = PlayerState.Move;
                    }

                }
                else
                {
                    PlayerStatus._instance.State = PlayerState.Move;
                    attack_target = null;
                    isInBattle = false;
                    isFirstAttack = true;
                }
            }
        }

        if(attack_target!=null)
        {
            attack_target.position = new Vector3(attack_target.position.x, transform.position.y, attack_target.position.z);
            transform.LookAt(attack_target.position);
        }
        BattleTrack();
    }

    //角色攻击动画
    public void PlayerAttack()
    {
        //处理第一次移动到敌人边上时仍播放Run动画的问题
        if (currentAnim == "Idle" || currentAnim == "Run")
        {
            anim.CrossFade("Attack1");
            attackNow = "Attack1";
            currentAnim = "Attack1";
        }
        timer += Time.deltaTime;
        if (timer >= attack_rate)
        {
            timer = 0;
            PlayerRandomAttack();
            currentAnim = attackNow;
            anim.CrossFade(attackNow);
            anim.CrossFadeQueued("Idle");
        }
        
    }

    //随机攻击动画
    public void PlayerRandomAttack()
    {
        float value = Random.Range(0, 1f);
        if (value <= critical_rate)
        {
            attackNow = "AttackCritical";
        }
        else if (value > 0.6f)
        {
            attackNow = "Attack1";
        }
        else
        {
            attackNow = "Attack2";
        }
    }

    /// <summary>
    /// 根据攻击的动画类型计算伤害
    /// </summary>
    /// <returns>返回伤害值</returns>
    public int AttackDamage()
    {
        
        int damage;
        if (attackNow == "Attack1") 
        {
            damage = PlayerStatus._instance.sum_attack;
        }
        else if (attackNow == "Attack2")
        {
            damage = (int)(PlayerStatus._instance.sum_attack*1.1f);
        }
        else if(attackNow== "AttackCritical")
        {
            damage = (int)(PlayerStatus._instance.sum_attack * 1.5f);
        }
        else
        {
            damage = 0;
        }
        attack_target.GetComponent<WolfBaby>().TakeDamage(damage);
        weapon_audio.Play();
        return damage;
    }
    //战斗与追踪
    public void BattleTrack()
    {
        if (isInBattle && attack_target != null)
        {
            float distance = Vector3.Distance(attack_target.position, transform.position);
            if(isFirstAttack)   //初次进入战斗
            {
                if (distance > attack_distance)
                {
                    anim.CrossFade("Run");
                    attack_target.position = new Vector3(attack_target.position.x, transform.position.y, attack_target.position.z);
                    transform.LookAt(attack_target.position);
                    controller.SimpleMove(transform.forward * 5);       //往前移动
                }
                else
                {
                    attack_target.position = new Vector3(attack_target.position.x, transform.position.y, attack_target.position.z);
                    transform.LookAt(attack_target.position);
                    PlayerRandomAttack();
                    anim.CrossFade(attackNow);
                    AttackDamage();
                    Instantiate(attack_effect, attack_target.position, Quaternion.identity);
                    isFirstAttack = false;
                }
            }
            else      //战斗中跟随与攻击
            {
                if (distance > attack_distance)
                {
                    anim.CrossFade("Run");
                    attack_target.position = new Vector3(attack_target.position.x, transform.position.y, attack_target.position.z);
                    transform.LookAt(attack_target.position);
                    controller.SimpleMove(transform.forward * 3);       //往前移动
                }
                else
                {
                    attack_target.position = new Vector3(attack_target.position.x, transform.position.y, attack_target.position.z);
                    transform.LookAt(attack_target.position);
                    timer += Time.deltaTime;
                    if (timer >= attack_rate)
                    {
                        timer = 0;
                        PlayerRandomAttack();
                        anim.CrossFade(attackNow);
                        AttackDamage();
                        Instantiate(attack_effect, attack_target.position, Quaternion.identity);
                        
                        anim.CrossFadeQueued("Idle");
                    }
                }
                
            }
        }
    }

    public void PlayIdle()
    {
        anim.Play("Idle");
    }
}
