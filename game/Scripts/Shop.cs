using Godot;
using System;

public partial class Shop : Node2D
{
    public override void _Ready()
    {
        InitializeUIEvents();
        HandlePause(false);
    }

    public void InitializeUIEvents()
    {
        GetNode<Label>("Money").Text = $"{GameLogic.gold}💵";
        GetNode<Button>("DebugMoreMoney").Connect(Button.SignalName.Pressed, Callable.From(MoreMoneyButton));
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
                HandlePause(true);
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
        GetNode<Button>("GoBack").Disabled = new_paused;
        GetNode<Button>("Upgrades/Firecracker").Disabled = new_paused;
        GetNode<Button>("Upgrades/Diamond").Disabled = new_paused;
        GetNode<Button>("Upgrades/Heart").Disabled = new_paused;
        GetNode<Button>("Powerups/Fire").Disabled = new_paused;
        GetNode<Button>("Powerups/Ice").Disabled = new_paused;
        GetNode<Button>("Powerups/Lightning").Disabled = new_paused;
    }

    public void OnMainMenuButton()
    {
        UIHelper.SwitchSceneTo(this, "Main Menu");
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

    public void OnGoBackButton()
    {
        UIHelper.SwitchSceneTo(this, UIHelper.previous_scene);
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

    public void MoreMoneyButton()
    {
        GameLogic.gold += 5000;
        GetNode<Label>("Money").Text = $"{GameLogic.gold}💵";
    }
}
