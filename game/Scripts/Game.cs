using Godot;
using System;

public partial class Game : Node
{
	private string player_name;
	private string difficulty;
	public Game(string player_name, string difficulty)
    {
        this.player_name = player_name.Trim() == "" ? "Anonymous" : player_name;
		this.difficulty = difficulty;
    }
	 
	public Game()
    {
        player_name = "Anonymous";
		difficulty = "easy";
	}
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
