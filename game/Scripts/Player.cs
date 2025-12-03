using Godot;
using System;

public static class Player
{
    private static string name;
    private static int lives;
    private static int currency;
    private static int score;
    // Powerup/Upgrade person can take care of dictionaries

    public static int getLives()
    {
        return lives;
    } 
    public static void setLives(int value)
    {
        lives = value;
    }

    public static int getCurrency()
    {
        return currency;
    } 
    public static void setCurrency(int value)
    {
        currency = value;
    }
}