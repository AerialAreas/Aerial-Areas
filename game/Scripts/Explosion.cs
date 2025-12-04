using Godot;
using System;

public class Explosion
{
    public Sprite sprite;
    public Explosion(Vector2 position)
    {
        sprite = new Sprite(position, "game/Sprites/temp_explosion.png", new Vector2(64, 64));
    }
}