using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceStatus : MonoBehaviour {

    private Text name_text;
    
    private ValueChange hpStatus;
    private ValueChange mpStatus;
    private ValueChange expStatus;

    private Text numHP_text;
    private Text numMP_text;
    private Text numEXP_text;

    private Image death_image;
    private PlayerAttack attack;
    private void Awake()
    {
        attack = GameObject.FindWithTag(Tags.player).GetComponent<PlayerAttack>();
        name_text = transform.Find("NameImage/Text").GetComponent<Text>();
        hpStatus = transform.Find("HPImage").GetComponent<ValueChange>();
        mpStatus = transform.Find("MPImage").GetComponent<ValueChange>();
        expStatus = transform.Find("EXPImage").GetComponent<ValueChange>();

        numHP_text = transform.Find("HPImage/NumText").GetComponent<Text>();
        numMP_text = transform.Find("MPImage/NumText").GetComponent<Text>();
        numEXP_text= transform.Find("EXPImage/NumText").GetComponent<Text>();

        death_image = transform.Find("FaceMask_RawImage/DeathImage").GetComponent<Image>();
    }

    private void Update()
    {
        if(attack.state==PlayerState.Death)
        {
            death_image.enabled = true;
        }
        else
        {
            death_image.enabled = false;
        }
        name_text.text = "LV" + PlayerStatus._instance.grade + " " + PlayerStatus._instance.player_name;
        hpStatus.SliderValue = PlayerStatus._instance.hp_current / PlayerStatus._instance.hp_max;
        mpStatus.SliderValue = PlayerStatus._instance.mp_current / PlayerStatus._instance.mp_max;
        expStatus.SliderValue = PlayerStatus._instance.exp_current / PlayerStatus._instance.exp_levelup;

        numHP_text.text = PlayerStatus._instance.hp_current + " / " + PlayerStatus._instance.hp_max;
        numMP_text.text= PlayerStatus._instance.mp_current +" / "+ PlayerStatus._instance.mp_max;
        numEXP_text.text = PlayerStatus._instance.exp_current+ " / " + PlayerStatus._instance.exp_levelup;
    }
}
