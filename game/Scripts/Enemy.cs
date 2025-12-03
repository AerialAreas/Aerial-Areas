using Godot;
using System;

public partial class Enemy : Sprite2D
{
    public Problem problem;
    public Vector2 velocity;
    public Sprite2D sprite;
    public Texture2D texture;
    public int value;
    public bool isHighlighted;
    protected Random rand = new Random();
    public override void _Draw()
    {
        base._Draw();
        GD.Print("c");
    }
    public override void _Process(double delta)
    {
        this._Draw();
    }

    public void Move()
    {
        GD.Print($"{sprite.Position.X}, {sprite.Position.Y}");
        Vector2 current_enemy_pos = sprite.Position;
        Vector2 enemy_velocity = velocity;
        sprite.Position = new Vector2(current_enemy_pos.X + enemy_velocity.X, current_enemy_pos.Y + enemy_velocity.Y);
        GD.Print($"{sprite.Position.X}, {sprite.Position.Y}");
    }

    public Enemy(string shape)
    {
        switch (shape)
        {
            case "Rectangle":
                problem = (Problem)Activator.CreateInstance(typeof(Rectangle), true);
                texture = GD.Load<Texture2D>("res://Sprites/geometroid.png");
                break;
            case "Triangle":
                problem = (Problem)Activator.CreateInstance(typeof(Triangle), true);
                texture = GD.Load<Texture2D>("res://Sprites/geometroid.png");
                break;
            case "Circle":
                problem = (Problem)Activator.CreateInstance(typeof(Circle), true);
                texture = GD.Load<Texture2D>("res://Sprites/geometroid.png");
                break;
        } // end of problem init

        // position init based on UI, all top of screen
        velocity = new Vector2(0, 1); // downwards for now
        sprite = new Sprite2D();
        sprite.Position = new Vector2(500, 500);
        // value init
        value = rand.Next(10, 21); // random between 10 and 20

        isHighlighted = false;

        // GD.Print($"Spawned an enemy with shape {shape} and velocity {velocity.X}, {velocity.Y} and value {value} and sprite path {sprite.filePath} and sprite pos {sprite.position.X}, {sprite.position.Y}");
        // problem.PrintProblemData();
    }
    public bool compareAnswer(string input)
    {
        if (input.Equals(problem.solution))
        {
            return true;
        }
        return false;
    }
}

// public class BossEnemy : Enemy
// {
// }
