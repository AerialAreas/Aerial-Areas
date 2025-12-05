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
    private int enemy_spawn_number;
    public override void _Ready()
    {
        HandlePause(false); // not changing the pause, just setting defaults
        InitializeUIEvents();
        StartWave();
    }

    public override void _Input(InputEvent @event)
    {
        Wave curr_wave = GameLogic.wave;
        Enemy found_enemy = new Enemy();
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed && !GameLogic.isPaused)
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
                            GetNode<Sprite2D>("GameContainer/Target").Position = wave_enemy.sprite.Position;
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
        else if (@event is InputEventKey eventKey)
        {
            if (GetNode<Control>("Options").Visible || GetNode<Control>("Formulas").Visible)
            {
                return;
            }
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
                HandlePause(true); // changing the bool of pause
            }
            if(eventKey.Pressed && eventKey.Keycode == Key.Up && !GameLogic.isPaused)
            {
                ChangeHighlightedEnemy("Up");
            }
            if(eventKey.Pressed && eventKey.Keycode == Key.Down && !GameLogic.isPaused)
            {
                ChangeHighlightedEnemy("Down");
            }
            if(eventKey.Pressed && eventKey.Keycode == Key.Shift)
            {
                OnFreezeButton();
            }
            if(eventKey.Pressed && eventKey.Keycode == Key.Ctrl)
            {
                OnFireballButton();
            }
            if(eventKey.Pressed && eventKey.Keycode == Key.Alt)
            {
                OnFrenzyButton();
            }
        }
    }
    public void ChangeHighlightedEnemy(string direction)
    {
        if(GameLogic.wave.unspawned_enemies.Count <= 1)
        {
            return;
        }
        int old_highlighted_index = -1;
        int highlighted_index = -1;
        for(int enemy_index = GameLogic.wave.unspawned_enemies.Count - 1; enemy_index >= 0; enemy_index--)
        {
            Enemy selected = GameLogic.wave.unspawned_enemies[enemy_index];
            if (selected.isHighlighted)
            {
                highlighted_index = enemy_index;
                old_highlighted_index = enemy_index;
            }
        }
        if(highlighted_index == -1)
        {
            GD.Print("Highlighted enemy is not found on arrow press");
            return;
        }
        if(direction == "Up")
        {
            highlighted_index--;
        }
        else
        {
            highlighted_index++;
        }
        if(highlighted_index < 0)
        {
            highlighted_index = GameLogic.wave.unspawned_enemies.Count - 1;
        }
        if(highlighted_index >= GameLogic.wave.unspawned_enemies.Count)
        {
            highlighted_index = 0;
        }
        GameLogic.wave.unspawned_enemies[highlighted_index].isHighlighted = true;
        GetNode<Sprite2D>("GameContainer/Target").Position = GameLogic.wave.unspawned_enemies[highlighted_index].sprite.Position;
        GameLogic.wave.unspawned_enemies[highlighted_index].problem.UpdateLabel(true);

        GameLogic.wave.unspawned_enemies[old_highlighted_index].isHighlighted = false;
        GameLogic.wave.unspawned_enemies[old_highlighted_index].problem.UpdateLabel(false);
    }

    public void StartWave()
    {
        GameLogic.wave = new Wave(GameLogic.wave_num);
        enemy_spawn_number = (GameLogic.wave_num - 1) % 3 * 2 + 10 + GameLogic.difficulty_enemy_count[GameLogic.difficulty];
        GetNode<Timer>("GameContainer/Timer").Start();
        GameLogic.isFreeze = false;
        GameLogic.isFrenzy = false;
        GameLogic.sceneSwitch = false;
    }
    public override void _Process(double delta) // should generally be called 60 times per second or whatever we set the framerate to
    {
        if (!GameLogic.isPaused)
        {
            HighlightEnemyIfNoneHighlighted();
            HandleMoveAndEscapes();
        }
    }
    public void HighlightEnemyIfNoneHighlighted()
    {
        Enemy lowest_enemy = new Enemy();
        float lowest_height = -1337f;
        if(GameLogic.wave.unspawned_enemies.Count == 0)
        {
            GetNode<Sprite2D>("GameContainer/Target").Position = new Vector2(-250,-250);
            return;
        }
        foreach(Enemy enemy in GameLogic.wave.unspawned_enemies)
        {
            if (enemy.isHighlighted)
            {
                GetNode<Sprite2D>("GameContainer/Target").Position = enemy.sprite.Position;
                return;
            }
            else
            {
                if(enemy.sprite.Position.Y > lowest_height)
                {
                    lowest_enemy = enemy;
                    lowest_height = enemy.sprite.Position.Y;
                }
            }
        }
        lowest_enemy.isHighlighted = true;
        GetNode<Sprite2D>("GameContainer/Target").Position = lowest_enemy.sprite.Position;
        lowest_enemy.problem.UpdateLabel(true);
    }

    public void HandleMoveAndEscapes()
    {
        List<Enemy> enemies = GameLogic.wave.unspawned_enemies; // todo fix this so its spawned enemies
        // this is a nifty workaround because we don't want to remove things from the collection its iterating over
        for(int enemy_index = enemies.Count - 1; enemy_index >= 0; enemy_index--)
        {
            bool enemy_escaped = enemies[enemy_index].Move();
            if (enemy_escaped)
            {
                GameLogic.lives--;
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/takeDamage.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Play();
                }
                GetNode<Label>("GameContainer/Lives").Text = $"❤️:{GameLogic.lives}/{GameLogic.max_lives}";
                RemoveEnemy(enemies[enemy_index]);
                CheckEnemies();
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
        newEnemy.problem.label.AddChild(newEnemy.problem.clickable);
        GetNode<VBoxContainer>("GameContainer/ProblemListPanelContainer/ProblemList").AddChild(newEnemy.problem.label);
        newEnemy.problem.clickable.Pressed += () => HighlightEnemy(newEnemy);
    }
    public void HighlightEnemy(Enemy enemy)
    {
        if (!GameLogic.isPaused)
        {
            foreach(Enemy e in GameLogic.wave.unspawned_enemies)
            {
                if (e.isHighlighted)
                {
                    e.isHighlighted = false;
                    e.problem.UpdateLabel(false);
                }
            }
            enemy.isHighlighted = true;
            GetNode<Sprite2D>("GameContainer/Target").Position = enemy.sprite.Position;
            enemy.problem.UpdateLabel(true);
        }
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        if (GameLogic.wave.unspawned_enemies.Contains(enemy))
        {
            GameLogic.wave.unspawned_enemies.Remove(enemy);  
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
        GetNode<Timer>("GameContainer/Explosion/Timer").Timeout += ExplosionDisappears;
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeTimer").Timeout += EnableFreeze;
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballCooldown").Timeout += EnableFireball;
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzyTimer").Timeout += EnableFrenzy;
        GetNode<Timer>("GameContainer/Timer").WaitTime = GameLogic.difficulty_spawn_time[GameLogic.difficulty];
        GetNode<AudioStreamPlayer>("GameContainer/BGMusic").Finished += ReplayBGMusic;
        GetNode<Timer>("GameContainer/Timer").Connect(Timer.SignalName.Timeout, Callable.From(EnemyCooldown));

        bool night = GameLogic.wave_num % 3 == 0;
        GetNode<Sprite2D>(night?"GameContainer/Night":"GameContainer/Day").Visible = true;
        GetNode<Sprite2D>(night?"GameContainer/Day":"GameContainer/Night").Visible = false;
        
        GetNode<Control>("Options").Visible = false;
        GetNode<Control>("Formulas").Visible = false;

        GetNode<Label>("GameContainer/Lives").Text += $"{GameLogic.lives}/{GameLogic.max_lives}";

        GetNode<Label>("GameContainer/Money").Text = $"💵:{GameLogic.currency}";
        GetNode<Button>("GameContainer/VBoxContainer/WinButton").Connect(Button.SignalName.Pressed, Callable.From(OnWinButton));
        GetNode<Button>("GameContainer/VBoxContainer/GameOverButton").Connect(Button.SignalName.Pressed, Callable.From(OnGameOverButton));
        GetNode<Button>("GameContainer/VBoxContainer/ScoreButton").Connect(Button.SignalName.Pressed, Callable.From(OnScoreButton));
        GetNode<Button>("GameContainer/VBoxContainer/ShopButton").Connect(Button.SignalName.Pressed, Callable.From(OnShopButton));

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
        if (!UIHelper.music)
        {
            GetNode<AudioStreamPlayer>("GameContainer/BGMusic").Stop();
        }
        else
        {
            GetNode<AudioStreamPlayer>("GameContainer/BGMusic").VolumeDb = -20.0f + UIHelper.volume/5.0f;
            if (UIHelper.volume == 0)
            {
                GetNode<AudioStreamPlayer>("GameContainer/BGMusic").VolumeDb = -80.0f;
            }
        }
        GetNode<LineEdit>("GameContainer/PanelContainer4/Answer").GrabClickFocus();
        GetNode<LineEdit>("GameContainer/PanelContainer4/Answer").GrabFocus();
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
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeTimer").Paused = new_paused;
        GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeSFX").StreamPaused = new_paused;
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballCooldown").Paused = new_paused;
        GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballSFX").StreamPaused = new_paused;
        GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzyTimer").Paused = new_paused;
        GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzySFX").StreamPaused = new_paused;
        GetNode<AudioStreamPlayer>("GameContainer/GameSFX").StreamPaused = new_paused;
        GetNode<AudioStreamPlayer>("GameContainer/BGMusic").StreamPaused = new_paused;
        GetNode<AudioStreamPlayer>("GameContainer/BGMusic").VolumeDb = -20.0f + UIHelper.volume/5.0f;
            if (UIHelper.volume == 0)
            {
                GetNode<AudioStreamPlayer>("GameContainer/BGMusic").VolumeDb = -80.0f;
            }
        if (!UIHelper.music)
        {
            GetNode<AudioStreamPlayer>("GameContainer/BGMusic").Stop();
        }
        else if(!GetNode<AudioStreamPlayer>("GameContainer/BGMusic").Playing && !new_paused)
        {
            GetNode<AudioStreamPlayer>("GameContainer/BGMusic").Play();
        }
        if (!UIHelper.sfx)
        {
            GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeSFX").Stop();
            GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballSFX").Stop();
            GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzySFX").Stop();
            GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Stop();
        }
    }

    public void ExplosionDisappears()
    {
        GetNode<Sprite2D>("GameContainer/Explosion").Position = new Vector2(-300f, -300f); // off screen
    }
    public void EnemyCooldown()
    {
        if (enemy_spawn_number != 0 && GameLogic.wave.unspawned_enemies.Count < 15 && !GameLogic.isFreeze)
        {
            AddEnemy(GameLogic.wave_num);
            enemy_spawn_number--;
        }
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
                Timer freeze = GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeTimer");
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Disabled = true;
                freeze.Start();
                GameLogic.isFreeze = true;
                GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Freeze/FreezeSFX").Play();
                }
                GetNode<Label>("GameContainer/Label").Text = "You used the 🧊 power up!";
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Text = $"🧊\n{GameLogic.powerup_inventory["Freeze"]}";
            }
            else
            {
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Play();
                }
                GetNode<Label>("GameContainer/Label").Text = "You don't have enough 🧊 power ups!";
            }
        }
    }

    public void OnFireballButton()
    {
        if (!GameLogic.isPaused)
        {
            Enemy selected_enemy = new Enemy();
            bool enemy_found = false;
            foreach (Enemy wave_enemy in GameLogic.wave.unspawned_enemies)
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
                return;
            }
            if (Powerup.UsePowerup("Fireball"))
            {
                Timer cooldown = GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballCooldown");
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Disabled = true;
                cooldown.Start();
                GiveMoney(selected_enemy);
                GiveScore(selected_enemy);
                RemoveEnemy(selected_enemy);
                GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Fireball/FireballSFX").Play();
                }
                GetNode<Label>("GameContainer/Label").Text = "You used the 🔥 power up!";
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Text = $"🔥\n{GameLogic.powerup_inventory["Fireball"]}";
                GetNode<Tooltip>("GameContainer/PanelContainer3/PowerUps/Fireball/Tooltip").Toggle(false);
                CheckEnemies();
            }
            else
            {
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Play();
                }
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
                Timer frenzy = GetNode<Timer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzyTimer");
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Disabled = true;
                frenzy.Start();
                GameLogic.isFrenzy = true;
                GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzySFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzySFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/PanelContainer3/PowerUps/Frenzy/FrenzySFX").Play();
                }
                GetNode<Label>("GameContainer/Label").Text = "You used the ⚡ power up!";
                GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Text = $"⚡\n{GameLogic.powerup_inventory["Frenzy"]}";
            }
            else
            {
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Play();
                }
                GetNode<Label>("GameContainer/Label").Text = "You don't have enough ⚡ power ups!";
            }
        }
    }

    public void CheckAnswer(string answer)
    {
        LineEdit a = GetNode<LineEdit>("GameContainer/PanelContainer4/Answer");
        Enemy selected_enemy = new Enemy();
        bool enemy_found = false;
        foreach (Enemy wave_enemy in GameLogic.wave.unspawned_enemies)
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
            
            HandleExplosion(selected_enemy);
            GD.Print("correct");
        }
        else
        {
            GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
            GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
            if (UIHelper.volume == 0)
            {
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").VolumeDb = -80.0f;
            }
            if (UIHelper.sfx) {
                GetNode<AudioStreamPlayer>("GameContainer/GameSFX").Play();
            }
            GD.Print("wrong");
        }
        a.Clear();
    }

    private void CheckEnemies()
    {
        if (GameLogic.lives <= 0)
        {
            GameLogic.sceneSwitch = true;
            UIHelper.SwitchSceneTo(this, "Game Over");
        }
        if (GameLogic.wave.unspawned_enemies.Count == 0 && enemy_spawn_number == 0)
        {
            if (GameLogic.wave_num == 12)
            {
                GameLogic.sceneSwitch = true;
                UIHelper.SwitchSceneTo(this, "Win");
            }
            else
            {
                GameLogic.sceneSwitch = true;
                UIHelper.SwitchSceneTo(this, "Shop");  
            }
        }
    }

    private void EnableFreeze()
    {
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Freeze").Disabled = false;
        GameLogic.isFreeze = false;
    }
    private void EnableFireball()
    {
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Fireball").Disabled = false;
    }

    private void EnableFrenzy()
    {
        GetNode<Button>("GameContainer/PanelContainer3/PowerUps/Frenzy").Disabled = false;
        GameLogic.isFrenzy = false;
    }

    private void ReplayBGMusic()
    {
        GetNode<AudioStreamPlayer>("GameContainer/BGMusic").Play();
    }
    public void HandleExplosion(Enemy enemy)
    {
        GiveMoney(enemy);
        RemoveEnemy(enemy);
        GiveScore(enemy);
        CheckEnemies();
        Explosion explosion = new Explosion(enemy.sprite.Position);
        Sprite2D explosion_sprite = GetNode<Sprite2D>("GameContainer/Explosion");
        GetNode<Timer>("GameContainer/Explosion/Timer").Start();
        explosion_sprite.Position = enemy.sprite.Position;
        int bigger_booms_level = GameLogic.upgrade_inventory["Bigger Booms"];
        explosion_sprite.Scale = new Vector2(bigger_booms_level * 0.1f, bigger_booms_level * 0.1f);

        // GetNode<Node>("GameContainer").AddChild(explosion);
        float ex_scale_x = explosion_sprite.Texture.GetSize().X * explosion_sprite.Scale.X / 2;
        float ex_scale_y = explosion_sprite.Texture.GetSize().Y * explosion_sprite.Scale.Y / 2;
        float ex_inner_x = explosion_sprite.Position.X - ex_scale_x;
        float ex_inner_y = explosion_sprite.Position.Y - ex_scale_y;
        float ex_outer_x = explosion_sprite.Position.X + ex_scale_x;
        float ex_outer_y = explosion_sprite.Position.Y + ex_scale_y;
        for(int enemy_index = GameLogic.wave.unspawned_enemies.Count - 1; enemy_index >= 0; enemy_index--)
        {
            Enemy e = GameLogic.wave.unspawned_enemies[enemy_index];
            float scale_x = e.sprite.Texture.GetSize().X * e.sprite.Scale.X / 2;
            float scale_y = e.sprite.Texture.GetSize().Y * e.sprite.Scale.Y / 2;
            float inner_x = e.sprite.Position.X - scale_x;
            float inner_y = e.sprite.Position.Y - scale_y;
            float outer_x = e.sprite.Position.X + scale_x;
            float outer_y = e.sprite.Position.Y + scale_y;
            if (outer_x > ex_inner_x && inner_x < ex_outer_x && outer_y > ex_inner_y && inner_y < ex_outer_y)
            {
                GiveMoney(e);
                RemoveEnemy(e);
                GiveScore(e);
                CheckEnemies();
            }
         }
    }
    public void GiveMoney(Enemy enemy)
    {
        GameLogic.currency += enemy.value;
        GetNode<Label>("GameContainer/Money").Text = GameLogic.currency.ToString();
    }
    public void GiveScore(Enemy enemy)
    {
        int score = (int)(enemy.score * (2 - (enemy.sprite.Position.Y/GameLogic.ENEMY_ESCAPE_BOUND)));
        if (GameLogic.isFrenzy)
        {
            score *= 2;
        }
        GameLogic.score += score;
    }
}