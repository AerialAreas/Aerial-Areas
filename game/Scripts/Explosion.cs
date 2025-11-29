using Godot;
using System;

public class Explosion
{
    private Sprite explosionSprite;
    public Explosion(Vector2 position)
    {
        explosionSprite = new Sprite(position, "game/Sprites/temp_explosion.png", new Vector2(64, 64));
    }

    public Sprite getSprite()
    {
        return explosionSprite;
    }
}