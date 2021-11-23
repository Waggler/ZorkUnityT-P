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

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.Return))
        {
            if (string.IsNullOrWhiteSpace(InputField.text) == false)
            {
                string inputString = InputField.text.Trim().ToUpper();
                InputReceived?.Invoke(this, inputString);
            }

            InputField.text = string.Empty;

            //InputField.Select();
            //InputField.ActivateInputField();
        }

    }

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
    /*
    public void GetInput()
    {
        string inputString = InputField.text;
        if (string.IsNullOrWhiteSpace(inputString) == false)
        {
            InputReceived?.Invoke(this, inputString);
        }

        InputField.text = string.Empty;
    }
    */
}
