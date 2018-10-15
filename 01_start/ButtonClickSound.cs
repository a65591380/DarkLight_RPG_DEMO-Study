using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour {

    private AudioSource audioSource;
    private Button button;
    

	// Use this for initialization
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
        button = this.GetComponent<Button>();
        button.onClick.AddListener(PlayClip);
    }
	
	// Update is called once per frame
	void Update () {

        //button.onClick.AddListener(PlayClip);
	}

    void PlayClip()
    {
        audioSource.Play();
    }

   
}
