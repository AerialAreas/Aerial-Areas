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
        GetNode<Label>("Money").Text = $"{GameLogic.currency}💵";
        GetNode<Button>("DebugMoreMoney").Connect(Button.SignalName.Pressed, Callable.From(MoreMoneyButton));
        GetNode<Button>("GoBack").Connect(Button.SignalName.Pressed, Callable.From(OnGoBackButton));

        GetNode<Button>("PauseMenu/MainMenuButton").Connect(Button.SignalName.Pressed, Callable.From(OnMainMenuButton));
        GetNode<Button>("PauseMenu/OptionsButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsButton));
        GetNode<Button>("PauseMenu/FormulasButton").Connect(Button.SignalName.Pressed, Callable.From(OnFormulasButton));
        GetNode<Button>("PauseMenu/Resume").Connect(Button.SignalName.Pressed, Callable.From(OnResumeButton));

        GetNode<Button>("Upgrades/BiggerBooms").Connect(Button.SignalName.Pressed, Callable.From(BiggerBoomsBought));
        GetNode<Button>("Upgrades/BiggerBooms").Text = $"🧨\nBigger Booms\n{Upgrade.upgrades["Bigger Booms"]}\nLevel: {GameLogic.upgrade_inventory["Bigger Booms"]}";
        if (GameLogic.isBiggerBoomMax)
        {
            GetNode<Button>("Upgrades/BiggerBooms").Text = "🧨\nBigger Booms\nMAX";
        }
        
        GetNode<Button>("Upgrades/Slow").Connect(Button.SignalName.Pressed, Callable.From(SlowBought));
        GetNode<Button>("Upgrades/Slow").Text = $"🐌\nSlow\n{Upgrade.upgrades["Slow"]}\nLevel: {GameLogic.upgrade_inventory["Slow"]}";
        if (GameLogic.isSlowMax)
        {
            GetNode<Button>("Upgrades/Slow").Text = "🐌\nSlow\nMAX";
        }

        GetNode<Button>("Upgrades/MaxLives").Connect(Button.SignalName.Pressed, Callable.From(MaxLivesBought));
        GetNode<Button>("Upgrades/MaxLives").Text = $"❤️\nMax Lives\n{Upgrade.upgrades["Max Lives"]}\nLevel: {GameLogic.upgrade_inventory["Max Lives"]}";
        if (GameLogic.isMaxLivesMax)
        {
            GetNode<Button>("Upgrades/MaxLives").Text = "❤️\nMax Lives\nMAX";
        }

        GetNode<Button>("Powerups/Fireball").Connect(Button.SignalName.Pressed, Callable.From(FireballBought));
        GetNode<Button>("Powerups/Fireball").Text = $"🔥\nFireball\n{Powerup.powerups["Fireball"]}\nQuantity: {GameLogic.powerup_inventory["Fireball"]}";

        GetNode<Button>("Powerups/Freeze").Connect(Button.SignalName.Pressed, Callable.From(FreezeBought));
        GetNode<Button>("Powerups/Freeze").Text = $"🧊\nFreeze\n{Powerup.powerups["Freeze"]}\nQuantity: {GameLogic.powerup_inventory["Freeze"]}";

        GetNode<Button>("Powerups/Frenzy").Connect(Button.SignalName.Pressed, Callable.From(FrenzyBought));
        GetNode<Button>("Powerups/Frenzy").Text = $"⚡\nFrenzy\n{Powerup.powerups["Frenzy"]}\nQuantity: {GameLogic.powerup_inventory["Frenzy"]}";
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
        GetNode<Button>("Upgrades/BiggerBooms").Disabled = new_paused;
        GetNode<Button>("Upgrades/Slow").Disabled = new_paused;
        GetNode<Button>("Upgrades/MaxLives").Disabled = new_paused;
        GetNode<Button>("Powerups/Fireball").Disabled = new_paused;
        GetNode<Button>("Powerups/Freeze").Disabled = new_paused;
        GetNode<Button>("Powerups/Frenzy").Disabled = new_paused;
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
        UIHelper.SwitchSceneTo(this, "Game");
    }
    public void BiggerBoomsBought()
    {
        if (CanBuy("Bigger Booms", false))
        {
            GetNode<Label>("DebugText").Text = "🧨 upgrade bought";
            Upgrade.IncreaseLevel("Bigger Booms");
            if ((GameLogic.upgrade_inventory["Bigger Booms"] - 1) <= 3)
            {
                Upgrade.upgrades["Bigger Booms"] = Upgrade.cost_per_level[GameLogic.upgrade_inventory["Bigger Booms"] - 1];
                GetNode<Button>("Upgrades/BiggerBooms").Text = $"🧨\nBigger Booms\n{Upgrade.upgrades["Bigger Booms"]}\nLevel: {GameLogic.upgrade_inventory["Bigger Booms"]}";
            }

            if (GameLogic.upgrade_inventory["Bigger Booms"] == 5)
            {
                GetNode<Button>("Upgrades/BiggerBooms").Text = "🧨\nBigger Booms\nMAX";
                GameLogic.isBiggerBoomMax = true;
            }
        }
    }
    public void SlowBought()
    {
        if (CanBuy("Slow", false))
        {
            GetNode<Label>("DebugText").Text = "🐌 upgrade bought";
            Upgrade.IncreaseLevel("Slow");
            if ((GameLogic.upgrade_inventory["Slow"] - 1) <= 3)
            {
                Upgrade.upgrades["Slow"] = Upgrade.cost_per_level[GameLogic.upgrade_inventory["Slow"] - 1];
                GetNode<Button>("Upgrades/Slow").Text = $"🐌\nSlow\n{Upgrade.upgrades["Slow"]}\nLevel: {GameLogic.upgrade_inventory["Slow"]}";
            }

            if (GameLogic.upgrade_inventory["Slow"] == 5)
            {
                GetNode<Button>("Upgrades/Slow").Text = "🐌\nSlow\nMAX";
                GameLogic.isSlowMax = true;
            }
        }
    }
    public void MaxLivesBought()
    {
        if (CanBuy("Max Lives", false))
        {
            GetNode<Label>("DebugText").Text = "❤️ upgrade bought";
            Upgrade.IncreaseLevel("Max Lives");
            if ((GameLogic.upgrade_inventory["Max Lives"] - 1) <= 3)
            {
                Upgrade.upgrades["Max Lives"] = Upgrade.cost_per_level[GameLogic.upgrade_inventory["Max Lives"] - 1];
                GetNode<Button>("Upgrades/MaxLives").Text = $"❤️\nMax Lives\n{Upgrade.upgrades["Max Lives"]}\nLevel: {GameLogic.upgrade_inventory["Max Lives"]}";
            }

            if (GameLogic.upgrade_inventory["Max Lives"] == 5)
            {
                GetNode<Button>("Upgrades/MaxLives").Text = "❤️\nMax Lives\nMAX";
                GameLogic.isMaxLivesMax = true;
            }
        }
    }
    public void FireballBought()
    {
        if (CanBuy("Fireball", true))
        {
            GetNode<Label>("DebugText").Text = "🔥 powerup bought";
            Powerup.GivePowerUp("Fireball");
            GetNode<Button>("Powerups/Fireball").Text = $"🔥\nFireball\n{Powerup.powerups["Fireball"]}\nQuantity: {GameLogic.powerup_inventory["Fireball"]}";
        }
    }
    public void FreezeBought()
    {
        if (CanBuy("Freeze", true))
        {
            GetNode<Label>("DebugText").Text = "🧊 powerup bought";
            Powerup.GivePowerUp("Freeze");
            GetNode<Button>("Powerups/Freeze").Text = $"🧊\nFreeze\n{Powerup.powerups["Freeze"]}\nQuantity: {GameLogic.powerup_inventory["Freeze"]}";
        }
    }
    public void FrenzyBought()
    {
        if (CanBuy("Frenzy", true))
        {
            GetNode<Label>("DebugText").Text = "⚡ powerup bought";
            Powerup.GivePowerUp("Frenzy");
            GetNode<Button>("Powerups/Frenzy").Text = $"⚡\nFrenzy\n{Powerup.powerups["Frenzy"]}\nQuantity: {GameLogic.powerup_inventory["Frenzy"]}";
        }
    }
    public void MoreMoneyButton()
    {
        GameLogic.currency += 5000;
        GetNode<Label>("Money").Text = $"{GameLogic.currency}💵";
    }
    private bool CanBuy(string item_name, bool isPowerUp)
    {
        if (isPowerUp)
        {
            if (GameLogic.currency >= Powerup.powerups[item_name] && GameLogic.powerup_inventory.TryGetValue(item_name, out int value) && value < 10)
            {
                GameLogic.currency -= Powerup.powerups[item_name];
                GetNode<Label>("Money").Text = $"{GameLogic.currency}💵";
                return true;
            }
            else if (GameLogic.currency < Powerup.powerups[item_name])
            {
                GetNode<Label>("DebugText").Text = "Haha You are a brokie Haha";
                return false;
            }
            else
            {
                GetNode<Label>("DebugText").Text = "Thats enough, buddy. No more greed.";
                return false;
            }
        }
        else
        {
           if (GameLogic.currency >= Upgrade.upgrades[item_name] && GameLogic.upgrade_inventory.TryGetValue(item_name, out int value) && value < 5)
            {
                GameLogic.currency -= Upgrade.upgrades[item_name];
                GetNode<Label>("Money").Text = $"{GameLogic.currency}💵";
                return true;
            }
            else if (GameLogic.currency < Upgrade.upgrades[item_name])
            {
                GetNode<Label>("DebugText").Text = "Haha You are a brokie Haha";
                return false;
            }
            else
            {
                GetNode<Label>("DebugText").Text = "Thats enough, buddy. No more greed.";
                return false;
            } 
        }
        
    }
}
