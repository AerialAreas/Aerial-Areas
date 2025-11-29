using Godot;
using System;



public abstract class Problem
{
    protected string solution;
    public string getSolution()
    {
        return solution;
    }
    public void setSolution(string value)
    {
        solution = value;
    }
    
    protected string shape;
    protected string problemType;
    protected Sprite sprite;
    protected Random rand = new Random();
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

        if (isSquare) // set sprite accordingly
        {
            // if problemType == area, set sprite = square area sprite
            if (problemType == "Area")
            {
                // set sprite to square area sprite
            }
            else
            {
                // set sprite to square perimeter sprite
            }
            // else if problemType == perimeter, set sprite = square perimeter sprite

        }
        else
        {
            // if problemType == area, set sprite = rectangle area sprite
            // else if problemType == perimeter, set sprite = rectangle perimeter sprite
        }
    }
} // end of Rectangle class
public class Triangle : Problem
{
    private int baseLength;
    private int height;
    private int side2;
    private int side3;
    private string identifier;
    Triangle()
    {
        shape = "Triangle";
        baseLength = rand.Next(1, 13); // 1 to 12
        height = rand.Next(1, 13); // 1 to 12
        side2 = rand.Next(1, 13);
        side3 = rand.Next(1, 13);

        // problemType init
        int typeDecider = rand.Next(0, 2); // 0 or 1
        if (typeDecider == 0)
        {
            problemType = "Area";
            solution = (0.5 * baseLength * height).ToString();
        }
        else
        {
            problemType = "Perimeter";
            solution = (baseLength + side2 + side3).ToString();
        }

        // set identifier based on side lengths
        if (baseLength == side2 && side2 == side3)
        {
            identifier = "Equilateral";
        }
        else if (baseLength == side2 || baseLength == side3 || side2 == side3)
        {
            identifier = "Isosceles";
        }
        else
        {
            identifier = "Scalene";
        }

        // set sprite accordingly based on identifier and problemType
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
                    solution = (radius * radius).ToString() + "Pi";
                    break;
                case (int)FillType.SEMI:
                    solution = (0.5 * radius * radius).ToString() + "Pi";
                    break;
                case (int)FillType.QUARTER:
                    solution = (0.25 * radius * radius).ToString() + "Pi";
                    break;
            }
        }
        else
        {
            problemType = "Perimeter";
            solution = (2 * radius).ToString() + "Pi";
        }
        // set sprite accordingly based on problemType
    }
} // end of Circle class
public class BossProblem : Problem
{
}
