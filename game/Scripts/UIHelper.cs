using Godot;
using System;
using System.Collections.Generic;

public static class UIHelper
{
	public static int volume = 100;
	public static bool music = true;
	public static bool sfx = true;
	public static string current_scene = "Main Menu";
	public static string previous_scene = "Main Menu";
	public static bool skip_tutorials = true; // usually set to false, but for development we should just keep this to true unless we want to work on the tutorial
	public static Dictionary<string, string> scene_to_scene_path = new Dictionary<string, string>
	{
		{"Formulas", "res://Scenes/Formulas.tscn"},
		{"High Scores", "res://Scenes/HighScores.tscn"},
		{"Options", "res://Scenes/Options.tscn"},
		{"Game", "res://Scenes/Game.tscn"},
		{"Shop", "res://Scenes/Shop.tscn"},
		{"Main Menu", "res://Scenes/MainMenu.tscn"},
		{"Game Over", "res://Scenes/GameOver.tscn"},
		{"Win", "res://Scenes/Win.tscn"},
		{"Tutorial", "res://Scenes/Tutorial.tscn"}
	};
	public static void SwitchSceneTo(Node node, string new_scene)
	{
		// GD.Print($"BEFORE Current Scene is {current_scene}, Previous Scene is {previous_scene}");
		node.GetTree().ChangeSceneToFile(scene_to_scene_path[new_scene]);
		previous_scene = current_scene;
		current_scene = new_scene;
		// GD.Print($"AFTER Current Scene is {current_scene}, Previous Scene is {previous_scene}");
	}

}
