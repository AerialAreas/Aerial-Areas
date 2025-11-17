using Godot;
using System;

public partial class Formulas : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button backtomain = GetNode<Button>("FormulasExitButton");
		backtomain.Connect(Button.SignalName.Pressed, Callable.From(OnFormulasExitButton));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnFormulasExitButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}
    
