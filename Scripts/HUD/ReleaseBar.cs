using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReleaseBar : MonoBehaviour {

    private Slider releaseBar_slider;
    public float timer = 0;
    private float release_time;
    private void Awake()
    {
        release_time = GameObject.FindWithTag(Tags.player).GetComponent<PlayerAttack>().skillRelease_time;
        releaseBar_slider = this.GetComponent<Slider>();
        transform.position = Camera.main.WorldToScreenPoint(GameObject.FindWithTag(Tags.player).transform.position) + new Vector3(0, 50, 0);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        releaseBar_slider.value = timer / release_time;
        if(timer>=release_time)
        {
            Destroy(this.gameObject);
        }
    }
}
