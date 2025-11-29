using Godot;
using System;

public abstract class Enemy
{
    private Problem problem;
    private Vector2 position;
    private Vector2 velocity;
    private int value;
    private bool isHighlighted;
    protected Random rand = new Random();
    public Enemy(string shape)
    {
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
        position = new Vector2(rand.Next(50, 750), 0); // x between 50 and 750, y = 0
        velocity = new Vector2(0, 5); // downwards for now

        // value init
        value = rand.Next(10, 21); // random between 10 and 20

        isHighlighted = false;
    }

    void move()
    {
        // if horizontal movement results in wall collision, flip h direction
        position.Y += velocity.Y;
        position.X += velocity.X;
        if (position.X <= 0 || position.X >= 800) // wall collision
        {
            velocity.X = -velocity.X;
        }
    }
    void takeLife()
    {
        Player.setLives(Player.getLives() - 1);
    }
    void enemyClick()
    {
        isHighlighted = true;
    }
    void giveMoney()
    {
        Player.setCurrency(Player.getCurrency() + value);
    }
    bool compareAnswer(string input)
    {
        if (input.Equals(problem.getSolution()))
        {
            return true;
        }
        return false;
    }
}
/*public class BossEnemy : Enemy
{
}*/
