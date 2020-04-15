using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonScript : MonoBehaviour
{
    public Animator animOptionButton;
    public Animator animSoundButton;
    public Animator animTraductorButton;
    public GameObject soundButton;
    public GameObject traductorButton;
    public GameObject panelTextTraduction;

    // Start is called before the first frame update
    void Start()
    {
        panelTextTraduction = GameObject.Find("PanelTW");
        panelTextTraduction.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void showOptions()
    {
        animOptionButton = GetComponent<Animator>(); 
        soundButton = GameObject.Find("SoundButton");
        traductorButton = GameObject.Find("TraductorButton");
        animSoundButton = soundButton.GetComponent<Animator>();
        animTraductorButton = traductorButton.GetComponent<Animator>();

        bool valueSB = animSoundButton.GetBool("DESPLEGAR");
        bool valueOB = animOptionButton.GetBool("DESPLEGAR");
        bool valueTB = animTraductorButton.GetBool("DESPLEGAR");


        if (valueOB == true) 
        {
            animOptionButton.SetBool("DESPLEGAR", false);
            animTraductorButton.SetBool("DESPLEGAR", false);
            animSoundButton.SetBool("DESPLEGAR", false);
        }
        else
        {
            animOptionButton.SetBool("DESPLEGAR", true);
            animTraductorButton.SetBool("DESPLEGAR", true);
            animSoundButton.SetBool("DESPLEGAR", true);
        }
    }

    public void showTraductorWindow()
    {
        panelTextTraduction.gameObject.SetActive(true);
    }

    public void cancelTraductorWindow() 
    {
        panelTextTraduction.gameObject.SetActive(false);
    }

}
