using Godot;
using System;

public class Sprite
{
	Vector2 position;
	string filePath;
	Vector2 size;
	public Sprite(Vector2 pos, string path, Vector2 sz)
	{
		position = pos;
		filePath = path;
		size = sz;
	}

	public void drawSprite()
	{
		
	}
}
