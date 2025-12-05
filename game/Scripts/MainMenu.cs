using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class MainMenu : Node2D
{
	public override void _Ready()
	{
		InitializeUIEvents();
		InitializeTemporaryHighScores();
		ReadPlayerDataFile();
		GameLogic.inGame = false;
	}

	public void ReadPlayerDataFile()
	{
		string player_data_file_path = "user://player_data.txt";
		if (!Godot.FileAccess.FileExists(player_data_file_path))
		{
			using var file = Godot.FileAccess.Open(player_data_file_path, Godot.FileAccess.ModeFlags.Write);
			file.StoreString($"Anonymous:false:100:True:True:False");
			GD.Print($"Created player data file: {player_data_file_path}");
		}
		string result = TextFileReader.GetStringFromTextFile(player_data_file_path);
		string []strings = result.Split(":");
		strings[5].Replace("\n", "");
		GetNode<LineEdit>("EnterName").Text = strings[0];
		if (strings[1] == "true")
        {
            GameLogic.nightmare_unlocked = true;
        }
		UIHelper.volume = strings[2].ToInt();
		UIHelper.music = Boolean.Parse(strings[3]);
		UIHelper.sfx = Boolean.Parse(strings[4]);
		UIHelper.skip_tutorials = Boolean.Parse(strings[5]);

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
				//file.StoreString($"blank sample data for the path: {high_score_path}\nJimbo: 100,000\nErik: 98,000\nMike Vrabel: 81,000\nJohnathan: 50,000\nGeoff: 34,500");
				file.StoreString("Jimbo:100000\nErik:98000\nMike Vrabel:81000\nJohnathan:50000\nGeoff:34500");
				GD.Print($"Created high score file: {high_score_path}");
			}
		}
	}
	public void OnExitButtonPressed()
	{
		GameLogic.player_name = GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "") == "" ? "Anonymous" : GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "");
        if (GameLogic.player_name.Length > 16)
        {
            GameLogic.player_name = GameLogic.player_name.Substring(0, 16);
        }
		UpdateName(GameLogic.player_name);
		GetTree().Quit();
	}

	public void OnFormulasButtonPressed()
	{
		GameLogic.player_name = GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "") == "" ? "Anonymous" : GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "");
        if (GameLogic.player_name.Length > 16)
        {
            GameLogic.player_name = GameLogic.player_name.Substring(0, 16);
        }
		UpdateName(GameLogic.player_name);
		UIHelper.SwitchSceneTo(this, "Formulas");
	}

	public void OnHighScoresButtonPressed()
	{
		GameLogic.player_name = GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "") == "" ? "Anonymous" : GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "");
        if (GameLogic.player_name.Length > 16)
        {
            GameLogic.player_name = GameLogic.player_name.Substring(0, 16);
        }
		UpdateName(GameLogic.player_name);
		UIHelper.SwitchSceneTo(this, "High Scores");
	}

	public void OnOptionsButtonPressed()
	{
		GameLogic.player_name = GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "") == "" ? "Anonymous" : GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "");
        if (GameLogic.player_name.Length > 16)
        {
            GameLogic.player_name = GameLogic.player_name.Substring(0, 16);
        }
		UpdateName(GameLogic.player_name);
		UIHelper.SwitchSceneTo(this, "Options");
	}

	public void OnStartButtonPressed()
	{
		GetNode<VBoxContainer>("DifficultyContainer").Visible = true;
	}

	public void StartGame(string difficulty)
	{
		GameLogic.player_name = GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "") == "" ? "Anonymous" : GetNode<LineEdit>("EnterName").Text.Trim().StripEscapes().Replace(":", "");
        if (GameLogic.player_name.Length > 16)
        {
            GameLogic.player_name = GameLogic.player_name.Substring(0, 16);
        }
		UpdateName(GameLogic.player_name);
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
	public void UpdateName(string newName)
    {
        string result = TextFileReader.GetStringFromTextFile("user://player_data.txt");
		string []strings = result.Split(":");
		strings[5].Replace("\n", "");
		if (newName != strings[0]) {
			strings[0] = newName;
			string newResult = "";
			foreach(string str in strings)
        	{
            	newResult += str + ":";
        	}
			newResult = newResult.Substring(0, newResult.Length - 1);
			using var file = Godot.FileAccess.Open("user://player_data.txt", Godot.FileAccess.ModeFlags.Write);
			file.StoreString(newResult);
		}
    }
	public static void UpdateNightmare()
    {
        string result = TextFileReader.GetStringFromTextFile("user://player_data.txt");
		string []strings = result.Split(":");
		strings[5].Replace("\n", "");
		if (strings[1] != "true") {
			strings[1] = "true";
			string newResult = "";
			foreach(string str in strings)
        	{
            	newResult += str + ":";
        	}
			newResult = newResult.Substring(0, newResult.Length - 1);
			using var file = Godot.FileAccess.Open("user://player_data.txt", Godot.FileAccess.ModeFlags.Write);
			file.StoreString(newResult);
		}
    }
	public static void UpdateVolumeSlider(int volume)
    {
        string result = TextFileReader.GetStringFromTextFile("user://player_data.txt");
		string []strings = result.Split(":");
		strings[5].Replace("\n", "");
		if (strings[2] != volume.ToString()) {
			strings[2] = volume.ToString();
			string newResult = "";
			foreach(string str in strings)
        	{
            	newResult += str + ":";
        	}
			newResult = newResult.Substring(0, newResult.Length - 1);
			using var file = Godot.FileAccess.Open("user://player_data.txt", Godot.FileAccess.ModeFlags.Write);
			file.StoreString(newResult);
		}
    }
	public static void UpdateSettings(int index, bool isToggled)
    {
        string result = TextFileReader.GetStringFromTextFile("user://player_data.txt");
		string []strings = result.Split(":");
		strings[5].Replace("\n", "");
		if (strings[index] != isToggled.ToString() && strings[index] != (isToggled.ToString() + "\n"))
        {
            strings[index] = isToggled.ToString();
        }
		string newResult = "";
		foreach(string str in strings)
        {
            newResult += str + ":";
        }
            newResult = newResult.Substring(0, newResult.Length - 1);
		using var file = Godot.FileAccess.Open("user://player_data.txt", Godot.FileAccess.ModeFlags.Write);
		file.StoreString(newResult);
    }
}
