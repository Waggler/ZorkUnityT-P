using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zork;
using TMPro;

public class UnityOutputService : MonoBehaviour, IOutputService
{

    [SerializeField] private TextMeshProUGUI OutputText;

    void Write(object value);

    void WriteLine(object value);

    void WriteLine(string value)
    {
        OutputText.text = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //-----------------------//
    void Update()
    //-----------------------//
    {
        if (Input.GetKey(KeyCode.Return))
        {
            string inputString = InputField.text;
            if (string.IsNullOrWhiteSpace(inputString) == false)
            {
                InputReceived?.Invoke(this, inputString);
            }

            InputField.text = string.Empty;
        }


    }//END Update

}
