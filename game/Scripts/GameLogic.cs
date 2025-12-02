using Godot;
using System;

public static class GameLogic
{
    public static bool nightmare_unlocked = false;
    public static string player_name;
    public static string difficulty;
    public static int score = 0;
    public static int wave_num = 0;
    public static int currency = 0;
    public static int lives = 100;
    public static bool isPaused = false;
    public static bool penalty = false;
    public static bool first_load = true;
    public static Wave wave;

    public static void HandleTick()
    {
        // GD.Print("A game tick has been handled.");
    }
    public static void SetToStart()
    {
        score = 0;
        wave_num = 1;
        currency = 0;
        lives = 100;
        penalty = false;
    }
}