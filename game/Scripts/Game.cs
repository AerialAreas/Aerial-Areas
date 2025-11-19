using Godot;
using System;

public partial class Game : Node
{
	public override void _Ready()
    {
        GameLogic.inGame = true;
        if (GameLogic.isPaused)
        {
            GetNode<VBoxContainer>("PauseMenu").Visible = true;
            GetNode<Button>("VBoxContainer/ShopButton").Disabled = true;
            GetNode<Button>("VBoxContainer/WinButton").Disabled = true;
            GetNode<Button>("VBoxContainer/GameOverButton").Disabled = true;
            GetNode<Button>("PanelContainer3/PowerUps/Ice").Disabled = true;
            GetNode<Button>("PanelContainer3/PowerUps/Fire").Disabled = true;
            GetNode<Button>("PanelContainer3/PowerUps/Lightning").Disabled = true;
            GetNode<TextEdit>("PanelContainer4/TextEdit").Editable = false;
        }
		GetNode<Label>("GameAttributes").Text = $"Player Name: {GameLogic.player_name}\nGame Difficulty: {GameLogic.difficulty}";
        GetNode<Button>("VBoxContainer/ShopButton").Connect(Button.SignalName.Pressed, Callable.From(OnShopButton));
		GetNode<Button>("VBoxContainer/WinButton").Connect(Button.SignalName.Pressed, Callable.From(OnWinButton));
		GetNode<Button>("VBoxContainer/GameOverButton").Connect(Button.SignalName.Pressed, Callable.From(OnGameOverButton));
        GetNode<Button>("PauseMenu/MainMenuButton").Connect(Button.SignalName.Pressed, Callable.From(OnMainMenuButton));
		GetNode<Button>("PauseMenu/OptionsButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsButton));
		GetNode<Button>("PauseMenu/FormulasButton").Connect(Button.SignalName.Pressed, Callable.From(OnFormulasButton));
		GetNode<Button>("PauseMenu/Resume").Connect(Button.SignalName.Pressed, Callable.From(OnResumeButton));
        GetNode<Button>("PanelContainer3/PowerUps/Ice").Connect(Button.SignalName.Pressed, Callable.From(OnIceButton));
        GetNode<Button>("PanelContainer3/PowerUps/Fire").Connect(Button.SignalName.Pressed, Callable.From(OnFireButton));
        GetNode<Button>("PanelContainer3/PowerUps/Lightning").Connect(Button.SignalName.Pressed, Callable.From(OnLightningButton));
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
                GetNode<Button>("VBoxContainer/ShopButton").Disabled = !GetNode<Button>("VBoxContainer/ShopButton").Disabled;
                GetNode<Button>("VBoxContainer/WinButton").Disabled = !GetNode<Button>("VBoxContainer/WinButton").Disabled;
                GetNode<Button>("VBoxContainer/GameOverButton").Disabled = !GetNode<Button>("VBoxContainer/GameOverButton").Disabled;
                GetNode<Button>("PanelContainer3/PowerUps/Ice").Disabled = !GetNode<Button>("PanelContainer3/PowerUps/Ice").Disabled;
                GetNode<Button>("PanelContainer3/PowerUps/Fire").Disabled = !GetNode<Button>("PanelContainer3/PowerUps/Fire").Disabled;
                GetNode<Button>("PanelContainer3/PowerUps/Lightning").Disabled = !GetNode<Button>("PanelContainer3/PowerUps/Lightning").Disabled;
                GetNode<TextEdit>("PanelContainer4/TextEdit").Editable = !GetNode<TextEdit>("PanelContainer4/TextEdit").Editable;
            }
        }
    }

	public void OnShopButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Shop.tscn");
    }

	public void OnWinButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Win.tscn");
    }

	public void OnGameOverButton()
    {
        GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
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
        GetNode<Button>("VBoxContainer/ShopButton").Disabled = false;
        GetNode<Button>("VBoxContainer/WinButton").Disabled = false;
        GetNode<Button>("VBoxContainer/GameOverButton").Disabled = false;
        GetNode<Button>("PanelContainer3/PowerUps/Ice").Disabled = false;
        GetNode<Button>("PanelContainer3/PowerUps/Fire").Disabled = false;
        GetNode<Button>("PanelContainer3/PowerUps/Lightning").Disabled = false;
        GetNode<TextEdit>("PanelContainer4/TextEdit").Editable = true;
    }

    public void OnIceButton()
    {
        GetNode<Label>("Label").Text = "You used the 🧊 power up!";
    }

    public void OnFireButton()
    {
        GetNode<Label>("Label").Text = "You used the 🔥 power up!";
    }

    public void OnLightningButton()
    {
        GetNode<Label>("Label").Text = "You used the ⚡ power up!";
    }
}
