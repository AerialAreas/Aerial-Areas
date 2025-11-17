using Godot;
using System;

public partial class Options : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        Button backtomain = GetNode<Button>("OptionsExitButton");
		backtomain.Connect(Button.SignalName.Pressed, Callable.From(OnOptionsExitButton));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnOptionsExitButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}
