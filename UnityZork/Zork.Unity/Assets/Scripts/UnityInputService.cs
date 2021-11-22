using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zork;
using TMPro;
using Zork.Common;

public class UnityInputService : MonoBehaviour, IInputService
{

    [SerializeField]private TMP_InputField InputField;

    public event EventHandler<string> InputReceived;

    /*
    public void GetInput()
    {
        string inputString = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputString) == false)
        {
            InputReceived?.Invoke(this, inputString);
        }
        
    }
    */
    void Update()
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
    }
    public void GetInput()
    {
        string inputString = InputField.text.Trim().ToUpper();
        if (string.IsNullOrWhiteSpace(inputString) == false)
        {
            InputReceived?.Invoke(this, inputString);
        }

        InputField.text = string.Empty;
    }

}
