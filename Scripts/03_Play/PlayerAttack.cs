using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PlayerState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}
public enum AttackState
{//攻击时候的状态
    Moving,
    Idle,
    Attack
}
public class PlayerAttack : MonoBehaviour {

    public PlayerState state=PlayerState.ControlWalk;
    public AttackState attack_state = AttackState.Idle;

    public float miss_rate = 0.3f;  //躲避几率

    public string anim_attackNormal;
    public string anim_idle;
    public string anim_now;
    public float time_attack;
    public float attack_rate=0.5f;
    private float timer = 0;
    public float distance_attack=5;

    private HPFollowTarget_Player hpfollow;
    private Animation anim;
    public Transform target;
    private PlayerMove move;
    public GameObject effect;
    private bool showEffect=false;
    private AudioSource player_audio;
    private Renderer body_render;
    private Color normal_color;
    public AudioClip miss_clip;
    public AudioClip attack_clip;
    public GameObject[] effectArray;
    private Dictionary<string, GameObject> efxDict = new Dictionary<string, GameObject>();

    private bool isLockTarget = false;
    private SkillInfo info;
    public GameObject beHitEfx_prefab;
    public AudioClip behit_clip;
    public GameObject releasebar_prefab;
    public GameObject magicRangeBulletEFX_prefab;
    public float skillRelease_time;  //技能释放时间
    private void Awake()
    {
        hpfollow = this.GetComponent<HPFollowTarget_Player>();
        anim = this.GetComponent<Animation>();
        move = this.GetComponent<PlayerMove>();
        player_audio = this.GetComponent<AudioSource>();
        body_render = this.GetComponentInChildren<Renderer>();
        normal_color = body_render.material.color;

        foreach(GameObject efx in effectArray)
        {
            efxDict.Add(efx.name, efx);
        }
    }
    // Update is called once per frame
    void Update () {

		if(Input.GetMouseButtonDown(0)&&state!=PlayerState.Death&& state != PlayerState.SkillAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.Enemy) //攻击
            {
                target = hitInfo.collider.transform;
                state = PlayerState.NormalAttack;
                transform.LookAt(target.position);
            }
            
            else //移动
            {
                state = PlayerState.ControlWalk;
                target = null;
            }
        }
      
        //攻击逻辑处理
        if(state==PlayerState.NormalAttack&&target!=null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if(distance<=distance_attack)
            {
                transform.LookAt(target);
                //攻击
                attack_state = AttackState.Attack;
                timer += Time.deltaTime;
                anim.CrossFade(anim_now);
                if(timer>=time_attack)
                {
                    anim_now = anim_idle;
                    if(showEffect==false)
                    {
                        showEffect = true;
                        //播放特效
                        Instantiate(effect, target.position, Quaternion.identity);
                        //声音
                        player_audio.clip = attack_clip;
                        player_audio.Play();
                        //造成伤害
                        target.GetComponent<WolfBaby>().TakeDamage(PlayerStatus._instance.sum_attack);
                    }
                }
                if(timer>=(1f/attack_rate))
                {
                    anim_now = anim_attackNormal;
                    timer = 0;
                    showEffect = false;
                }
            }
            else
            {
                //走向敌人
                attack_state = AttackState.Moving;
                move.SimpleMove(target.position);
            }
        }
	}


    //主角受伤
    public void TakeDamage(int attack)
    {
        if(state!=PlayerState.Death)
        {
            float value = Random.Range(0, 1f);
            if (value <= miss_rate)
            {
                //miss
                hpfollow.MissTextControl();
                player_audio.clip = miss_clip;
                player_audio.Play();
            }
            else
            {
                int damage = attack - PlayerStatus._instance.sum_def;
                Instantiate(beHitEfx_prefab, transform.position, Quaternion.identity);
                player_audio.clip = behit_clip;
                player_audio.Play();
                if (damage >= 1)
                {
                    PlayerStatus._instance.hp_current -= damage;
                    hpfollow.DamageTextControl(damage);
                    StartCoroutine(ShowBodyRed());
                }
                else
                {
                    damage = 1;
                    PlayerStatus._instance.hp_current -= damage;
                    hpfollow.DamageTextControl(damage);
                    StartCoroutine(ShowBodyRed());
                    
                }
                if (PlayerStatus._instance.hp_current <= 0)
                {
                    state = PlayerState.Death;
                    anim.CrossFade("Death");
                }
            }
        }
    }

    //身体变红
    IEnumerator ShowBodyRed()
    {
        body_render.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        body_render.material.color = normal_color;
    }

    //使用技能
    public void UseSkill(SkillInfo info)
    {

        switch (info.applyType)
        {
            case ApplyType.Passive:
                StartCoroutine(OnPassiveSkillUse(info));
                break;
            case ApplyType.Buff:
                StartCoroutine(OnBuffSkillUse(info));
                break;
            case ApplyType.SingleTarget:
                OnSingleTargetSkillUse(info);
                break;
            case ApplyType.MultiTarget:
                StartCoroutine(OnMultiSkillUse(info));
                break;

        }
        //用技能

    }


    //处理增益技能（回血回蓝）
    public IEnumerator OnPassiveSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        anim.CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;

        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        Instantiate(prefab, transform.position, Quaternion.identity);
        //根据技能类型判断加血加蓝
        int hp = 0, mp = 0;
        if (info.applyProperty == ApplyProperty.HP)
        {
            hp = info.applyValue;
        }
        else if (info.applyProperty == ApplyProperty.MP)
        {
            mp = info.applyValue;
        }
        PlayerStatus._instance.mp_current -= info.mp;
        PlayerStatus._instance.GetDrug(hp, mp);
    }

    //处理buff技能（增强能力）
    public IEnumerator OnBuffSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        anim.CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;
        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        Instantiate(prefab, transform.position, Quaternion.identity);
        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                PlayerStatus._instance.attack*= 2;
                break;
            case ApplyProperty.AttackSpeed:
                attack_rate *= 2;
                break;
            case ApplyProperty.Def:
                PlayerStatus._instance.sum_def *= 2;
                break;
            case ApplyProperty.Speed:
                move.speed *= 2;
                break;
        }
        PlayerStatus._instance.mp_current -= info.mp;
        yield return new WaitForSeconds(info.applyTime);
        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                PlayerStatus._instance.attack /= 2;
                break;
            case ApplyProperty.AttackSpeed:
                attack_rate /= 2;
                break;
            case ApplyProperty.Def:
                PlayerStatus._instance.sum_def /= 2;
                break;
            case ApplyProperty.Speed:
                move.speed /= 2;
                break;
        }
    }

    //处理单体技能
    public void OnSingleTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        this.info = info;
        anim.CrossFade(info.aniname);
        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        Instantiate(prefab,transform.position, Quaternion.identity);
        PlayerStatus._instance.mp_current -= info.mp;
        state = PlayerState.NormalAttack;

    }

    //范围伤害技能
    IEnumerator OnMultiSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        this.info = info;
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        Instantiate(releasebar_prefab, GameObject.FindGameObjectWithTag("HUD").transform);
        GameObject bullet= Instantiate(magicRangeBulletEFX_prefab, target.position, Quaternion.identity);
        yield return new WaitForSeconds(skillRelease_time);
        Destroy(bullet);
        anim.CrossFade(info.aniname);
        //实例化特效
        
        Instantiate(prefab,target.position, Quaternion.identity);
        PlayerStatus._instance.mp_current -= info.mp;
        yield return new WaitForSeconds(0.833f);
        
        state = PlayerState.NormalAttack;
    }
}
