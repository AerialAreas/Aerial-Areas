using Godot;
using System;

public class Player
{
    private string name;
    private int lives;
    public int getLives()
    {
        return lives;
    } 
    public void setLives(int value)
    {
        lives = value;
    }

    private int currency;
    public int getCurrency()
    {
        return currency;
    } 
    public void setCurrency(int value)
    {
        currency = value;
    }
    private int score;
    // Powerup/Upgrade person can take care of dictionaries

}