using Godot;
using System;

public class Enemy
{
    public Problem problem;
    public Vector2 velocity;
    public Sprite sprite;
    public int value;
    public bool isHighlighted;
    protected Random rand = new Random();
    public Enemy(string shape)
    {
        switch (shape)
        {
            case "Rectangle":
                problem = (Problem)Activator.CreateInstance(typeof(Rectangle), true);
                sprite = new Sprite(new Vector2(rand.Next(500, 600), 300), "res://Sprites/geometroid.png", new Vector2(1, 1));
                break;
            case "Triangle":
                problem = (Problem)Activator.CreateInstance(typeof(Triangle), true);
                sprite = new Sprite(new Vector2(rand.Next(500, 600), 300), "res://Sprites/geometroid.png", new Vector2(1, 1));
                break;
            case "Circle":
                problem = (Problem)Activator.CreateInstance(typeof(Circle), true);
                sprite = new Sprite(new Vector2(rand.Next(500, 600), 300), "res://Sprites/geometroid.png", new Vector2(1, 1));
                break;
        } // end of problem init

        // position init based on UI, all top of screen
        velocity = new Vector2(0, 5); // downwards for now

        // value init
        value = rand.Next(10, 21); // random between 10 and 20

        isHighlighted = false;

        GD.Print($"Spawned an enemy with shape {shape} and velocity {velocity.X}, {velocity.Y} and value {value} and sprite path {sprite.filePath} and sprite pos {sprite.position.X}, {sprite.position.Y}");
        problem.PrintProblemData();
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
