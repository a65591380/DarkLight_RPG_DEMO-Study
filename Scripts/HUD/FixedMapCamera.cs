using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMapCamera : MonoBehaviour {

    private Transform player;
    private Vector3 offset;
    private Camera map_camera;
    private void Awake()
    {
        map_camera = this.GetComponent<Camera>();
        
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        offset = transform.position - player.position;
        map_camera.fieldOfView = 70;
    }

    private void Update()
    {
        transform.position = offset + player.position;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    //放大小地图
    public void OnZoomIn()
    {
        Debug.Log(map_camera.fieldOfView);
        
        map_camera.fieldOfView -= 10;
        map_camera.fieldOfView = Mathf.Clamp(map_camera.fieldOfView, 40, 100);
    }
    //缩小小地图
    public void OnZoomOut()
    {
        Debug.Log(map_camera.fieldOfView);
        
        map_camera.fieldOfView += 10;
        map_camera.fieldOfView = Mathf.Clamp(map_camera.fieldOfView, 40, 100);
    }
}
