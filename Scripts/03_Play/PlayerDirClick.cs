using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDirClick :MonoBehaviour {

    bool isMoving = false;

    public GameObject click_effect_prefab;
    public Vector3 targetPos = Vector3.zero;

    private PlayerMove playerMove;
    private PlayerAttack state;
    public void Start()
    {
        playerMove = this.GetComponent<PlayerMove>();
        targetPos = transform.position;
        state = this.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update () {
        if(state.state!=PlayerState.Death&& state.state != PlayerState.SkillAttack)
        {
            //判断鼠标是否点击在UI上
            //Is the pointer with the given ID over an EventSystem object?
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //把点击位置转换成射线
                RaycastHit hitInfo;
                bool isCollider = Physics.Raycast(ray, out hitInfo);


                if (isCollider && hitInfo.collider.tag == Tags.ground)
                {
                    isMoving = true;
                    ShowClickEffect(hitInfo.point);     //点击特效
                    LookAtTarget(hitInfo.point);        //转向
                }
            }
        }
     
        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }

        //摁住鼠标时也能转向
        if (isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //把点击位置转换成射线
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            LookAtTarget(hitInfo.point);        //转向
        }
        //由于鼠标抬起后角色依然在移动，发生的碰撞可能导致Position偏移
        //实时更新目标位置，防止由于位置更新的不正确导致角色一直往前移（持续摁住鼠标移动后会发生）
        else
        {
            if (playerMove.isMove)
            {
                LookAtTarget(targetPos);
            }

        }
    }

    //实例化点击特效
    void ShowClickEffect(Vector3 hitPoint)
    {
        hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.2f, hitPoint.z);
        GameObject.Instantiate(click_effect_prefab,hitPoint,Quaternion.identity);
    }

    //让主角朝向目标位置
     void LookAtTarget(Vector3 hitPoint)
    {
        targetPos = hitPoint;
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);        //Y轴坐标用自身的，防止点击后角色浮空
        this.transform.LookAt(targetPos);
    }
}
