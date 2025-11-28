using Godot;
using System;

public static class GameLogic
{
    public static bool nightmare_unlocked = false;
    public static string player_name;
    public static string difficulty;
    public static int score = 0;
    public static int wave = 0;
    public static int gold = 0;
    public static bool isPaused = false;
    public static bool penalty = false;

    public static void HandleTick()
    {
        // GD.Print("A game tick has been handled.");
    }
    public static void SetToStart()
    {
        score = 0;
        wave = 1;
        gold = 0;
        penalty = false;
    }
}