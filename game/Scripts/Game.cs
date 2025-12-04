using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Transactions;

public partial class Game : Node2D
{
    public override void _Ready()
    {
        HandlePause(false); // not changing the pause, just setting defaults
        InitializeUIEvents();
        StartFirstWave();
    }

    public void StartFirstWave()
    {
        GameLogic.wave = new Wave(1);
        for(int i = 0; i < 15; i++)
        {
            AddEnemy();
        }
    }
    public override void _Process(double delta) // should generally be called 60 times per second or whatever we set the framerate to
    {
        if (!GameLogic.isPaused)
        {
            List<Enemy> enemies = GameLogic.wave.unspawned_enemies; // todo fix this so its spawned enemies
            // this is a nifty workaround because we don't want to remove things from the collection its iterating over
            for(int enemy_index = enemies.Count - 1; enemy_index >= 0; enemy_index--)
            {
                bool enemy_escaped = enemies[enemy_index].Move();
                if (enemy_escaped)
                {
                    RemoveEnemy(enemies[enemy_index]);
                }
            }
        }
    }

    public override void _Draw()
    {
        base._Draw();
    }
    public void AddEnemy()
    {
        int temp = new Random().Next(1, 4);
        string type = "";
        if(temp == 1)
        {
            type = "Triangle";
        }
        if(temp == 2)
        {
            type = "Rectangle";
        }
        if(temp == 3)
        {
            type = "Circle";
        }
        Enemy newEnemy = new Enemy(type);
        GameLogic.wave.unspawned_enemies.Add(newEnemy);
        AddChild(newEnemy.sprite);
        GetNode<VBoxContainer>("ProblemListPanelContainer/ProblemList").AddChild(newEnemy.problem.label);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        RemoveChild(enemy.sprite);
        RemoveChild(enemy.problem.label);
        GameLogic.wave.unspawned_enemies.Remove(enemy); // todo fix this so it removes spawned enemies
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