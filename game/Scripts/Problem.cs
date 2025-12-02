using Godot;
using System;
using System.Security.Cryptography.X509Certificates;



public abstract class Problem
{
    public string solution;    
    public string shape;
    public string problemType;
    public Sprite sprite;
    public Random rand = new Random();
    public abstract void PrintProblemData();
}
public class Rectangle : Problem
{
    private int length;
    private int width;
    private bool isSquare;
    Rectangle()
    {
        shape = "Rectangle";
        length = rand.Next(1, 13); // 1 to 12
        width = rand.Next(1, 13); // 1 to 12

        if (length == width) // square?
        {
            isSquare = true;
        }
        else
        {
            isSquare = false;
        }

        // problemType init
        int typeDecider = rand.Next(0, 2); // 0 or 1
        if (typeDecider == 0)
        {
            problemType = "Area";
            solution = (length * width).ToString();
        }
        else
        {
            problemType = "Perimeter";
            solution = (2 * (length + width)).ToString();
        }
    }
    public override void PrintProblemData()
    {
        GD.Print($"I am a rectangle with length {length}, width {width}, and type {problemType}, solution is {solution}");
    }
} // end of Rectangle class
public class Triangle : Problem
{
    public int base_length;
    public int height;
    public int side2;
    public int side3;
    public int base1;
    public string identifier;
    Triangle()
    {
        shape = "Triangle";
        base_length = rand.Next(3, 13); // 1 to 12
        side2 = rand.Next(3, 13);
        side3 = rand.Next(Math.Abs(side2-base_length), side2+base_length);
        base1 = side2 / ((side2 + side3) * base_length);
        height = (int)Math.Sqrt(Math.Pow(side2, 2) - Math.Pow(base1, 2));

        // problemType init
        int typeDecider = rand.Next(0, 2); // 0 or 1
        if (typeDecider == 0)
        {
            problemType = "Area";
            solution = (0.5 * base_length * height).ToString();
        }
        else
        {
            problemType = "Perimeter";
            solution = (base_length + side2 + side3).ToString();
        }

        // set identifier based on side lengths
        if (base_length == side2 && side2 == side3)
        {
            identifier = "Equilateral";
        }
        else if (base_length == side2 || base_length == side3 || side2 == side3)
        {
            identifier = "Isosceles";
        }
        else
        {
            identifier = "Scalene";
        }
    }
    public override void PrintProblemData()
    {
        GD.Print($"I am a shape {shape} with length {base_length}, height {height}, side2 {side2}, side3 {side3}, base1{base1} type {problemType}, identifier {identifier}, solution is {solution}");
    }
} // end of Triangle class
public class Circle : Problem
{
    enum FillType { FULL = 1, SEMI = 2, QUARTER = 3 }
    private int radius;
    int fillType;
    Circle()
    {
        shape = "Circle";
        radius = rand.Next(1, 13); // 1 to 12
        fillType = rand.Next(1, 4); // 1 to 3
        int typeDecider;
        int randomX = rand.Next(50, 750); // x between 50 and 750
        
        // problemType init
        if(fillType == 1)
        {
            typeDecider = rand.Next(0, 2); // 0 or 1
        }
        else
        {
            typeDecider = 0; // only area problems for semi and quarter circles
        }
        
        if (typeDecider == 0)
        {
            problemType = "Area";
            switch (fillType)
            {
                case (int)FillType.FULL:
                    solution = (radius * radius).ToString() + "Pi";
                    break;
                case (int)FillType.SEMI:
                    solution = (0.5 * radius * radius).ToString() + "Pi";
                    break;
                case (int)FillType.QUARTER:
                    solution = (0.25 * radius * radius).ToString() + "Pi";
                    break;
                
            }
            sprite = new Sprite(new Vector2(randomX, 0), "game/Sprites/temp_circle_a.png", new Vector2(50, 50));
        }
        else
        {
            problemType = "Perimeter";
            solution = (2 * radius).ToString() + "Pi";
            sprite = new Sprite(new Vector2(randomX, 0), "game/Sprites/temp_circle_p.png", new Vector2(50, 50));
        }
        // set sprite accordingly based on problemType
    }
    public override void PrintProblemData()
    {
        //GD.Print($"I am a rectangle with length {length}, width {width}, and type {problemType}, solution is {solution}");
    }
} // end of Circle class
// public class BossProblem : Problem
// {
// }
