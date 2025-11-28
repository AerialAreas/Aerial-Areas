using Godot;
using Player;
using Problem;
using Random;
using System;


public abstract class Enemy
{
    private Problem problem;
    private Vector2 position;
    private int value;
    private bool isHighlighted;

    Enemy(string furthestUnlockedShape)
    {
        Random rand = new Random();
        // furthestUnlockedShape is the last shape introduced, if its a triangle, rectangles and triangles can be spawned.
        int shapesAllowed = 0;
        switch (furthestUnlockedShape)
        {
            case "Rectangle":
                shapesAllowed = 1;
                break;
            case "Triangle":
                shapesAllowed = 2;
                break;
            case "Circle":
                shapesAllowed = 3;
                break;
        }
        int shapeToSpawn = rand.Next(0, shapesAllowed); // generating random shape
        switch (shapeToSpawn)
        {
            case 0:
                problem = new Rectangle();
                break;
            case 1:
                problem = new Triangle();
                break;
            case 2:
                problem = new Circle();
                break;
        } // end of problem init

        // position init based on UI, all top of screen

        // value init
        value = rand.Next(10, 21); // random between 10 and 20

        isHighlighted = false;
    }

    void move()
    {
        // if horizontal movement results in wall collision, flip h direction
    }
    void takeLife(Player target)
    {
        target.setLives(target.getLives() - 1);
    }
    void enemyClick(Event key)
    {
        
    }
    void giveMoney(Player target)
    {
        target.setCurrency(target.getCurrency() + value);
    }
    bool compareAnswer(string input)
    {
        if (input.equals(problem.solution))
        {
            return true;
        }
        return false;
    }
}
public class BossEnemy : Enemy
{
}
