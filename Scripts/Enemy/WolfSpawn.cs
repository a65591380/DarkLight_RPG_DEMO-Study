using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawn : MonoBehaviour {

    public GameObject wolfBabyPrefab;
    public int maxNum = 5;
	private int currentNum;
    public float timer;
    public float spawn_rate = 3f;       //生成速度

    private void Update()
    {
        if(currentNum<maxNum)
        {
            timer += Time.deltaTime;
            if(timer>spawn_rate)
            {
                Vector3 pos = transform.position;
                pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);
                GameObject go= Instantiate(wolfBabyPrefab,pos,Quaternion.identity);
                go.GetComponent<WolfBaby>().spawn=this;
                timer = 0;
                currentNum++; 
            }
        }
    }

    public void MinusNum()
    {
        this.currentNum--;
    }
}
