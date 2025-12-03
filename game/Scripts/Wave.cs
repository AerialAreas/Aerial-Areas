using Godot;
using System;
using System.Collections.Generic;

public class Wave
{
    public int waveNum;
    public List<Enemy> unspawned_enemies;
    public List<Enemy> spawned_enemies;
    public Wave(int number)
    {
        waveNum = number;
        unspawned_enemies = new List<Enemy>();
        spawned_enemies = new List<Enemy>();
    }

    public void SpawnEnemy(int index)
    {
        spawned_enemies.Add(unspawned_enemies[index]);
        unspawned_enemies.Remove(unspawned_enemies[index]);
    }

    public void displayProblems()
    {
        foreach(Enemy enemy in spawned_enemies)
        {
            // Display enemy.problem on screen
        }
    }

    public bool checkLastWave()
    {
        return waveNum >= 12;
    }

    public void HandleExplosion(Explosion explosion)
    {
        Sprite explosionSprite = explosion.sprite;
        int innerExplosionX = (int)explosionSprite.position.X;
        int innerExplosionY = (int)explosionSprite.position.Y;
        int outerExplosionX = (int)(innerExplosionX + explosionSprite.size.X);
        int outerExplosionY = (int)(innerExplosionY + explosionSprite.size.Y);
        foreach(Enemy enemy in spawned_enemies)
        {
            Sprite enemySprite = enemy.problem.sprite;
            int enemyX = (int)enemySprite.position.X;
            int enemyY = (int)enemySprite.position.Y;
            
            //assuming sprites have position toward top-left corner of png
            if (enemyX < outerExplosionX && enemyX > innerExplosionX - enemySprite.size.X &&
                enemyY < outerExplosionY && enemyY > innerExplosionY - enemySprite.size.Y)
            {
                // destroyEnemy(enemy);
            }
        }
    }

    public void destroyEnemy(Enemy enemy)
    {
        GameLogic.currency += enemy.value;
        spawned_enemies.Remove(enemy);
    }
}