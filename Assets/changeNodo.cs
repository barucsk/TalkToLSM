using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeNodo : StateMachineBehaviour
{
    public Animator animRosa;
    public GameObject rosa;
    public MainButtonScript mbs;

    public void OnStateEnter()
    {
        rosa = GameObject.Find("Rosa");
        animRosa = rosa.GetComponent<Animator>();
    }

    public void OnStateExit()
    {
        mbs = FindObjectOfType<MainButtonScript>();
        mbs.contador++;
        animRosa.SetInteger("INDICADOR", mbs.ordenAnimaciones[mbs.contador]);
    }
}
