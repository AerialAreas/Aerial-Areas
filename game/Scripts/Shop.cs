using Godot;
using System;

public partial class Shop : Node2D
{
    public override void _Ready()
    {
		GameLogic.inShop = true;
		if (GameLogic.isPaused)
        {
            GetNode<VBoxContainer>("PauseMenu").Visible = true;
            GetNode<Button>("GoBack").Disabled = true;
            GetNode<Button>("Upgrades/Firecracker").Disabled = true;
            GetNode<Button>("Upgrades/Diamond").Disabled = true;
            GetNode<Button>("Upgrades/Heart").Disabled = true;
            GetNode<Button>("Powerups/Fire").Disabled = true;
            GetNode<Button>("Powerups/Ice").Disabled = true;
            GetNode<Button>("Powerups/Lightning").Disabled = true;
        }
		GetNode<Button>("GoBack").Connect(Button.SignalName.Pressed, Callable.From(OnGoBackButton));
		GetNode<Button>("PauseMenu/MainMenuButton").Connect(Button.SignalName.Pressed, Callable.From(OnMainMenuButton));
		GetNode<Button>("PauseMenu/OptionsButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsButton));
		GetNode<Button>("PauseMenu/FormulasButton").Connect(Button.SignalName.Pressed, Callable.From(OnFormulasButton));
		GetNode<Button>("PauseMenu/Resume").Connect(Button.SignalName.Pressed, Callable.From(OnResumeButton));

        GetNode<Button>("Upgrades/Firecracker").Connect(Button.SignalName.Pressed, Callable.From(FirecrackerBought));
        GetNode<Button>("Upgrades/Diamond").Connect(Button.SignalName.Pressed, Callable.From(DiamondBought));
        GetNode<Button>("Upgrades/Heart").Connect(Button.SignalName.Pressed, Callable.From(HeartBought));
        GetNode<Button>("Powerups/Fire").Connect(Button.SignalName.Pressed, Callable.From(FireBought));
        GetNode<Button>("Powerups/Ice").Connect(Button.SignalName.Pressed, Callable.From(IceBought));
        GetNode<Button>("Powerups/Lightning").Connect(Button.SignalName.Pressed, Callable.From(LightningBought));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
				GameLogic.isPaused = !GameLogic.isPaused;
                GetNode<VBoxContainer>("PauseMenu").Visible = !GetNode<VBoxContainer>("PauseMenu").Visible;
				GetNode<Button>("GoBack").Disabled = !GetNode<Button>("GoBack").Disabled;
                GetNode<Button>("Upgrades/Firecracker").Disabled = !GetNode<Button>("Upgrades/Firecracker").Disabled;
                GetNode<Button>("Upgrades/Diamond").Disabled = !GetNode<Button>("Upgrades/Diamond").Disabled;
                GetNode<Button>("Upgrades/Heart").Disabled = !GetNode<Button>("Upgrades/Heart").Disabled;
                GetNode<Button>("Powerups/Fire").Disabled = !GetNode<Button>("Powerups/Fire").Disabled;
                GetNode<Button>("Powerups/Ice").Disabled = !GetNode<Button>("Powerups/Ice").Disabled;
                GetNode<Button>("Powerups/Lightning").Disabled = !GetNode<Button>("Powerups/Lightning").Disabled;
            }
        }
    }

	public void OnMainMenuButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }

	public void OnOptionsButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Options.tscn");
    }

	public void OnFormulasButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Formulas.tscn");
    }

	public void OnResumeButton()
    {
        GameLogic.isPaused = false;
        GetNode<VBoxContainer>("PauseMenu").Visible = false;
        GetNode<Button>("GoBack").Disabled = false;
        GetNode<Button>("Upgrades/Firecracker").Disabled = false;
        GetNode<Button>("Upgrades/Diamond").Disabled = false;
        GetNode<Button>("Upgrades/Heart").Disabled = false;
        GetNode<Button>("Powerups/Fire").Disabled = false;
        GetNode<Button>("Powerups/Ice").Disabled = false;
        GetNode<Button>("Powerups/Lightning").Disabled = false;
    }

	public void OnGoBackButton()
    {
        GameLogic.inShop = false;
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }
    public void FirecrackerBought()
    {
        GetNode<Label>("DebugText").Text = "🧨 upgrade bought";
    }
    public void DiamondBought()
    {
        GetNode<Label>("DebugText").Text = "💎 upgrade bought";
    }
    public void HeartBought()
    {
        GetNode<Label>("DebugText").Text = "❤️ upgrade bought";
    }
    public void FireBought()
    {
        GetNode<Label>("DebugText").Text = "🔥 powerup bought";
    }
    public void IceBought()
    {
        GetNode<Label>("DebugText").Text = "🧊 powerup bought";
    }
    public void LightningBought()
    {
        GetNode<Label>("DebugText").Text = "⚡ powerup bought";
    }
}
