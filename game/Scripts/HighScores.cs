using Godot;
using System;
using System.IO;

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
}
