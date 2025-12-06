using Godot;
using System;

public partial class GameOver : Node
{
    public override void _Ready()
    {
        GameLogic.isPaused = false;
        GetNode<Label>("Score").Text = $"Score: {GameLogic.score}\nWave: {GameLogic.wave_num}";
        HighScores.UpdateHighScore(GameLogic.difficulty, GameLogic.player_name, GameLogic.score);
        GetNode<AudioStreamPlayer>("GameOverSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
        GetNode<AudioStreamPlayer>("GameOverSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/gameOver.wav");
        if (UIHelper.volume == 0)
        {
            GetNode<AudioStreamPlayer>("GameOverSFX").VolumeDb = -80.0f;
        }
        if (UIHelper.sfx) {
            GetNode<AudioStreamPlayer>("GameOverSFX").Play();
        }
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
