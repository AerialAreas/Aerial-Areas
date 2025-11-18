using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class MainMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public bool nightmare_unlocked = true;
	public override void _Ready()
	{
		GD.Print("Hello, program started");
		List<string> high_scores_paths = new List<string>{"user://easy_highscores.txt", "user://medium_highscores.txt", "user://hard_highscores.txt", "user://nightmare_highscores.txt"};
		foreach(string high_score_path in high_scores_paths)
        {
            if (!File.Exists(high_score_path))
            {
				using var file = Godot.FileAccess.Open(high_score_path, Godot.FileAccess.ModeFlags.Write);
				file.StoreString($"blank sample data for the path: {high_score_path}\nJimbo: 100,000\nErik: 98,000\nMike Vrabel: 81,000\nJohnathan: 50,000\nGeoff: 34,500");
				GD.Print($"Created high score file: {high_score_path}");
            }
        }
        if (nightmare_unlocked)
        {
            GetNode<Button>("DifficultyContainer/NightmareButton").Text = "Nightmare";
        }
        else
        {
			GetNode<Button>("DifficultyContainer/NightmareButton").Text = "Nightmare🔒";
        }

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print("Hi, a tick has passed");
	}

	public void OnExitButtonPressed()
    {
        GD.Print("You clicked exit sir!!!");
		GetTree().Quit();
    }

	public void OnFormulasButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Formulas.tscn");
    }

	public void OnHighScoresButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/HighScores.tscn");
    }

	public void OnOptionsButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Options.tscn");
    }

	public void OnStartButtonPressed()
    {
        GetNode<VBoxContainer>("DifficultyContainer").Visible = true;
    }

	public void OnEasyButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
		Game g = new Game(GetNode<TextEdit>("TextEdit").Text, "easy");
    }
	public void OnMediumButtonPressed()
    {
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
		Game g = new Game(GetNode<TextEdit>("TextEdit").Text, "medium");

    }
	public void OnHardButtonPressed()
    {
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
		Game g = new Game(GetNode<TextEdit>("TextEdit").Text, "hard");

    }
	public void OnNightmareButtonPressed()
    {
		if(nightmare_unlocked)
		{
			GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");  
			Game g = new Game(GetNode<TextEdit>("TextEdit").Text, "nightmare"); 
		}
    }
}
