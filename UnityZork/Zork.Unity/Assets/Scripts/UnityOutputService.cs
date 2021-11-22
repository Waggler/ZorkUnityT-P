using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zork;
using TMPro;
using Zork.Common;

public class UnityOutputService : MonoBehaviour, IOutputService
{

    [SerializeField] private TextMeshProUGUI OutputText;
    [SerializeField] private TMP_InputField InputField;

    public void Clear()
    {

    }

    public void Write(string value)
    {

    }

    public void Write(object value) => Write(value.ToString());

    public void WriteLine(object value)
    {
        OutputText.text = value.ToString();
    }

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

    void IOutputService.Write(object value)
    {
        throw new NotImplementedException();
    }

    void IOutputService.WriteLine(object value)
    {
        throw new NotImplementedException();
    }
}
