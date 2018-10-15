using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    private PlayerMove move;
    private PlayerAttack attack;

    // Use this for initialization
    void Start()
    {
        move = this.GetComponent<PlayerMove>();
        attack = this.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (attack.state == PlayerState.ControlWalk)
        {
            if (move.state == ControlWalkState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == ControlWalkState.Idle)
            {
                PlayAnim("Idle");
            }
        }
        else if (attack.state == PlayerState.NormalAttack)
        {
            if (attack.attack_state == AttackState.Moving)
            {
                PlayAnim("Run");
            }
        }
    }

    void PlayAnim(string animName)
    {
        GetComponent<Animation>().CrossFade(animName);
    }
}
