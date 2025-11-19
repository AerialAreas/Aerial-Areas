using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Formulas : Node2D
{
	private int current_formula = 0;
	List<Sprite2D> shape_formulas = new List<Sprite2D>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shape_formulas.Add(GetNode<Sprite2D>("RectanglesFormula"));
		shape_formulas.Add(GetNode<Sprite2D>("TrianglesFormula"));
		shape_formulas.Add(GetNode<Sprite2D>("CirclesFormula"));
		ShowFormula();
		GetNode<Button>("FormulasExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnFormulasExitButton));
		GetNode<Button>("LeftButton").Connect(Button.SignalName.Pressed, Callable.From(OnLeftButton));
		GetNode<Button>("RightButton").Connect(Button.SignalName.Pressed, Callable.From(OnRightButton));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnFormulasExitButton()
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

	public void OnLeftButton()
    {
        current_formula--;
		if (current_formula < 0)
        {
            current_formula = shape_formulas.Count - 1;
        }
		ShowFormula();
    }

	public void OnRightButton()
    {
        current_formula++;
		if (current_formula >= shape_formulas.Count)
        {
            current_formula = 0;
        }
		ShowFormula();
    }

	public void ShowFormula()
    {
        for (int i = 0; i < shape_formulas.Count; i++)
        {
            if (i == current_formula)
            {
                shape_formulas[i].Visible = true;
            }
			else
            {
                shape_formulas[i].Visible = false;
            }
        }
    }
}
    
