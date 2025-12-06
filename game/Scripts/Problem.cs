using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

public abstract class Problem
{
    public int fill; // -1 for triangles and rectanges, 1 for full circle, 2 for half, 4 for quarter
    public string solution;    
    public string shape;
    public string problemType;
    public RichTextLabel label;
    public Random rand = new Random();
    public abstract void PrintProblemData();
    public abstract void UpdateLabel(bool hl);
    public Button clickable;

    public Problem()
    {
        Button b = new Button();
        b.Size = new Godot.Vector2(225f, 32);
        b.Modulate = new Color(0,0,0,0);
        clickable = b;
    }
    public string fontPath = "res://Font/ScienceGothic-VariableFont_CTRS,slnt,wdth,wght.ttf";
}
public class Rectangle : Problem
{
    public int length;
    public int width;
    public Rectangle()
    {
        length = rand.Next(1, 13); // 1 to 12
        width = rand.Next(1, 13); // 1 to 12

        if (length == width) // square?
        {
            shape = "Square";
        }
        else
        {
            shape = "Rectangle";
        }

        label = new RichTextLabel();
        label.FitContent = true;
        label.BbcodeEnabled = true;
        label.AddThemeFontOverride("normal_font", GD.Load<FontFile>(fontPath));
        label.AddThemeFontSizeOverride("normal_font_size", 15);
        // problemType init
        int typeDecider = rand.Next(0, 2); // 0 or 1
        if (typeDecider == 0)
        {
            problemType = "Area";
            solution = (length * width).ToString();
            label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_rectangle_area.png[/img] length = {length}, width = {width}";
        }
        else
        {
            problemType = "Perimeter";
            solution = (2 * (length + width)).ToString();
            label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_rectangle_perimeter.png[/img] length = {length}, width = {width}";
        }
    }

    public override void UpdateLabel(bool hl)
    {
        if (problemType == "Area")
        {
            if (hl)
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_rectangle_area.png[/img][color=#FFFF00] length = {length}, width = {width}[/color]";
            }
            else
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_rectangle_area.png[/img][color=#FFFFFF] length = {length}, width = {width}[/color]";
            }
        }
        else
        {
            if (hl)
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_rectangle_perimeter.png[/img][color=#FFFF00] length = {length}, width = {width}[/color]";
            }
            else
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_rectangle_perimeter.png[/img][color=#FFFFFF] length = {length}, width = {width}[/color]";
            }
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
    public Triangle()
    {
        shape = "Triangle";
        base_length = rand.Next(3, 13); // 3 to 12
        height = rand.Next(3, 13); // 3 to 12
        side2 = rand.Next(3, 13);
        side3 = rand.Next(Math.Abs(side2 - base_length) + 1, side2 + base_length - 1);

        // problemType init
        label = new RichTextLabel();
        label.FitContent = true;
        label.BbcodeEnabled = true;
        label.AddThemeFontOverride("normal_font", GD.Load<FontFile>(fontPath));
        label.AddThemeFontSizeOverride("normal_font_size", 15);
        int typeDecider = rand.Next(0, 2); // 0 or 1
        if (typeDecider == 0)
        {
            problemType = "Area";
            solution = (0.5 * base_length * height).ToString();
            label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_triangle_area.png[/img] base = {base_length}, height = {height}";
        }
        else
        {
            problemType = "Perimeter";
            solution = (base_length + side2 + side3).ToString();
            label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_triangle_perimeter.png[/img] side lengths {base_length}, {side2}, {side3}";
        }
    }

    public override void UpdateLabel(bool hl)
    {
        if (problemType == "Area")
        {
            if (hl)
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_triangle_area.png[/img][color=FFFF00] base = {base_length}, height = {height}[/color]";
            }
            else
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_triangle_area.png[/img][color=FFFFFF] base = {base_length}, height = {height}[/color]";
            }
        }
        else
        {
            if (hl)
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_triangle_perimeter.png[/img][color=FFFF00]side lengths {base_length}, {side2}, {side3}[/color]";
            }
            else
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_triangle_perimeter.png[/img][color=FFFFFF]side lengths {base_length}, {side2}, {side3}[/color]";
            }
        }
    }
    public override void PrintProblemData()
    {
        GD.Print($"I am a shape {shape} with length {base_length}, height {height}, side2 {side2}, side3 {side3}, type {problemType}, solution is {solution}");
    }
}
public class Circle : Problem
{
    public enum FillType { FULL = 1, SEMI = 2, QUARTER = 3 }
    private int radius;
    public int fillType;
    public Circle()
    {
        shape = "Circle";
        radius = rand.Next(1, 13); // 1 to 12
        fillType = rand.Next(1, 4); // 1 to 3
        fill = fillType;
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
        
        label = new RichTextLabel();
        label.FitContent = true;
        label.BbcodeEnabled = true;
        label.AddThemeFontOverride("normal_font", GD.Load<FontFile>(fontPath));
        label.AddThemeFontSizeOverride("normal_font_size", 15);
        if (typeDecider == 0)
        {
            problemType = "Area";
            switch (fillType)
            {
                case (int)FillType.FULL:
                    solution = (radius * radius).ToString() + "p";
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_circle_area.png[/img] radius = {radius}";
                    break;
                case (int)FillType.SEMI:
                    solution = (0.5 * radius * radius).ToString() + "p";
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_halfcircle_area.png[/img] radius = {radius}";
                    break;
                case (int)FillType.QUARTER:
                    solution = (0.25 * radius * radius).ToString() + "p";
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_quartercircle_area.png[/img] radius = {radius}";
                    break;   
            }
        }
        else
        {
            problemType = "Perimeter";
            solution = (2 * radius).ToString() + "p";
            label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_circle_circumference.png[/img] radius = {radius}";
        }
    }

    public override void UpdateLabel(bool hl)
    {
        if (problemType == "Area")
        {
            if (hl)
            {
                if (fillType == 1)
                {
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_circle_area.png[/img][color=FFFF00] radius = {radius}[/color]";
                }
                else if (fillType == 2)
                {
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_halfcircle_area.png[/img][color=FFFF00] radius = {radius}[/color]";
                }
                else
                {
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_quartercircle_area.png[/img][color=FFFF00] radius = {radius}[/color]";
                }
            }
            else
            {
                if (fillType == 1)
                {
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_circle_area.png[/img][color=FFFFFF] radius = {radius}[/color]";
                }
                else if (fillType == 2)
                {
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_halfcircle_area.png[/img][color=FFFFFF] radius = {radius}[/color]";
                }
                else
                {
                    label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_quartercircle_area.png[/img][color=FFFFFF] radius = {radius}[/color]";
                }
            }
        }
        else
        {
            if (hl)
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_circle_circumference.png[/img][color=FFFF00] radius = {radius}[/color]";
            }
            else
            {
                label.Text = $"[img height=32]res://Sprites/ProblemList/problemlist_circle_circumference.png[/img][color=FFFFFF] radius = {radius}[/color]";
            }
        }
    }
    public override void PrintProblemData()
    {
        GD.Print($"I am a circle with r {radius}, fillType {fillType}, type {problemType}, solution is {solution}");
    }
} // end of Circle class
