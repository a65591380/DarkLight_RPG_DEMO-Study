using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ValueChange : MonoBehaviour {

    
    private Text value_text;
    private Slider slider;
    public float SliderValue
    {
        set
        {
            slider.value = value;
        }
    }
    private void Awake()
    {
        slider = this.GetComponent<Slider>();
        value_text = transform.Find("Bar/Text").GetComponentInChildren<Text>();
    }
  

    public void OnValueChanged()
    {
        value_text.text = Mathf.Round((slider.value * 100)).ToString() + "%";
    }
	
}
