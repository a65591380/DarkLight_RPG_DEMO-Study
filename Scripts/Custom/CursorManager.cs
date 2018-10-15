using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    public static CursorManager _instance;
    
    public Texture2D cursor_attack;
    public Texture2D cursor_lock_target;
    public Texture2D cursor_normal;
    public Texture2D cursor_npc_talk;
    public Texture2D cursor_pick;

    private Vector2 hotspot = Vector2.zero;
    private CursorMode mode = CursorMode.Auto;

    // Use this for initialization
    void Start () {
        _instance = this;
	}
	
    //设置鼠标指针图片
	public void SetNormal()
    {
        Cursor.SetCursor(cursor_normal, hotspot, mode);
    }

    public void SetNpcTalk()
    {
        Cursor.SetCursor(cursor_npc_talk, hotspot, mode);
    }

    public void SetAttack()
    {
        Cursor.SetCursor(cursor_attack, hotspot, mode);
    }
    public void SetLock()
    {
        Cursor.SetCursor(cursor_lock_target, hotspot, mode);
    }
}
