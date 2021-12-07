using Zork;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI CurrentLocationText;

    [SerializeField] private TextMeshProUGUI MovesText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    [SerializeField] private UnityInputService InputService;
    [SerializeField] private UnityOutputService OutputService;

    private Game _game;

    //---------------------//
    void Start()
    //---------------------//
    {
        TextAsset gametextAsset = Resources.Load<TextAsset>("Zork");

        _game = JsonConvert.DeserializeObject<Game>(gametextAsset.text);
        _game.Player.LocationChanged += (sender, Location) => CurrentLocationText.text = $"Location: {Location.ToString()}";
        _game.Start(InputService, OutputService);

        CurrentLocationText.text = $"Location: {_game.StartingLocation.ToString()}";

        _game.previousLocation = _game.Player.Location;
        _game.Output.WriteLine(_game.WelcomeMessage);

        _game.Output.WriteLine(_game.Player.Location);
        Game.Look(_game);
        _game.Output.WriteLine(" ");

        InputService.InputField.Select();
        InputService.InputField.ActivateInputField();

        _game.Player.MovesChanged += (sender, moves) => MovesText.text = $"Moves: {moves.ToString()}";
        _game.Player.ScoreChanged += (sender, score) => ScoreText.text = $"Score: {score.ToString()}";

    }//END Start

    //---------------------//
    private void Update()
    //---------------------//
    {
        if (_game.IsRunning == false)
        {
            _game.Output.WriteLine(_game.ExitMessage);
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }

    }//END Update

}//END GameManager
