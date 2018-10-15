using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PressAnyKeyAnim : MonoBehaviour {

    private PlayableDirector pressAnyKey;
    private float timer = 0f;
    private GameObject button;
    private bool isAnyKeyDown = false;

    public float startTime = 4f;
   

    private void Awake()
    {
        pressAnyKey = GetComponent<PlayableDirector>();
        button = transform.parent.Find("StartButtonGroup").gameObject;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
        timer += Time.deltaTime;
        Debug.Log(timer);

        //4秒后显示pressKey提示
        if(timer>startTime)
        {
            pressAnyKey.enabled = true;
            
            if (isAnyKeyDown == false && Input.anyKey)  //按下后显示button
            {
                ShowButton();
            }
        }
		
	}

    //显示button
    void ShowButton()
    {
        button.SetActive(true);
        this.gameObject.SetActive(false);
        isAnyKeyDown = true;
        
    }
}
