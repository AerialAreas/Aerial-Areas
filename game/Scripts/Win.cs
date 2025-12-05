using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

public partial class Win : Node2D
{
	public override void _Ready()
	{
		GameLogic.isPaused = false;
		GetNode<Label>("Score").Text = $"Score: {GameLogic.score}";
		HighScores.UpdateHighScore(GameLogic.difficulty, GameLogic.player_name, GameLogic.score);
		if (!GameLogic.nightmare_unlocked && GameLogic.difficulty == "hard")
        {
            GetNode<Label>("Reward").Text = "You Unlocked Nightmare Mode!";
			MainMenu.UpdateNightmare();
        }
		GetNode<AudioStreamPlayer>("WinSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
        GetNode<AudioStreamPlayer>("WinSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/win.wav");
        if (UIHelper.volume == 0)
        {
            GetNode<AudioStreamPlayer>("WinSFX").VolumeDb = -80.0f;
        }
        if (UIHelper.sfx) {
            GetNode<AudioStreamPlayer>("WinSFX").Play();
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
