using Godot;
using System;

public static class GameLogic
{
    public static string player_name;
    public static string difficulty;
    public static int score = 0;
    public static int wave = 0;
    public static int gold = 0;
    public static bool inGame = false;
    public static bool inShop = false;
    public static bool isPaused = false;
    public static bool penalty = false;
}