using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMovingCamera : MonoBehaviour {

    public float speed = 5;

    private float endZPos = -20;

	
	// Update is called once per frame
	void Update () {
		if(transform.position.z<endZPos)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
           
        }
	}
}
