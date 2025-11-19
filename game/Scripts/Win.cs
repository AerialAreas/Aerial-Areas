using Godot;
using System;

public partial class Win : Node2D
{
    public override void _Ready()
    {
		GameLogic.inGame = false;
		GameLogic.inShop = false;
		GameLogic.isPaused = false;
		GetNode<Label>("Score").Text = $"Score: {GameLogic.score}";
		GetNode<Button>("ExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnExitButton));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnExitButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}
