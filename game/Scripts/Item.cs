using Godot;
using System.Collections.Generic;

public abstract class Item
{
	string item_name;
	int cost;
	Sprite2D sprite;
	string powerup_or_item;
}

public class Powerup : Item
{
	private void UsePowerup()
	{
		// todo
	}
}

public class Upgrade : Item
{
	List<int> scaling_per_level = new List<int>(); // List of how much better it gets per upgrade (first value is how much at level 1, etc)
}