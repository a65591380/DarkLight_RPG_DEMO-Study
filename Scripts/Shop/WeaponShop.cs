using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour {

    public static WeaponShop _instance;

    public int[] itemIDList;
    private void Awake()
    {
        _instance = this;
    }
    
}
