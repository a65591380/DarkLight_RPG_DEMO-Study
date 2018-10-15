using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomNpc : MonoBehaviour {

    public void OnMouseEnter()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            CursorManager._instance.SetNpcTalk();
        }
        
    }

    private void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            AudioSource clickAudio = GetComponent<AudioSource>();
            clickAudio.Play();
        }
            
    }
}
