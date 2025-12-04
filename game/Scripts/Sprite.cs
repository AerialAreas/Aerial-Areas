using Godot;
using System;

public partial class Sprite : Node2D
{
	public Vector2 position;
	public string filePath;
	public Vector2 size;

	public Sprite()
	{
		position = new Vector2(0, 0);
		filePath = "";
		size = new Vector2(0, 0);	
	}
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
