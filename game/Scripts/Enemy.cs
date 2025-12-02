using Godot;
using System;

public class Enemy
{
    private Problem problem;
    private Vector2 velocity;
    public Sprite sprite;
    public int value;
    private bool isHighlighted;
    protected Random rand = new Random();
    public Enemy(string shape)
    {
        Vector2 position = new Vector2(rand.Next(0, 1), 0);
        switch (shape)
        {
            case "Rectangle":
                problem = (Problem)Activator.CreateInstance(typeof(Rectangle), true);
                break;
            case "Triangle":
                problem = (Problem)Activator.CreateInstance(typeof(Triangle), true);
                break;
            case "Circle":
                problem = (Problem)Activator.CreateInstance(typeof(Circle), true);
                break;
        } // end of problem init

        // position init based on UI, all top of screen
        velocity = new Vector2(0, 5); // downwards for now

        // value init
        value = rand.Next(10, 21); // random between 10 and 20

        isHighlighted = false;
    }

    public Problem getProblem()
    {
        return problem;
    }
    public Vector2 getVelocity()
    {
        return velocity;
    }
    public void setVelocity(Vector2 vel)
    {
        velocity = vel;
    }

    public void takeLife()
    {
        GameLogic.lives -= 1;
    }
    public void giveMoney(int money)
    {
        GameLogic.currency += money;
    }
    public bool compareAnswer(string input)
    {
        if (input.Equals(problem.getSolution()))
        {
            return true;
        }
        return false;
    }
}

// public class BossEnemy : Enemy
// {
// }
