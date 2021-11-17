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

    event EventHandler<string> InputReceived;


    public void ProcessInput()
    {

    }
}
