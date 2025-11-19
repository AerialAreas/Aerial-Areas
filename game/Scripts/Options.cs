using Godot;
using System;

public partial class Options : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		GetNode<Button>("OptionsExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsExitButton));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnOptionsExitButton()
    {
        if (GameLogic.inGame)
        {
            if (GameLogic.inShop)
            {
                GetTree().ChangeSceneToFile("res://Scenes/Shop.tscn");
                return;
            }
            GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
        }
		else
        {
            GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
        }
    }
}
