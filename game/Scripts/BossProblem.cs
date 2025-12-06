using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

public abstract class BossProblem
{
    public List<Enemy> enemy_list = new List<Enemy>();
    public Random rand = new Random();
}

public class RectangleBoss: BossProblem
{
    RectangleBoss()
    {
        for (int i = 0; i < 15; i++)
        {
            Enemy e = new Enemy();
            e.problem = new Rectangle();
            enemy_list.Add(e);
        }
    }
}

public class TriangleBoss: BossProblem
{
    TriangleBoss()
    {
        for (int i = 0; i < 15; i++)
        {
            Enemy e = new Enemy();
            e.problem = new Triangle();
            enemy_list.Add(e);
        }
    }
}

public class CircleBoss: BossProblem
{
    CircleBoss()
    {
        for (int i = 0; i < 15; i++)
        {
            Enemy e = new Enemy();
            e.problem = new Circle();
            enemy_list.Add(e);
        }
    }
}

public class FinalBoss: BossProblem
{
    FinalBoss()
    {
        for (int i = 0; i < 20; i++)
        {
            int num = rand.Next(1, 4);
            if (num == 1)
            {
                Enemy e = new Enemy();
                e.problem = new Rectangle();
                enemy_list.Add(e);
            }
            else if (num == 2)
            {
                Enemy e = new Enemy();
                e.problem = new Triangle();
                enemy_list.Add(e);
            }
            else
            {
                Enemy e = new Enemy();
                e.problem = new Circle();
                enemy_list.Add(e);
            }
        }
    }
}