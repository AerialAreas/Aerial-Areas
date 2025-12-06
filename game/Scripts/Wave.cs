using Godot;
using System;
using System.Collections.Generic;

public class Wave
{
    public int waveNum;
    public List<Enemy> unspawned_enemies = new List<Enemy>();
    public BossEnemy boss;
    public Wave(int number)
    {
        waveNum = number;
        unspawned_enemies = new List<Enemy>();
    }
}