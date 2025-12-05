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

    public void SpawnEnemy(int index)
    {
        unspawned_enemies.Remove(unspawned_enemies[index]);
    }

    public void HandleExplosion(Explosion explosion)
    {
        // Sprite2D explosionSprite = explosion.sprite;
        // int innerExplosionX = (int)explosionSprite.position.X;
        // int innerExplosionY = (int)explosionSprite.position.Y;
        // int outerExplosionX = (int)(innerExplosionX + explosionSprite.size.X);
        // int outerExplosionY = (int)(innerExplosionY + explosionSprite.size.Y);
        // foreach(Enemy enemy in spawned_enemies)
        // {
        //     Sprite2D enemySprite = enemy.sprite;
        //     int enemyX = (int)enemySprite.Position.X;
        //     int enemyY = (int)enemySprite.Position.Y;
            
        //     //assuming sprites have position toward top-left corner of png
        //     if (enemyX < outerExplosionX && enemyX > innerExplosionX - enemySprite.size.X &&
        //         enemyY < outerExplosionY && enemyY > innerExplosionY - enemySprite.size.Y)
        //     {
        //         // destroyEnemy(enemy);
        //     }
        // }
    }
}