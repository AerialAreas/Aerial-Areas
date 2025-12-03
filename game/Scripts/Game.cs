using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Transactions;

public partial class Game : Node2D
{
    public int tick_count = 0; // testing variable
    public override void _Ready() // careful with this function, because the player going to options/formulas resets this scene so these get called again!
    {
        HandlePause(false); // not changing the pause, just setting defaults
        InitializeUIEvents();
        StartFirstWave();
        DrawGameObjects();
    }

    public void StartFirstWave()
    {
        if (GameLogic.first_load)
        {
            GameLogic.first_load = false;
            GameLogic.wave = new Wave(1);
            for(int i = 0; i < 10; i++)
            {
                AddEnemy();
            }
        }
    }
    public override void _Process(double delta) // should generally be called 60 times per second or whatever we set the framerate to
    {
        if (!GameLogic.isPaused)
        {
            foreach(Enemy enemy in GameLogic.wave.unspawned_enemies) // todo fix
            {
                enemy.Move();
            }
        }
    }

    public override void _Draw()
    {
        base._Draw();
    }

    public void DrawGameObjects() // maybe we make this public and GameLogic calls it? I think it needs to be here because this is the script for the actual Game scene
    {
        
    }
    public void AddEnemy()
    {
        Enemy newEnemy = new Enemy("Circle");
        Sprite2D sprite = newEnemy.sprite;
        newEnemy.SetScript(GD.Load<Script>("res://Scripts/Enemy.cs"));
        sprite.Texture = GD.Load<Texture2D>("res://Sprites/geometroid.png");
        sprite.Position = newEnemy.sprite.Position;
        sprite.Scale = new Vector2(.25f, .25f);
        GameLogic.wave.unspawned_enemies.Add(newEnemy);
        AddChild(newEnemy.sprite);
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

        GetNode<Button>("PanelContainer2/Upgrades/BiggerBooms").Text += $"\nLevel {GameLogic.upgrade_inventory["Bigger Booms"]}";

        GetNode<Button>("PanelContainer2/Upgrades/Slow").Text += $"\nLevel {GameLogic.upgrade_inventory["Slow"]}";

        GetNode<Button>("PanelContainer2/Upgrades/MaxLives").Text += $"\nLevel {GameLogic.upgrade_inventory["Max Lives"]}";

        GetNode<Button>("PanelContainer3/PowerUps/Freeze").Connect(Button.SignalName.Pressed, Callable.From(OnFreezeButton));
        GetNode<Button>("PanelContainer3/PowerUps/Freeze").Text += $"\n{GameLogic.powerup_inventory["Freeze"]}";

        GetNode<Button>("PanelContainer3/PowerUps/Fireball").Connect(Button.SignalName.Pressed, Callable.From(OnFireballButton));
        GetNode<Button>("PanelContainer3/PowerUps/Fireball").Text += $"\n{GameLogic.powerup_inventory["Fireball"]}";

        GetNode<Button>("PanelContainer3/PowerUps/Frenzy").Connect(Button.SignalName.Pressed, Callable.From(OnFrenzyButton));
        GetNode<Button>("PanelContainer3/PowerUps/Frenzy").Text += $"\n{GameLogic.powerup_inventory["Frenzy"]}";

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
        GetNode<Button>("PanelContainer3/PowerUps/Freeze").Disabled = new_paused;
        GetNode<Button>("PanelContainer3/PowerUps/Fireball").Disabled = new_paused;
        GetNode<Button>("PanelContainer3/PowerUps/Frenzy").Disabled = new_paused;
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

    public void OnFreezeButton()
    {
        if (Powerup.UsePowerup("Freeze"))
        {
            GetNode<Label>("Label").Text = "You used the 🧊 power up!";
            GetNode<Button>("PanelContainer3/PowerUps/Freeze").Text = $"🧊\n{GameLogic.powerup_inventory["Freeze"]}";
        }
        else
        {
            GetNode<Label>("Label").Text = "You don't have enough 🧊 power ups!";
        }
    }

    public void OnFireballButton()
    {
        if (Powerup.UsePowerup("Fireball"))
        {
            GetNode<Label>("Label").Text = "You used the 🔥 power up!";
            GetNode<Button>("PanelContainer3/PowerUps/Fireball").Text = $"🔥\n{GameLogic.powerup_inventory["Fireball"]}";
        }
        else
        {
            GetNode<Label>("Label").Text = "You don't have enough 🔥 power ups!";
        }
    }

    public void OnFrenzyButton()
    {
        if (Powerup.UsePowerup("Frenzy"))
        {
            GetNode<Label>("Label").Text = "You used the ⚡ power up!";
            GetNode<Button>("PanelContainer3/PowerUps/Frenzy").Text = $"⚡\n{GameLogic.powerup_inventory["Frenzy"]}";
        }
        else
        {
            GetNode<Label>("Label").Text = "You don't have enough ⚡ power ups!";
        }
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