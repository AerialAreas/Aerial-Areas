using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;

public abstract class Item
{
    
}

public class Powerup : Item
{
	public static Dictionary<string, int> powerups = new Dictionary<string, int>
    {
        {"Freeze", 100},
        {"Fireball", 100},
        {"Frenzy", 100}
    };

	public static void GivePowerUp(string name)
    {
        GameLogic.powerup_inventory[name] += 1;
    }
	
	public static bool UsePowerup(string name)
	{
		if (GameLogic.powerup_inventory.TryGetValue(name, out int value) && value > 0)
        {
            GameLogic.powerup_inventory[name] -= 1;
            return true;
        }
        return false;
	}
}

public class Upgrade : Item
{
	public static List<int> cost_per_level = [100, 200, 300, 400]; // List of how much better it gets per upgrade (first value is how much at level 1, etc)
    public static Dictionary<string, int> upgrades = new Dictionary<string, int>
    {
        {"Bigger Booms", cost_per_level[GameLogic.upgrade_inventory["Bigger Booms"] - 1]},
        {"Slow", cost_per_level[GameLogic.upgrade_inventory["Bigger Booms"] - 1]},
        {"Max Lives", cost_per_level[GameLogic.upgrade_inventory["Bigger Booms"] - 1]}
    };

    public static void IncreaseLevel(string name)
    {
        GameLogic.upgrade_inventory[name] += 1;
    }
}