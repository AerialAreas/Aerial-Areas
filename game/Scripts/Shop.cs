using Godot;
using System;
using System.Collections.Generic;

public partial class Shop : Node2D
{
    public readonly List<string> too_little_money_messages = new List<string>{"Save up a little more money for that one.", "You can't afford that, sorry."};
    public readonly List<string> already_maxed_messages = new List<string>{"Relax, you have it maxed out.", "Thats enough buddy, no more greed."};
    public readonly List<string> successfully_bought_messages = new List<string>{"Pleasure doing business.", "My bank account is so happy right now.", "No ice soup for me tonight!"};
    public readonly List<string> enter_shop_messages = new List<string>{"Check out my upgrades and powerups!", "Welcome to my business."};
    public Random rand = new Random();
    public override void _Ready()
    {
        InitializeUIEvents();
        GameLogic.sceneSwitch = false;
        ShopKeeperSays("enter_shop");
    }

    public void InitializeUIEvents()
    {
        GetNode<Timer>("SpeechBubble/Timer").Timeout += SpeechBubbleVanish;

        GetNode<Label>("Money").Text = $"💵:{GameLogic.currency}";
        GetNode<Button>("DebugMoreMoney").Connect(Button.SignalName.Pressed, Callable.From(MoreMoneyButton));
        GetNode<Button>("GoBack").Connect(Button.SignalName.Pressed, Callable.From(OnGoBackButton));

        GetNode<Button>("Items/Upgrades/BiggerBooms").Connect(Button.SignalName.Pressed, Callable.From(BiggerBoomsBought));
        GetNode<Button>("Items/Upgrades/BiggerBooms").Text = $"💥\nBigger Booms\n{Upgrade.upgrades["Bigger Booms"]}\nLevel: {GameLogic.upgrade_inventory["Bigger Booms"]}";
        if (GameLogic.isBiggerBoomMax)
        {
            GetNode<Button>("Items/Upgrades/BiggerBooms").Text = "💥\nBigger Booms\nMAX";
        }
        
        GetNode<Button>("Items/Upgrades/Slow").Connect(Button.SignalName.Pressed, Callable.From(SlowBought));
        GetNode<Button>("Items/Upgrades/Slow").Text = $"🐌\nSlow\n{Upgrade.upgrades["Slow"]}\nLevel: {GameLogic.upgrade_inventory["Slow"]}";
        if (GameLogic.isSlowMax)
        {
            GetNode<Button>("Items/Upgrades/Slow").Text = "🐌\nSlow\nMAX";
        }

        GetNode<Button>("Items/Upgrades/MaxLives").Connect(Button.SignalName.Pressed, Callable.From(MaxLivesBought));
        GetNode<Button>("Items/Upgrades/MaxLives").Text = $"❤️\nMax Lives\n{Upgrade.upgrades["Max Lives"]}\nLevel: {GameLogic.upgrade_inventory["Max Lives"]}";
        if (GameLogic.isMaxLivesMax)
        {
            GetNode<Button>("Items/Upgrades/MaxLives").Text = "❤️\nMax Lives\nMAX";
        }

        GetNode<Button>("Items/Powerups/Fireball").Connect(Button.SignalName.Pressed, Callable.From(FireballBought));
        GetNode<Button>("Items/Powerups/Fireball").Text = $"🔥\nFireball\n{Powerup.powerups["Fireball"]}\nQuantity: {GameLogic.powerup_inventory["Fireball"]}";

        GetNode<Button>("Items/Powerups/Freeze").Connect(Button.SignalName.Pressed, Callable.From(FreezeBought));
        GetNode<Button>("Items/Powerups/Freeze").Text = $"🧊\nFreeze\n{Powerup.powerups["Freeze"]}\nQuantity: {GameLogic.powerup_inventory["Freeze"]}";

        GetNode<Button>("Items/Powerups/Frenzy").Connect(Button.SignalName.Pressed, Callable.From(FrenzyBought));
        GetNode<Button>("Items/Powerups/Frenzy").Text = $"⚡\nFrenzy\n{Powerup.powerups["Frenzy"]}\nQuantity: {GameLogic.powerup_inventory["Frenzy"]}";
        
        GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
        GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/winWave.wav");
        if (UIHelper.volume == 0)
        {
            GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
        }
        if (UIHelper.sfx) {
            GetNode<AudioStreamPlayer>("ShopSFX").Play();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void ShopKeeperSays(string type)
    {
        if(type == "bought")
        {
            GetNode<Label>("SpeechBubble/SpeechBubbleText").Text = successfully_bought_messages[rand.Next(0, successfully_bought_messages.Count)];
        }
        else if(type == "cant_afford")
        {
            GetNode<Label>("SpeechBubble/SpeechBubbleText").Text = too_little_money_messages[rand.Next(0, too_little_money_messages.Count)];
        }
        else if (type == "already_maxed")
        {
            GetNode<Label>("SpeechBubble/SpeechBubbleText").Text = already_maxed_messages[rand.Next(0, already_maxed_messages.Count)];  
        }
        else if(type == "enter_shop")
        {
            GetNode<Label>("SpeechBubble/SpeechBubbleText").Text = enter_shop_messages[rand.Next(0, enter_shop_messages.Count)];  
        }
        GetNode<Timer>("SpeechBubble/Timer").Start();
        GetNode<Sprite2D>("SpeechBubble").Visible = true;
    }
    public void SpeechBubbleVanish()
    {
        GetNode<Sprite2D>("SpeechBubble").Visible = false;
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

    public void OnGoBackButton()
    {
        GameLogic.wave_num++;
        UIHelper.SwitchSceneTo(this, "Game");
    }

    public void BiggerBoomsBought()
    {
        if (CanBuy("Bigger Booms", false))
        {
            Upgrade.IncreaseLevel("Bigger Booms");
            if ((GameLogic.upgrade_inventory["Bigger Booms"] - 1) <= 3)
            {
                Upgrade.upgrades["Bigger Booms"] = Upgrade.cost_per_level[GameLogic.upgrade_inventory["Bigger Booms"] - 1];
                GetNode<Button>("Items/Upgrades/BiggerBooms").Text = $"💥\nBigger Booms\n{Upgrade.upgrades["Bigger Booms"]}\nLevel: {GameLogic.upgrade_inventory["Bigger Booms"]}";
            }

            if (GameLogic.upgrade_inventory["Bigger Booms"] == 5)
            {
                GetNode<Button>("Items/Upgrades/BiggerBooms").Text = "💥\nBigger Booms\nMAX";
                GameLogic.isBiggerBoomMax = true;
            }
        }
    }
    public void SlowBought()
    {
        if (CanBuy("Slow", false))
        {
            Upgrade.IncreaseLevel("Slow");
            GameLogic.slow_multiplier -= 0.1f;
            if ((GameLogic.upgrade_inventory["Slow"] - 1) <= 3)
            {
                Upgrade.upgrades["Slow"] = Upgrade.cost_per_level[GameLogic.upgrade_inventory["Slow"] - 1];
                GetNode<Button>("Items/Upgrades/Slow").Text = $"🐌\nSlow\n{Upgrade.upgrades["Slow"]}\nLevel: {GameLogic.upgrade_inventory["Slow"]}";
            }

            if (GameLogic.upgrade_inventory["Slow"] == 5)
            {
                GetNode<Button>("Items/Upgrades/Slow").Text = "🐌\nSlow\nMAX";
                GameLogic.isSlowMax = true;
            }
        }
    }
    public void MaxLivesBought()
    {
        if (CanBuy("Max Lives", false))
        {
            Upgrade.IncreaseLevel("Max Lives");
            GameLogic.lives += 5;
            GameLogic.max_lives += 5;
            if ((GameLogic.upgrade_inventory["Max Lives"] - 1) <= 3)
            {
                Upgrade.upgrades["Max Lives"] = Upgrade.cost_per_level[GameLogic.upgrade_inventory["Max Lives"] - 1];
                GetNode<Button>("Items/Upgrades/MaxLives").Text = $"❤️\nMax Lives\n{Upgrade.upgrades["Max Lives"]}\nLevel: {GameLogic.upgrade_inventory["Max Lives"]}";
            }

            if (GameLogic.upgrade_inventory["Max Lives"] == 5)
            {
                GetNode<Button>("Items/Upgrades/MaxLives").Text = "❤️\nMax Lives\nMAX";
                GameLogic.isMaxLivesMax = true;
            }
        }
    }
    public void FireballBought()
    {
        if (CanBuy("Fireball", true))
        {
            Powerup.GivePowerUp("Fireball");
            GetNode<Button>("Items/Powerups/Fireball").Text = $"🔥\nFireball\n{Powerup.powerups["Fireball"]}\nQuantity: {GameLogic.powerup_inventory["Fireball"]}";
        }
    }
    public void FreezeBought()
    {
        if (CanBuy("Freeze", true))
        {
            Powerup.GivePowerUp("Freeze");
            GetNode<Button>("Items/Powerups/Freeze").Text = $"🧊\nFreeze\n{Powerup.powerups["Freeze"]}\nQuantity: {GameLogic.powerup_inventory["Freeze"]}";
        }
    }
    public void FrenzyBought()
    {
        if (CanBuy("Frenzy", true))
        {
            Powerup.GivePowerUp("Frenzy");
            GetNode<Button>("Items/Powerups/Frenzy").Text = $"⚡\nFrenzy\n{Powerup.powerups["Frenzy"]}\nQuantity: {GameLogic.powerup_inventory["Frenzy"]}";
        }
    }
    public void MoreMoneyButton()
    {
        GameLogic.currency += 5000;
        GetNode<Label>("Money").Text = $"💵:{GameLogic.currency}";
    }
    private bool CanBuy(string item_name, bool isPowerUp)
    {
        if (isPowerUp)
        {
            if (GameLogic.currency >= Powerup.powerups[item_name] && GameLogic.powerup_inventory.TryGetValue(item_name, out int value) && value < 10)
            {
                GameLogic.currency -= Powerup.powerups[item_name];
                GetNode<Label>("Money").Text = $"💵:{GameLogic.currency}";
                GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/purchase.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("ShopSFX").Play();
                }
                ShopKeeperSays("bought");
                return true;
            }
            else if (GameLogic.currency < Powerup.powerups[item_name])
            {
                GetNode<Label>("DebugText").Text = "Haha You are a brokie Haha";
                GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("ShopSFX").Play();
                }
                ShopKeeperSays("cant_afford");
                return false;
            }
            else
            {
                GetNode<Label>("DebugText").Text = "Thats enough, buddy. No more greed.";
                GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("ShopSFX").Play();
                }
                ShopKeeperSays("already_maxed");
                return false;
            }
        }
        else
        {
           if (GameLogic.currency >= Upgrade.upgrades[item_name] && GameLogic.upgrade_inventory.TryGetValue(item_name, out int value) && value < 5)
            {
                ShopKeeperSays("bought");
                GameLogic.currency -= Upgrade.upgrades[item_name];
                GetNode<Label>("Money").Text = $"💵:{GameLogic.currency}";
                GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/purchase.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("ShopSFX").Play();
                }
                return true;
            }
            else if (GameLogic.currency < Upgrade.upgrades[item_name])
            {
                GetNode<Label>("DebugText").Text = "Haha You are a brokie Haha";
                GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("ShopSFX").Play();
                }
                ShopKeeperSays("cant_afford");
                return false;
            }
            else
            {
                GetNode<Label>("DebugText").Text = "Thats enough, buddy. No more greed.";
                GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -10.0f + UIHelper.volume/5.0f;
                GetNode<AudioStreamPlayer>("ShopSFX").Stream = (Godot.AudioStream)GD.Load("res://Audio/failNoise.wav");
                if (UIHelper.volume == 0)
                {
                    GetNode<AudioStreamPlayer>("ShopSFX").VolumeDb = -80.0f;
                }
                if (UIHelper.sfx) {
                    GetNode<AudioStreamPlayer>("ShopSFX").Play();
                }
                ShopKeeperSays("already_maxed");
                return false;
            } 
        }
        
    }
}
