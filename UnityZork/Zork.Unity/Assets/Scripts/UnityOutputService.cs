using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zork.Common;
using System;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private int MaxEntries = 60;

    [SerializeField]
    private Transform OutputTextContainer;

    [SerializeField]
    private TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private Image NewLinePrefab;

    private readonly List<GameObject> mEntries;

    public UnityOutputService() => mEntries = new List<GameObject>();

    //---------------------//
    public void Clear() => mEntries.ForEach(entry => Destroy(entry));
    //---------------------//

    //---------------------//
    private void ParseAndWriteLine(string value)
    //---------------------//
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

    }//END ParseAndWriteLine


    //---------------------//
    public void WriteNewLine()
    //---------------------//
    {
        var newLine = Instantiate(NewLinePrefab);
        newLine.transform.SetParent(OutputTextContainer, false);
        mEntries.Add(newLine.gameObject);

    }//END WriteNewLine

    //---------------------//
    private void WriteTextLine(string value)
    //---------------------//
    {
        var textLine = Instantiate(TextLinePrefab);
        textLine.transform.SetParent(OutputTextContainer, false);
        textLine.text = value;
        mEntries.Add(textLine.gameObject);

    }//END WriteTextLine

    //---------------------//
    public void Write(object value)
    //---------------------//
    {
        ParseAndWriteLine(value.ToString());

    }//END Write

    //---------------------//
    public void WriteLine(object value)
    //---------------------//
    {
        ParseAndWriteLine(value.ToString());

    }//END WriteLine


}//END UnityOutputService