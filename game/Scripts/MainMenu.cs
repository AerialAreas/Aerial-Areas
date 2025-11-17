using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class MainMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello, program started");
		List<string> high_scores_paths = new List<string>{"user://easy_highscores.txt", "user://medium_highscores.txt", "user://hard_highscores.txt", "user://nightmare_highscores.txt"};
		foreach(string high_score_path in high_scores_paths)
        {
            if (!File.Exists(high_score_path))
            {
				using var file = Godot.FileAccess.Open(high_score_path, Godot.FileAccess.ModeFlags.Write);
				file.StoreString($"blank sample data for the path: {high_score_path}");
				GD.Print($"Created high score file: {high_score_path}");
            }
        }

		Button exit_button = GetNode<Button>("ExitButton");
		exit_button.Connect(Button.SignalName.Pressed, Callable.From(OnExitButtonPressed));

		Button formulas_button = GetNode<Button>("ButtonsContainer/FormulasButton");
		formulas_button.Connect(Button.SignalName.Pressed, Callable.From(OnFormulasButtonPressed));

		Button options_button = GetNode<Button>("ButtonsContainer/OptionsButton");
		options_button.Connect(Button.SignalName.Pressed, Callable.From(OnOptionsButtonPressed));

		Button high_scores_button = GetNode<Button>("ButtonsContainer/HighScoresButton");
		high_scores_button.Connect(Button.SignalName.Pressed, Callable.From(OnHighScoresButtonPressed));
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
}
    
