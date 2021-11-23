using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zork;
using TMPro;
using Zork.Common;
using System.Linq;
using UnityEngine.UIElements;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField] private int MaxEntries = 60;

    [SerializeField] private Transform OutputTextContainer;

    [SerializeField] private TextMeshProUGUI TextLinePrefab;

    [SerializeField] private Image NewLinePrefab;

    private readonly List<GameObject> mEntries;

    //[SerializeField] private TextMeshProUGUI OutputText;
    //[SerializeField] private TMP_InputField InputField;

    public UnityOutputService()
    {
        mEntries = new List<GameObject>();
    }

    public void Clear()
    {
        mEntries.ForEach(EntryPointNotFoundException => Destroy(entry));
        mEntries.Clear();
    }
    public void Write(string value) => ParseAndWriteLine(value);

    public void WriteLine(string value) => ParseAndWriteLine(value);

    /*
    public void Write(object value) => Write(value.ToString());
    
    public void WriteLine(object value)
    {
        OutputText.text = value;
    }

    void WriteLine(string value)
    {
        OutputText.text = value;
    }
    */

    private void ParseAndWriteLine(string value)
    {
        string[] delimiters = { "\n" };

        var lines = value.Split(delimiters, StringSplitOptions.None);
        foreach (var line in lines)
        {
            if (mEntries.Count >= MaxEntries)
            {
                var entry = mEntries.First();
                Destroy(entry);
                mEntries.Remove(entry);
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                WriteNewLine();
            }
            else
            {
                WriteTextLine(line);
            }
        }
    }

    private void WriteNewLine()
    {
        var newLine = Instantiate(NewLinePrefab, );
        newLine.transform.SetParent(OutputTextContainer, false);
        mEntries.Add(newLine.gameObject);
    }

    private void WriteTextLine(string value)
    {
        var textLine = Instantiate(TextLinePrefab);
        textLine.transform.SetParent(OutputTextContainer, false);
        textLine.text = value;
        mEntries.Add(textLine.gameObject);
    }

    /*
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
    */

    /*
    void IOutputService.Write(object value)
    {
        throw new NotImplementedException();
    }

    void IOutputService.WriteLine(object value)
    {
        throw new NotImplementedException();
    }
    */
}