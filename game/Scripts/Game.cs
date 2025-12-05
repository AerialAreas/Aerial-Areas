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

    public override void _Input(InputEvent @event)
    {
        Wave curr_wave = GameLogic.wave;
        Enemy found_enemy = new Enemy();

        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
            {
                bool enemy_found = false;
                foreach (Enemy wave_enemy in curr_wave.unspawned_enemies)
                {
                    float scale_x = wave_enemy.sprite.Texture.GetSize().X * wave_enemy.sprite.Scale.X / 2;
                    float scale_y = wave_enemy.sprite.Texture.GetSize().Y * wave_enemy.sprite.Scale.Y / 2;
                    float enemy_x = wave_enemy.sprite.Position.X - scale_x;
                    float enemy_y = wave_enemy.sprite.Position.Y - scale_y;
                    float mouse_x = mouseButtonEvent.Position.X;
                    float mouse_y = mouseButtonEvent.Position.Y;
                    float outer_x = wave_enemy.sprite.Position.X + scale_x;
                    float outer_y = wave_enemy.sprite.Position.Y + scale_y;
                    if (enemy_x < mouse_x && outer_x > mouse_x && enemy_y < mouse_y && outer_y > mouse_y && !enemy_found)
                    {
                        enemy_found = true;
                        found_enemy = wave_enemy;
                    }
                }

                foreach (Enemy wave_enemy in curr_wave.unspawned_enemies)
                {
                    if (enemy_found)
                    {
                        if (wave_enemy == found_enemy)
                        {
                            wave_enemy.isHighlighted = true;
                            wave_enemy.problem.UpdateLabel(true);
                        } 
                        else
                        {
                            wave_enemy.isHighlighted = false;
                            wave_enemy.problem.UpdateLabel(false);
                        }
                    }
                }
            }
        }
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
        bool night = GameLogic.wave_num % 3 == 0;
        GetNode<Sprite2D>(night?"GameContainer/Night":"GameContainer/Day").Visible = true;
        GetNode<Sprite2D>(night?"GameContainer/Day":"GameContainer/Night").Visible = false;
        
        GetNode<Control>("Options").Visible = false;
        GetNode<Control>("Formulas").Visible = false;

        GetNode<Label>("GameContainer/Money").Text = $"{GameLogic.currency}💵";
        GetNode<Label>("GameContainer/GameAttributes").Text = $"Player Name: {GameLogic.player_name}\nGame Difficulty: {GameLogic.difficulty}";
        GetNode<Button>("GameContainer/VBoxContainer/ShopButton").Connect(Button.SignalName.Pressed, Callable.From(OnShopButton));
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
        GetNode<Button>("GameContainer/VBoxContainer/ShopButton").Disabled = new_paused;
        GetNode<Button>("GameContainer/VBoxContainer/WinButton").Disabled = new_paused;
        GetNode<Button>("GameContainer/VBoxContainer/GameOverButton").Disabled = new_paused;
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Disabled = new_paused;
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Disabled = new_paused;
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Disabled = new_paused;
        GetNode<LineEdit>("GameContainer/PanelContainer4/Answer").Editable = !new_paused;
    }

    public void OnShopButton()
    {
        UIHelper.SwitchSceneTo(this, "Shop");
        GameLogic.first_load = true;
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

    public void OnFireballButton()
    {
        if (Powerup.UsePowerup("Fireball"))
        {
            GetNode<Label>("GameContainer/Label").Text = "You used the 🔥 power up!";
            GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Text = $"🔥\n{GameLogic.powerup_inventory["Fireball"]}";
        }
        else
        {
            GetNode<Label>("GameContainer/Label").Text = "You don't have enough 🔥 power ups!";
        }
    }

    public void OnFrenzyButton()
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

    public void CheckAnswer(string answer)
    {
        LineEdit a = GetNode<LineEdit>("GameContainer/PanelContainer4/Answer");
        Wave curr_wave = GameLogic.wave;
        Enemy selected_enemy = new Enemy();
        bool enemy_found = false;
        foreach (Enemy wave_enemy in curr_wave.unspawned_enemies)
        {
            if (wave_enemy.isHighlighted)
            {
                selected_enemy = wave_enemy;
                enemy_found = true;
                break;
            }
        }

        if (!enemy_found)
        {
            a.Clear();
            return;
        }

        GD.Print(answer.Trim());
        GD.Print(selected_enemy.problem.solution);
        if (answer.Trim() == selected_enemy.problem.solution)
        {   
            
            curr_wave.HandleExplosion(selected_enemy);
            GD.Print("correct");
        }
        else
        {
            GD.Print("wrong");
        }
        a.Clear();
    }
}