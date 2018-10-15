using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDamage : MonoBehaviour {

    private List<WolfBaby> wolfList = new List<WolfBaby>();


    public void OnTriggerEnter(Collider other)
    {
        if(other.tag==Tags.Enemy)
        {
            //对单个敌人只触发一次伤害
            WolfBaby baby = other.GetComponent<WolfBaby>();
            int index = wolfList.IndexOf(baby);
            if(index==-1)
            {
                baby.TakeDamage(PlayerStatus._instance.sum_attack * 4);
                wolfList.Add(baby);
            }
        }
    }
}
