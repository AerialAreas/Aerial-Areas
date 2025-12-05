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
    int enemy_spawn_number;
    public override void _Ready()
    {
        HandlePause(false); // not changing the pause, just setting defaults
        InitializeUIEvents();
        StartWave();
    }

    public void StartWave()
    {
        GameLogic.wave = new Wave(GameLogic.wave_num);
        enemy_spawn_number = (GameLogic.wave_num - 1) % 3 * 2 + 10 + GameLogic.difficulty_enemy_count[GameLogic.difficulty];
        GetNode<Timer>("GameContainer/Timer").Start();
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
                    GameLogic.lives--;
                    GetNode<Label>("GameContainer/Lives").Text = $"Lives: {GameLogic.lives}/{GameLogic.max_lives}";
                    RemoveEnemy(enemies[enemy_index]);
                    CheckEnemies();
                }
            }
        }
    }

    public override void _Draw()
    {
        base._Draw();
    }
    public void AddEnemy(int wave_num)
    {
        string type = "";
        if (wave_num < 4)
        {
            type = "Rectangle";
        }
        else if (wave_num > 3 && wave_num < 7)
        {
            type = "Triangle";
        }
        else if (wave_num > 6 && wave_num < 10)
        {
            type = "Circle";
        }
        else
        {
            int temp = new Random().Next(1, 4);
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
        }
        Enemy newEnemy = new Enemy(type);
        GameLogic.wave.unspawned_enemies.Add(newEnemy);
        GetNode<Node>("GameContainer").AddChild(newEnemy.sprite);
        GetNode<VBoxContainer>("GameContainer/ProblemListPanelContainer/ProblemList").AddChild(newEnemy.problem.label);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        if (GameLogic.wave.unspawned_enemies.Contains(enemy))
        {
            GameLogic.wave.unspawned_enemies.Remove(enemy); // todo fix this so it removes spawned enemies   
        }

        if (enemy.sprite != null && enemy.sprite.IsInsideTree())
        {
            enemy.sprite.QueueFree();
        }

        if (enemy.problem != null && enemy.problem.label != null && enemy.problem.label.IsInsideTree())
        {
            enemy.problem.label.QueueFree();
        }
    }

    public void InitializeUIEvents()
    {
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballCooldown").Timeout += EnableFireball;
        GetNode<Timer>("GameContainer/Timer").WaitTime = GameLogic.difficulty_spawn_time[GameLogic.difficulty];
        GetNode<Timer>("GameContainer/Timer").Connect(Timer.SignalName.Timeout, Callable.From(EnemyCooldown));

        bool night = GameLogic.wave_num % 3 == 0;
        GetNode<Sprite2D>(night?"GameContainer/Night":"GameContainer/Day").Visible = true;
        GetNode<Sprite2D>(night?"GameContainer/Day":"GameContainer/Night").Visible = false;
        
        GetNode<Control>("Options").Visible = false;
        GetNode<Control>("Formulas").Visible = false;

        GetNode<Label>("GameContainer/Lives").Text += $" {GameLogic.lives}/{GameLogic.max_lives}";

        GetNode<Label>("GameContainer/Money").Text = $"{GameLogic.currency}💵";
        GetNode<Button>("GameContainer/VBoxContainer/WinButton").Connect(Button.SignalName.Pressed, Callable.From(OnWinButton));
        GetNode<Button>("GameContainer/VBoxContainer/GameOverButton").Connect(Button.SignalName.Pressed, Callable.From(OnGameOverButton));
        GetNode<Button>("GameContainer/VBoxContainer/ScoreButton").Connect(Button.SignalName.Pressed, Callable.From(OnScoreButton));

        GetNode<Button>("GameContainer/PauseMenu/MainMenuButton").Connect(Button.SignalName.Pressed, Callable.From(OnMainMenuButton));
        GetNode<Button>("GameContainer/PauseMenu/OptionsButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsButton));
        GetNode<Button>("GameContainer/PauseMenu/FormulasButton").Connect(Button.SignalName.Pressed, Callable.From(OnFormulasButton));
        GetNode<Button>("GameContainer/PauseMenu/Resume").Connect(Button.SignalName.Pressed, Callable.From(OnResumeButton));

        GetNode<Button>("GameContainer/PanelContainer2/Upgrades/BiggerBooms").Text += $"\nLevel {GameLogic.upgrade_inventory["Bigger Booms"]}";

        GetNode<Button>("GameContainer/PanelContainer2/Upgrades/Slow").Text += $"\nLevel {GameLogic.upgrade_inventory["Slow"]}";

        GetNode<Button>("GameContainer/PanelContainer2/Upgrades/MaxLives").Text += $"\nLevel {GameLogic.upgrade_inventory["Max Lives"]}";

        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Connect(Button.SignalName.Pressed, Callable.From(OnFreezeButton));
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Text += $"\n{GameLogic.powerup_inventory["Freeze"]}";

        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Connect(Button.SignalName.Pressed, Callable.From(OnFireballButton));
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Text += $"\n{GameLogic.powerup_inventory["Fireball"]}";

        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Connect(Button.SignalName.Pressed, Callable.From(OnFrenzyButton));
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Text += $"\n{GameLogic.powerup_inventory["Frenzy"]}";

        GetNode<LineEdit>("GameContainer/PanelContainer4/Answer").TextSubmitted += CheckAnswer;
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
        GetNode<VBoxContainer>("GameContainer/PauseMenu").Visible = new_paused;
        GetNode<LineEdit>("GameContainer/PanelContainer4/Answer").Editable = !new_paused;
        GetNode<Timer>("GameContainer/Timer").Paused = new_paused;
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballCooldown").Paused = new_paused;
    }

    public void EnemyCooldown()
    {
        if (enemy_spawn_number != 0 && GameLogic.wave.unspawned_enemies.Count < 15)
        {
            AddEnemy(GameLogic.wave_num);
            enemy_spawn_number--;
        }
    }

    public void OnWinButton()
    {
        UIHelper.SwitchSceneTo(this, "Win");
    }

    public void OnGameOverButton()
    {
        UIHelper.SwitchSceneTo(this, "Game Over");
    }

    public void OnScoreButton()
    {
        GameLogic.score += 5000;
        GD.Print($"Score: {GameLogic.score}");
    }

    public void OnMainMenuButton()
    {
        UIHelper.SwitchSceneTo(this, "Main Menu");
        GameLogic.isPaused = false;
    }

    public void OnOptionsButton()
    {
        GetNode<Control>("GameContainer").Visible = false;
        GetNode<Control>("Options").Visible = true;
        GetNode<Control>("Formulas").Visible = false;
        GetNode<VBoxContainer>("GameContainer/PauseMenu").Visible = false;
    }

    public void OnFormulasButton()
    {
        GetNode<Control>("GameContainer").Visible = false;
        GetNode<Control>("Options").Visible = false;
        GetNode<Control>("Formulas").Visible = true;
        GetNode<VBoxContainer>("GameContainer/PauseMenu").Visible = false;
    }

    public void ShowGame()
    {
        GetNode<Control>("GameContainer").Visible = true;
        GetNode<Control>("Options").Visible = false;
        GetNode<Control>("Formulas").Visible = false;
        GetNode<VBoxContainer>("GameContainer/PauseMenu").Visible = true;
    }

    public void OnResumeButton()
    {
        HandlePause(true);
    }

    public void OnFreezeButton()
    {
        if (!GameLogic.isPaused)
        {
            if (Powerup.UsePowerup("Freeze"))
            {
                GetNode<Label>("GameContainer/Label").Text = "You used the 🧊 power up!";
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Text = $"🧊\n{GameLogic.powerup_inventory["Freeze"]}";
            }
            else
            {
                GetNode<Label>("GameContainer/Label").Text = "You don't have enough 🧊 power ups!";
            }
        }
    }

    public void OnFireballButton()
    {
        if (!GameLogic.isPaused)
        {
            if (Powerup.UsePowerup("Fireball"))
            {
                Timer cooldown = GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballCooldown");
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Disabled = true;
                cooldown.Start();
                GetNode<Label>("GameContainer/Label").Text = "You used the 🔥 power up!";
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Text = $"🔥\n{GameLogic.powerup_inventory["Fireball"]}";
            }
            else
            {
                GetNode<Label>("GameContainer/Label").Text = "You don't have enough 🔥 power ups!";
            }
        }
    }

    public void OnFrenzyButton()
    {
        if (!GameLogic.isPaused)
        {
            if (Powerup.UsePowerup("Frenzy"))
            {
                GetNode<Label>("GameContainer/Label").Text = "You used the ⚡ power up!";
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Text = $"⚡\n{GameLogic.powerup_inventory["Frenzy"]}";
            }
            else
            {
                GetNode<Label>("GameContainer/Label").Text = "You don't have enough ⚡ power ups!";
            }
        }
    }

    public void CheckAnswer(string answer)
    {
        LineEdit a = GetNode<LineEdit>("GameContainer/PanelContainer4/Answer");
        if (answer.Trim() == "30")
        {
            GetNode<Label>("GameContainer/DebugEnemy").Text = "Oh no you defeated me...";
        }
        a.Clear();
    }

    private void CheckEnemies()
    {
        if (GameLogic.lives <= 0)
        {
            UIHelper.SwitchSceneTo(this, "Game Over");
        }
        if (GameLogic.wave.unspawned_enemies.Count == 0 && enemy_spawn_number == 0)
        {
            if (GameLogic.wave_num == 12)
            {
                UIHelper.SwitchSceneTo(this, "Win");
            }
            else
            {
                UIHelper.SwitchSceneTo(this, "Shop");  
            }
        }
    }

    private void EnableFireball()
    {
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Disabled = false;
    }
}