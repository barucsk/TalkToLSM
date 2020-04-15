using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input : MonoBehaviour
{

    private TouchScreenKeyboard mobileKeys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInputEvent()
    {
        mobileKeys = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false);
    }
}
