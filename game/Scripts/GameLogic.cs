using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class GameLogic
{
    public const int ENEMY_LEFT_BOUND = 250;
    public const int ENEMY_RIGHT_BOUND = 1015;
    public const int ENEMY_ESCAPE_BOUND = 600;
    public const int ENEMY_SPAWN_Y = -50;
    public static string player_name;
    public static bool nightmare_unlocked = false;
    public static string difficulty;
    public static int score = 0;
    public static int wave_num;
    public static int currency = 0;
    public static int lives;
    public static int max_lives;
    public static bool isPaused = false;
    public static bool inGame = false;
    public static bool isBiggerBoomMax = false;
    public static bool isSlowMax = false;
    public static bool isMaxLivesMax = false;
    public static float slow_multiplier = 1.0f;
    public static bool isFrenzy = false;
    public static bool isFreeze = false;
    public static bool sceneSwitch = false;
    public static Dictionary<string, int> upgrade_inventory = new Dictionary<string, int>
    {
        // Permanent for each run
        {"Bigger Booms", 1},
        {"Slow", 1},
        {"Max Lives", 1}
    };
    public static Dictionary<string, int> powerup_inventory = new Dictionary<string, int>
    {
        {"Freeze", 1}, // stops all enemy movement for a certain period of time
        {"Fireball", 1}, // destroy the closest enemy
        {"Frenzy", 1} // x2 score for a certain period of time
    };
    public static Dictionary<string, int> difficulty_enemy_count = new Dictionary<string, int>
    {
        {"easy", 0},
        {"medium", 2},
        {"hard", 5},
        {"nightmare", 10}
    };
    public static Dictionary<string, float> difficulty_spawn_time = new Dictionary<string, float>
    {
        {"easy", 5.0f},
        {"medium", 4.0f},
        {"hard", 3.0f},
        {"nightmare", 1.5f}
    };
    public static Dictionary<string, int> difficulty_speed_multiplier = new Dictionary<string, int>
    {
        {"easy", 1},
        {"medium", 2},
        {"hard", 3},
        {"nightmare", 5}
    };
    public static bool first_load = true;
    public static Wave wave;
    public static Game game;
    public static void SetToStart()
    {
        score = 0;
        wave_num = 1;
        currency = 0;
        lives = 5;
        max_lives = 5;
        isBiggerBoomMax = false;
        isSlowMax = false;
        isMaxLivesMax = false;
        first_load = true;
        inGame = true;
        foreach (string u_key in upgrade_inventory.Keys)
        {
            upgrade_inventory[u_key] = 1;
        }
        foreach (string p_key in powerup_inventory.Keys)
        {
            powerup_inventory[p_key] = 1;
        }
        foreach (string upgrade_key in Upgrade.upgrades.Keys)
        {
            Upgrade.upgrades[upgrade_key] = 100;
        }
    }
}