using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonScript : MonoBehaviour
{
    public Animator anim;
    public Animator animSoundButton;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void showOptions()
    {
        GameObject button = GameObject.Find("SoundButton");
        anim = GetComponent<Animator>();
        bool value = anim.GetBool("DESPLEGAR");
        animSoundButton = button.GetComponent<Animator>();
        bool valueSB = animSoundButton.GetBool("DESPLEGAR");


        if (value == true) 
        {
            anim.SetBool("DESPLEGAR", false);

            animSoundButton.SetBool("DESPLEGAR", false);
        }
        else
        {
            anim.SetBool("DESPLEGAR", true);

            animSoundButton.SetBool("DESPLEGAR", true);
        }
    }
}
