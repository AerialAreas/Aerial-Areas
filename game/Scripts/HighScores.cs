using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;

public partial class HighScores : Node
{
	public override void _Ready()
	{
		GetNode<Button>("HighScoresExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnHighScoresExitButton));
		GetNode<Button>("HBoxContainer/EasyButton").Connect(Button.SignalName.Pressed, Callable.From(OnEasyButtonPressed));
		GetNode<Button>("HBoxContainer/MediumButton").Connect(Button.SignalName.Pressed, Callable.From(OnMediumButtonPressed));
		GetNode<Button>("HBoxContainer/HardButton").Connect(Button.SignalName.Pressed, Callable.From(OnHardButtonPressed));
		GetNode<Button>("HBoxContainer/NightmareButton").Connect(Button.SignalName.Pressed, Callable.From(OnNightmareButtonPressed));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnHighScoresExitButton()
	{
		UIHelper.SwitchSceneTo(this, "Main Menu");
	}

	public void OnEasyButtonPressed()
	{
		string result = TextFileReader.GetStringFromTextFile("user://easy_highscores.txt"); // get data from file
		GetNode<Label>("HighScoresShownData").Text = result; // set it to the label called "HighScoresShownData"
	}
	public void OnMediumButtonPressed()
	{
		string result = TextFileReader.GetStringFromTextFile("user://medium_highscores.txt"); // get data from file
		GetNode<Label>("HighScoresShownData").Text = result; // set it to the label called "HighScoresShownData"
	}
	public void OnHardButtonPressed()
	{
		string result = TextFileReader.GetStringFromTextFile("user://hard_highscores.txt"); // get data from file
		GetNode<Label>("HighScoresShownData").Text = result; // set it to the label called "HighScoresShownData"
	}
	public void OnNightmareButtonPressed()
	{
		string result = TextFileReader.GetStringFromTextFile("user://nightmare_highscores.txt"); // get data from file
		GetNode<Label>("HighScoresShownData").Text = result; // set it to the label called "HighScoresShownData"
	}

	public static void UpdateHighScore(String difficulty, String player_name, int score)
    {
        string result = TextFileReader.GetStringFromTextFile($"user://{difficulty}_highscores.txt");
		string[] strings = result.Split("\n");
		List<string> names = new List<string>();
		List<string> scores = new List<string>();
		foreach(string str in strings)
        {
			if (str.Length!=0) {
                names.Add(str.Split(":")[0]);
				scores.Add(str.Split(":")[1]);
			}
        }
		if(score > scores[4].ToInt()){
			names[4] = player_name;
			scores[4] = score.ToString();
			for(int i = 4; i > 0; i--)
        	{
				if (scores[i].ToInt() > scores[i-1].ToInt())
                {
					string temp_name = names[i];
					string temp_score = scores[i];
					names[i] = names[i-1];
					scores[i] = scores[i-1];
					names[i-1] = temp_name;
					scores[i-1] = temp_score;
                }
        	}
			string new_result = "";
            for (int i = 0; i < 5; i++)
            {
                new_result += names[i] + ":" + scores[i] + "\n";
            }
			using var file = Godot.FileAccess.Open($"user://{difficulty}_highscores.txt", Godot.FileAccess.ModeFlags.Write);
			file.StoreString(new_result);
		}
    }
}
