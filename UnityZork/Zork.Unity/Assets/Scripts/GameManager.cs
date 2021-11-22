using UnityEngine;
using Zork.Common;
using Zork;

public class GameManager : MonoBehaviour
{

    [SerializeField] private string ZorkGameFileAssetName = "Zork";
    [SerializeField] private UnityInputService InputService;
    [SerializeField] private UnityOutputService OutputService;


    //-----------------------//
    void Awake()
    //-----------------------//
    {
        TextAsset gameJsonAsset = Resources.Load<TextAsset>(ZorkGameFileAssetName);

        Game.Start(gameJsonAsset.text, InputService, OutputService);
        Game.Instance.CommandManager.PerformCommand(Game.Instance, "LOOK");

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
        //Moving to inputfield enter/onEndEdit
        
        if (Input.GetKey(KeyCode.Return))
        {
            InputService.ProcessInput();    
        }
        


    }//END Update

    //-----------------------//
    public void GetEnter()
    //-----------------------//
    {
        //IInputService.ProcessInput();


    }//END Update

}//END GameManager
