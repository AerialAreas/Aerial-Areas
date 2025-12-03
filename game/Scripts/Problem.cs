using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;



public abstract class Problem
{
    public string solution;    
    public string shape;
    public string problemType;
    public RichTextLabel label;
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

        label = new RichTextLabel();
        label.Text = $"[img height=32]res://Sprites/geometroid.png[/img] length = BROOKHART, height = OMARALY";
        label.FitContent = true;
        label.BbcodeEnabled = true;
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
    public string identifier;
    Triangle()
    {
        shape = "Triangle";
        base_length = rand.Next(3, 13); // 1 to 12
        height = rand.Next(3, 13); // 1 to 12
        side2 = rand.Next(3, 13);
        side3 = rand.Next(Math.Abs(side2 - base_length) + 1, side2 + base_length - 1);

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
        GD.Print($"I am a shape {shape} with length {base_length}, height {height}, side2 {side2}, side3 {side3}, type {problemType}, identifier {identifier}, solution is {solution}");
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
                    solution = (radius * radius).ToString() + "p";
                    break;
                case (int)FillType.SEMI:
                    solution = (0.5 * radius * radius).ToString() + "p";
                    break;
                case (int)FillType.QUARTER:
                    solution = (0.25 * radius * radius).ToString() + "p";
                    break;   
            }
        }
        else
        {
            problemType = "Perimeter";
            solution = (2 * radius).ToString() + "p";
        }
    }
    public override void PrintProblemData()
    {
        GD.Print($"I am a circle with r {radius}, fillType {fillType}, type {problemType}, solution is {solution}");
    }
} // end of Circle class
// public class BossProblem : Problem
// {
// }
