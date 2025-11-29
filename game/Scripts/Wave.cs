using Godot;
using System;
using System.Collections.Generic;

public class Wave
{
    private int waveNum;
    private List<Enemy> enemyList;
    private List<Enemy> spawnedEnemies;
    private string difficulty;
    public Wave(int number, string diff)
    {
        waveNum = number;
        difficulty = diff;
        enemyList = new List<Enemy>();
        spawnedEnemies = new List<Enemy>();
        generateEnemies();
    }

    private void generateEnemies()
    {
        int enemyCount = 10; 
        for (int i = 0; i < enemyCount; i++)
        {
            Enemy newEnemy = new Enemy("Rectangle");
            enemyList.Add(newEnemy);
        }
    }
    public void spawnEnemies()
    {
        for(int i = 0; i < enemyList.Count / 2; i++) // spawns half of generated enemies
        {
            spawnedEnemies.Add(enemyList[i]);
        }
        enemyList.RemoveRange(0, enemyList.Count / 2);
    }

    public void displayProblems()
    {
        foreach(Enemy enemy in spawnedEnemies)
        {
            // Display enemy.problem on screen
        }
    }

    public bool checkLastWave()
    {
        return waveNum >= 12;
    }

    public void checkEnemyPositions(Explosion explosion)
    {
        Sprite explosionSprite = explosion.getSprite();
        int innerExplosionX = (int)explosionSprite.getPosition().X;
        int innerExplosionY = (int)explosionSprite.getPosition().Y;
        int outerExplosionX = (int)(innerExplosionX + explosionSprite.getSize().X);
        int outerExplosionY = (int)(innerExplosionY + explosionSprite.getSize().Y);
        foreach(Enemy enemy in spawnedEnemies)
        {
            Sprite enemySprite = enemy.getProblem().getSprite();
            int enemyX = (int)enemySprite.getPosition().X;
            int enemyY = (int)enemySprite.getPosition().Y;
            
            //assuming sprites have position toward top-left corner of png
            if (enemyX < outerExplosionX && enemyX > innerExplosionX - enemySprite.getSize().X &&
                enemyY < outerExplosionY && enemyY > innerExplosionY - enemySprite.getSize().Y)
            {
                destroyEnemy(enemy);
            }
        }
    }

    public void destroyEnemy(Enemy enemy)
    {
        //more to be added
        enemy.giveMoney();
        spawnedEnemies.Remove(enemy);
        // enemy destroy()
    }
}