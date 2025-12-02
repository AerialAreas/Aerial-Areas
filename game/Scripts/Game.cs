using Godot;
using System;
using System.ComponentModel;

public partial class Game : Node
{
    public int tick_count = 0; // testing variable
    public override void _Ready() // careful with this function, because the player going to options/formulas resets this scene so these get called again!
    {
        HandlePause(false); // not changing the pause, just setting defaults
        InitializeUIEvents();
    }
    public override void _Process(double delta) // should generally be called 60 times per second or whatever we set the framerate to
    {
        if (!GameLogic.isPaused)
        {
            tick_count++;
            GameLogic.HandleTick();
            DrawGameObjects();
        }
    }

    private void DrawGameObjects() // maybe we make this public and GameLogic calls it? I think it needs to be here because this is the script for the actual Game scene
    {

    }

    public void InitializeUIEvents()
    {
        GetNode<Label>("Money").Text = $"{GameLogic.currency}💵";
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
        GetNode<LineEdit>("PanelContainer4/Answer").TextSubmitted += CheckAnswer;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
                HandlePause(true); // changing the bool of pause
            }
        }
    }
    public void HandlePause(bool pause_changed) // if pause_changed is true, it enables/disables the ui elements. if its false, it just sets them to what they should be based on isPaused
    {
        bool new_paused;
        if (pause_changed)
        {
            new_paused = !GameLogic.isPaused;
        }
        else
        {
            new_paused = GameLogic.isPaused;
        }
        GameLogic.isPaused = new_paused;
        GetNode<VBoxContainer>("PauseMenu").Visible = new_paused;
        GetNode<Button>("VBoxContainer/ShopButton").Disabled = new_paused;
        GetNode<Button>("VBoxContainer/WinButton").Disabled = new_paused;
        GetNode<Button>("VBoxContainer/GameOverButton").Disabled = new_paused;
        GetNode<Button>("PanelContainer3/PowerUps/Ice").Disabled = new_paused;
        GetNode<Button>("PanelContainer3/PowerUps/Fire").Disabled = new_paused;
        GetNode<Button>("PanelContainer3/PowerUps/Lightning").Disabled = new_paused;
        GetNode<LineEdit>("PanelContainer4/Answer").Editable = !new_paused;
    }

    public void OnShopButton()
    {
        UIHelper.SwitchSceneTo(this, "Shop");
    }

    public void OnWinButton()
    {
        UIHelper.SwitchSceneTo(this, "Win");
    }

    public void OnGameOverButton()
    {
        UIHelper.SwitchSceneTo(this, "Game Over");
    }

    public void OnMainMenuButton()
    {
        UIHelper.SwitchSceneTo(this, "Main Menu");
        GameLogic.isPaused = false;
    }

    public void OnOptionsButton()
    {
        UIHelper.SwitchSceneTo(this, "Options");
    }

    public void OnFormulasButton()
    {
        UIHelper.SwitchSceneTo(this, "Formulas");
    }

    public void OnResumeButton()
    {
        HandlePause(true);
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

    public void CheckAnswer(string answer)
    {
        LineEdit a = GetNode<LineEdit>("PanelContainer4/Answer");
        if (answer.Trim() == "30")
        {
            GetNode<Label>("DebugEnemy").Text = "Oh no you defeated me...";
        }
        a.Clear();
    }
}
