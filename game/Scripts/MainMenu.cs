using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class MainMenu : Node2D
{
	public override void _Ready()
	{
		InitializeUIEvents();
		InitializeTemporaryHighScores();
		ReadPlayerDataFile();
	}

	public void ReadPlayerDataFile()
	{
		string player_data_file_path = "user://player_data.txt";
		if (!Godot.FileAccess.FileExists(player_data_file_path))
		{
			using var file = Godot.FileAccess.Open(player_data_file_path, Godot.FileAccess.ModeFlags.Write);
			file.StoreString($"nightmare_unlocked:false");
			GD.Print($"Created player data file: {player_data_file_path}");
		}
	}
	public void InitializeUIEvents() // Let's just put all of our ui events here
	{
		GetNode<Button>("ExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnExitButtonPressed));
		GetNode<Button>("ButtonsContainer/StartButton").Connect(Button.SignalName.Pressed, Callable.From(OnStartButtonPressed));
		GetNode<Button>("ButtonsContainer/FormulasButton").Connect(Button.SignalName.Pressed, Callable.From(OnFormulasButtonPressed));
		GetNode<Button>("ButtonsContainer/OptionsButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsButtonPressed));
		GetNode<Button>("ButtonsContainer/HighScoresButton").Connect(Button.SignalName.Pressed, Callable.From(OnHighScoresButtonPressed));

		GetNode<Button>("DifficultyContainer/EasyButton").Connect(Button.SignalName.Pressed, Callable.From(OnEasyButtonPressed));
		GetNode<Button>("DifficultyContainer/MediumButton").Connect(Button.SignalName.Pressed, Callable.From(OnMediumButtonPressed));
		GetNode<Button>("DifficultyContainer/HardButton").Connect(Button.SignalName.Pressed, Callable.From(OnHardButtonPressed));
		GetNode<Button>("DifficultyContainer/NightmareButton").Connect(Button.SignalName.Pressed, Callable.From(OnNightmareButtonPressed));
	}
	public void InitializeTemporaryHighScores() // This is for the sample data for high scores, but we could have predefined high scores in there
	{
		List<string> high_scores_paths = new List<string> { "user://easy_highscores.txt", "user://medium_highscores.txt", "user://hard_highscores.txt", "user://nightmare_highscores.txt" };
		foreach (string high_score_path in high_scores_paths)
		{
			if (!Godot.FileAccess.FileExists(high_score_path))
			{
				using var file = Godot.FileAccess.Open(high_score_path, Godot.FileAccess.ModeFlags.Write);
				file.StoreString($"blank sample data for the path: {high_score_path}\nJimbo: 100,000\nErik: 98,000\nMike Vrabel: 81,000\nJohnathan: 50,000\nGeoff: 34,500");
				GD.Print($"Created high score file: {high_score_path}");
			}
		}
	}
	public void OnExitButtonPressed()
	{
		GetTree().Quit();
	}

	public void OnFormulasButtonPressed()
	{
		UIHelper.SwitchSceneTo(this, "Formulas");
	}

	public void OnHighScoresButtonPressed()
	{
		UIHelper.SwitchSceneTo(this, "High Scores");
	}

	public void OnOptionsButtonPressed()
	{
		UIHelper.SwitchSceneTo(this, "Options");
	}

	public void OnStartButtonPressed()
	{
		GetNode<VBoxContainer>("DifficultyContainer").Visible = true;
	}

	public void StartGame(string difficulty)
	{
		GameLogic.player_name = GetNode<LineEdit>("EnterName").Text.Trim() == "" ? "Anonymous" : GetNode<LineEdit>("EnterName").Text.Trim();
		GameLogic.difficulty = difficulty;
		GameLogic.SetToStart(); // setting default values
		if (UIHelper.skip_tutorials)
		{
			UIHelper.SwitchSceneTo(this, "Game");
		}
		else
		{
			UIHelper.SwitchSceneTo(this, "Tutorial");
		}
	}

	public void OnEasyButtonPressed()
	{
		StartGame("easy");
	}
	public void OnMediumButtonPressed()
	{
		StartGame("medium");
	}
	public void OnHardButtonPressed()
	{
		StartGame("hard");
	}
	public void OnNightmareButtonPressed()
	{
		if (GameLogic.nightmare_unlocked)
		{
			StartGame("nightmare");
		}
	}
}