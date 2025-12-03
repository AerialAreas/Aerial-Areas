using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class GameLogic
{
    public static string player_name;
    public static bool nightmare_unlocked = false;
    public static string difficulty;
    public static int score = 0;
    public static int wave_num = 0;
    public static int currency = 0;
    public static int lives = 100;
    public static bool isPaused = false;
    public static bool penalty = false;
    public static bool isBiggerBoomMax = false;
    public static bool isSlowMax = false;
    public static bool isMaxLivesMax = false;
    public static Dictionary<string, int> upgrade_inventory = new Dictionary<string, int>
    {
        // Permanent for each run
        {"Bigger Booms", 1},
        {"Slow", 1},
        {"Max Lives", 1}
    };
    public static Dictionary<string, int> powerup_inventory = new Dictionary<string, int>
    {
        {"Freeze", 0}, // stops all enemy movement for a certain period of time
        {"Fireball", 0}, // destroy the closest enemy
        {"Frenzy", 0} // x2 score for a certain period of time
    };
    public static bool first_load = true;
    public static Wave wave;
    public static Game game;
    
    public static void SetToStart()
    {
        score = 0;
        wave_num = 1;
        currency = 0;
        lives = 100;
        penalty = false;
        isBiggerBoomMax = false;
        isSlowMax = false;
        isMaxLivesMax = false;
        List<string> u_keys = upgrade_inventory.Keys.ToList();
        foreach (string u_key in u_keys)
        {
            upgrade_inventory[u_key] = 1;
        }
        List<string> p_keys = powerup_inventory.Keys.ToList();
        foreach (string p_key in p_keys)
        {
            powerup_inventory[p_key] = 0;
        }
        List<string> upgrade_keys = Upgrade.upgrades.Keys.ToList();
        foreach (string upgrade_key in upgrade_keys)
        {
            Upgrade.upgrades[upgrade_key] = 100;
        }
    }
}