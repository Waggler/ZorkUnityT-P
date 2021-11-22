using UnityEngine;
using Zork.Common;
using Zork;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string ZorkGameFileAssetName = "Zork";
    [SerializeField] private UnityInputService InputService;
    [SerializeField] private UnityOutputService OutputService;

    private Game _game;


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
        TextAsset gameTextAsset = Resources.Load<TextAsset>("Zork");
        _game = JsonConvert.DeserializeObject<Game>(gameTextAsset.text);
        _game.Start(InputService, OutputService);


        //_game.Player.LocationChanged += (sender, locatiom) => { if (CurrentLocationText != null) {CurrentLocationText = location.ToString(); }
        //_game.Player.MovesChanged += (sender, locatiom) => { if (MovesText != null) {MovesText = moves.ToString(); }
        //_game.Player.ScoreChanged += (sender, locatiom) => { if (ScoreText != null) {ScoreText = score.ToString(); }

    }//END Start

    //-----------------------//
    void Update()
    //-----------------------//
    {
        //Moving to inputfield enter/onEndEdit
        /*
        if (Input.GetKey(KeyCode.Return))
        {
            InputService.ProcessInput();    
        }
        */

    }//END Update

    private void _game_GameStopped(object sender, System.EventArgs e)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //-----------------------//
    public void GetEnter()
    //-----------------------//
    {
        IInputService.ProcessInput();


    }//END Update

}//END GameManager
