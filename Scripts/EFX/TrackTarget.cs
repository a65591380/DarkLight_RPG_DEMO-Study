using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//飞向目标
public class TrackTarget : MonoBehaviour {

    public float speed=1;
    public float triggerRange = 0.1f;  //魔法弹和目标间的距离，小于此距离则触发伤害
    private PlayerAttack attack;
    public GameObject explodeEfx_prefab;
    private AudioSource efx_audio;
    public bool hasExplode = false;
    private void Awake()
    {
        attack = GameObject.FindWithTag(Tags.player).GetComponent<PlayerAttack>();
        efx_audio = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if(!hasExplode)
        StartCoroutine(Track());
	}

    IEnumerator Track()
    {
        if (attack.target != null&&attack.target.GetComponent<WolfBaby>().state!=WolfState.Death)
        {
            transform.LookAt(attack.target.position);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            float distance = Vector3.Distance(transform.position, attack.target.position);
            if (distance < triggerRange)
            {
                explodeEfx_prefab.SetActive(true);
                efx_audio.Play();
                attack.target.GetComponent<WolfBaby>().TakeDamage(PlayerStatus._instance.sum_attack * 3);
                hasExplode = true;
                yield return new WaitForSeconds(0.5f);
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
