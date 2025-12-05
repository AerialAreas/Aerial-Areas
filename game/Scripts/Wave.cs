using Godot;
using System;
using System.Collections.Generic;

public class Wave
{
    public int waveNum;
    public List<Enemy> unspawned_enemies;
    public Wave(int number)
    {
        waveNum = number;
        unspawned_enemies = new List<Enemy>();
    }

    public void HandleExplosion(Enemy enemy)
    {
        enemy.giveMoney();
        destroyEnemy(enemy);
        Explosion explosion = new Explosion(enemy.Position);
        // GetNode<Node>("GameContainer").AddChild(explosion);
        float ex_scale_x = explosion.sprite.Texture.GetSize().X * explosion.sprite.Scale.X / 2;
        float ex_scale_y = explosion.sprite.Texture.GetSize().Y * explosion.sprite.Scale.Y / 2;
        float ex_inner_x = explosion.sprite.Position.X - ex_scale_x;
        float ex_inner_y = explosion.sprite.Position.Y - ex_scale_y;
        float ex_outer_x = explosion.sprite.Position.X + ex_scale_x;
        float ex_outer_y = explosion.sprite.Position.Y + ex_scale_y;
        foreach(Enemy e in unspawned_enemies)
        {
            float scale_x = e.sprite.Texture.GetSize().X * e.sprite.Scale.X / 2;
            float scale_y = e.sprite.Texture.GetSize().Y * e.sprite.Scale.Y / 2;
            float inner_x = e.sprite.Position.X - scale_x;
            float inner_y = e.sprite.Position.Y - scale_y;
            float outer_x = e.sprite.Position.X + scale_x;
            float outer_y = e.sprite.Position.Y + scale_y;
            if (outer_x > ex_inner_x && inner_x < ex_outer_x && outer_y > ex_inner_y && inner_y < ex_outer_y)
            {
                e.giveMoney();
                destroyEnemy(e);
            }
         }
    }

    public void destroyEnemy(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        if (GameLogic.wave.unspawned_enemies.Contains(enemy))
        {
            GameLogic.wave.unspawned_enemies.Remove(enemy); // todo fix this so it removes spawned enemies   
        }

        if (enemy.sprite != null && enemy.sprite.IsInsideTree())
        {
            enemy.sprite.QueueFree();
        }

        if (enemy.problem != null && enemy.problem.label != null && enemy.problem.label.IsInsideTree())
        {
            enemy.problem.label.QueueFree();
        }
    }
}