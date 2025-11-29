using Godot;
using System;

public class Sprite
{
	Vector2 position;
	string filePath;
	Vector2 size;

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

	public Vector2 getPosition()
	{
		return position;
	}
	public void setPosition(Vector2 pos){
		position = pos;
	}
	public Vector2 getSize()
	{
		return size;
	}
	public void setSize(Vector2 sz){
		size = sz;
	}

	public void drawSprite()
	{
		
	}
}
