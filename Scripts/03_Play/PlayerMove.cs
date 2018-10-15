using UnityEngine;
using System.Collections;

public enum ControlWalkState
{
    Moving,
    Idle
}

public class PlayerMove : MonoBehaviour
{

    public float speed = 4;
    public float originalspeed = 5;
    public ControlWalkState state = ControlWalkState.Idle;
    private PlayerDirClick dir;
    private PlayerAttack attack;
    private CharacterController controller;
    public bool isMove = false;

    void Start()
    {
        dir = this.GetComponent<PlayerDirClick>();
        controller = this.GetComponent<CharacterController>();
        attack = this.GetComponent<PlayerAttack>();
    }
    // Update is called once per frame
    void Update()
    {
        speed =originalspeed+ ((float)PlayerStatus._instance.sum_speed / 100f);
        if (attack.state == PlayerState.ControlWalk&&attack.state!=PlayerState.Death&& attack.state != PlayerState.SkillAttack)
        {
            float distance = Vector3.Distance(dir.targetPos, transform.position);
            if (distance > 0.3f)
            {
                isMove = true;
                state = ControlWalkState.Moving;
                controller.SimpleMove(transform.forward * speed);
            }
            else
            {
                isMove = false;
                state = ControlWalkState.Idle;
            }
        }
    }

    public void SimpleMove(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
        controller.SimpleMove(transform.forward * speed);
    }
}
