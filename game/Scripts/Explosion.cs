using Godot;
using System;

public class Explosion
{
    public Sprite2D sprite;

    public Explosion(Vector2 position)
    {
        sprite = new Sprite2D();
        sprite.Position = position;
        // sprite.Texture = GD.Load<Texture2D>("res://Sprites/temp_explosion.png");
        sprite.Scale = new Vector2(.2f, .2f);
    }
}
