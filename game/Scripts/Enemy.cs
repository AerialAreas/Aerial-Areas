using Godot;
using System;

public partial class Enemy : Sprite2D
{
    public Problem problem;
    public Vector2 velocity;
    public Sprite2D sprite;
    public int value;
    public bool isHighlighted;
    protected Random rand = new Random();
    public override void _Draw()
    {
        base._Draw();
    }
    public override void _Process(double delta)
    {
        this._Draw();
    }

    public bool Move() // returns whether the enemy has made it to the bottom or not
    {
        // GD.Print($"{sprite.Position.X}, {sprite.Position.Y}");
        Vector2 current_enemy_pos = sprite.Position;
        Vector2 enemy_velocity = velocity;
        sprite.Position = new Vector2(current_enemy_pos.X + enemy_velocity.X, current_enemy_pos.Y + enemy_velocity.Y);
        if(sprite.Position.X <= GameLogic.ENEMY_LEFT_BOUND || sprite.Position.X >= GameLogic.ENEMY_RIGHT_BOUND)
        {
            velocity = new Vector2(velocity.X * -1, velocity.Y);
        }
        return sprite.Position.Y >= GameLogic.ENEMY_ESCAPE_BOUND;
        // GD.Print($"{sprite.Position.X}, {sprite.Position.Y}");
    }

    public Enemy() {}

    public Enemy(string shape)
    {
        string texture_path = "";
        switch (shape)
        {
            case "Rectangle":
                problem = (Problem)Activator.CreateInstance(typeof(Rectangle), true);
                if(problem.problemType == "Area")
                {
                    if (problem.shape == "Square")
                    {
                        texture_path = "res://Sprites/Enemies/enemy_square_area.png";
                    }
                    else
                    {
                        texture_path = "res://Sprites/Enemies/enemy_rectangle_area.png";
                    }
                }
                else
                {
                    if (problem.shape == "Square")
                    {
                        texture_path = "res://Sprites/Enemies/enemy_square_perimeter.png";
                    }
                    else
                    {
                        texture_path = "res://Sprites/Enemies/enemy_rectangle_perimeter.png";
                    }
                }
                break;
            case "Triangle":
                problem = (Problem)Activator.CreateInstance(typeof(Triangle), true);
                if(problem.problemType == "Area")
                {
                    texture_path = "res://Sprites/Enemies/enemy_triangle_area.png";
                }
                else
                {
                    texture_path = "res://Sprites/Enemies/enemy_triangle_perimeter.png";
                }
                break;
            case "Circle":
                problem = (Problem)Activator.CreateInstance(typeof(Circle), true);
                if(problem.problemType == "Area")
                {
                    if(problem.fill == 1)
                    {
                        texture_path = "res://Sprites/Enemies/enemy_circle_area.png";
                    }
                    else if(problem.fill == 2)
                    {
                        texture_path = "res://Sprites/Enemies/enemy_halfcircle_area.png";
                    }
                    else
                    {
                        texture_path = "res://Sprites/Enemies/enemy_quartercircle_area.png";
                    }
                }
                else
                {
                    texture_path = "res://Sprites/Enemies/enemy_circle_circumference.png";
                }
                break;
        }
        float VELOCITY_MULTIPLIER = GameLogic.difficulty_speed_multiplier[GameLogic.difficulty];
        velocity = new Vector2((GD.Randf() - 0.5f) / 5f, 0.1f + (GD.Randf() / 10f)); // 0.1f to 0.2f down and -0.1f to 0.1f horizontal
        velocity = new Vector2(velocity.X * VELOCITY_MULTIPLIER, velocity.Y * VELOCITY_MULTIPLIER);
        
        sprite = new Sprite2D();
        sprite.Position = new Vector2(rand.Next(GameLogic.ENEMY_LEFT_BOUND + 10, GameLogic.ENEMY_RIGHT_BOUND - 10), GameLogic.ENEMY_SPAWN_Y);
        sprite.Texture = GD.Load<Texture2D>(texture_path);
        sprite.Scale = new Vector2(.25f, .25f);
        SetScript(GD.Load<Script>("res://Scripts/Enemy.cs"));

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

    public void giveMoney()
    {
        GD.Print("Jew jew jew");
    }
}

// public class BossEnemy : Enemy
// {
// }
