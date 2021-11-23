using UnityEngine;
using Zork.Common;
using Newtonsoft.Json;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentLocationText;
    [SerializeField] private TextMeshProUGUI MovesText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    [SerializeField] private UnityInputService InputService;
    [SerializeField] private UnityOutputService OutputService;

    private Game _game;

    /*
    [SerializeField] private string ZorkGameFileAssetName = "Zork";
    [SerializeField] private UnityInputService Input;
    [SerializeField] private UnityOutputService Output;
    */

    
    void Awake()
    {
        /*
        TextAsset gameJsonAsset = Resources.Load<TextAsset>(ZorkGameFileAssetName);

        Game.Start(gameJsonAsset.text, Input, Output);
        //Game.Instance.CommandManager.PerformCommand(Game.Instance, "LOOK");
        */

        /*
        TextAsset gameTextAsset = Resources.Load<TextAsset>(GameFilename);
        TextAsset worldTextAsset = Resources.Load<TextAsset>(WorldFilename);
        TextAsset itemsTextAsset = Resources.Load<TextAsset>(ItemFilename);

        var gameJsonStrings = new GameJsonStrings
        {
            GameJsonString = gameTextAsset.text,
            WorldJsonString = worldTextAsset.text,
            ItemsJsonString = itemsTextAsset.text,

        };

        Game.Initialize(gameJsonStrings, InputService, OutputService, LoadExternalScripts);
        LocationText.text = Game.Instance.Player.Location.ToString();
        Game.Instance.Player.LocationChanged += (sender, location) => CurrentLocationText.text = location.ToString();
        Game.Instance.Player.ScoreChanged += (sender, location) => ScoreText.text = $"Score: {score}";
        Game.Instance.Player.LocationChanged += (sender, location) => CurrentLocationText.text = $"Moves: {moves}";
        OutputService.WriteLine(string.Empty);
        */
    }//END Awake


    //-----------------------//
    void Start()
    //-----------------------//
    {
        TextAsset gameTextAsset = Resources.Load<TextAsset>("Zork");
        _game = JsonConvert.DeserializeObject<Game>(gameTextAsset.text);

        _game.GameStopped += _game_GameStopped;
        _game.Start(InputService, OutputService);

        //InputService.InputField.Select();
        //InputService.InputField.ActivateInputField();

        //_game.Player.LocationChanged += (sender, location) => { if (CurrentLocationText != null) {CurrentLocationText.text = currentLocation.ToString(); } }
        //_game.Player.MovesChanged += (sender, moves) => { if (MovesText != null) {MovesText.text = moves.ToString();) } }
        //_game.Player.ScoreChanged += (sender, score) => { if (ScoreText != null) {ScoreText.text = currentLocation.ToString(); } } 

    }//END Start

    
    //-----------------------//
    private void _game_GameStopped(object sender, System.EventArgs e)
    //-----------------------//
    {
        if (_game.IsRunning == false)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }//END Update
    
    /*
    //-----------------------//
    public void GetEnter()
    //-----------------------//
    {
        IInputService.ProcessInput();


    }//END GetEnter
    */

}//END GameManager
