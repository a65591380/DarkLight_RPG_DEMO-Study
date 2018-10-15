//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackTrigger : MonoBehaviour {

//    private WolfBaby wolfbaby;

//    private void Start()
//    {
//        wolfbaby = this.GetComponentInParent<WolfBaby>();
//    }
//    private void OnTriggerEnter(Collider other)
//    {

//        if (other.tag == Tags.player)
//        {
//            wolfbaby.attack_target = other.transform;
//            wolfbaby.state = WolfState.Attack;
//        }
//    }
//}
