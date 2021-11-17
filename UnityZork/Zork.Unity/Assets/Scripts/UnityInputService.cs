using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zork;
using TMPro;

public class UnityInputService : MonoBehaviour, IInputService
{

    [SerializeField]private TMP_InputField InputField;

    event EventHandler<string> InputReceived;


    public void ProcessInput()
    {

    }
}
