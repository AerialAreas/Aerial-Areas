using Godot;
using System;

public partial class GameOver : Node
{
    public override void _Ready()
    {
        GameLogic.isPaused = false;
        GetNode<Label>("Score").Text = $"Score: {GameLogic.score}\nWave: {GameLogic.wave}";
        HighScores.UpdateHighScore(GameLogic.difficulty, GameLogic.player_name, GameLogic.score);
        InitializeUIEvents();
    }

    public void InitializeUIEvents()
    {
        GetNode<Button>("ExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnExitButton));
    }

    public void OnExitButton()
    {
        UIHelper.SwitchSceneTo(this, "Main Menu");
    }
}
