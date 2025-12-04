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
		if (GameLogic.difficulty == "hard")
        {
            MainMenu.UpdateNightmare();
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
