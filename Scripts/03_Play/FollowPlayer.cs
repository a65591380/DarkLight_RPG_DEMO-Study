using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public float distance = 0f;
    public float scrollSpeed = 5f;
    public float rotateSpeed = 2f;

    private bool isRotate = false;
    private Transform player;
    private Vector3 offsetPosition;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        transform.LookAt(player.position);
        offsetPosition = transform.position -  player.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = offsetPosition + player.position;
        
        RotateView();
        ScrollView();
    }

    //控制距离远近
    //Edit -> Project Setting -> Axes / Mouse ScrollWheel
    void ScrollView()
    {
        distance = offsetPosition.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel") * -scrollSpeed;
        distance=Mathf.Clamp(distance,2, 18);
        offsetPosition = distance * offsetPosition.normalized;      //距离长度 * 单位向量
    }

    //视野旋转
    void RotateView()
    {
        //鼠标右键摁下旋转
        if(Input.GetMouseButtonDown(1))
        {
            isRotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotate = false;
        }

        if(isRotate)
        {
            //左右旋转
            transform.RotateAround(player.position, player.up, rotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            //上下旋转
            transform.RotateAround(player.position, -transform.right, rotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;
            if(x<10 || x>80)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - player.position;
    }
}
