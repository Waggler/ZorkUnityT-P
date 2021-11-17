using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private string ZorkGameFileAssetName = "Zork";
    [SerializeField] private UnityOutPutService Output;


    //-----------------------//
    void Awake()
    //-----------------------//
    {
        TextAsset gameJsonAsset = Resources.Load<TextAsset>(ZorkGameFileAssetName);

        Game.Start(gameJsonAsset.text, Input, Output);

    }//END Awake

    //-----------------------//
    void Start()
    //-----------------------//
    {

    }//END Start

    //-----------------------//
    void Update()
    //-----------------------//
    {
        if (Input.GetKey(KeyCode.Return))
        {
            InputService.ProcessInput();    
        }


    }//END Update

}//END GameManager
