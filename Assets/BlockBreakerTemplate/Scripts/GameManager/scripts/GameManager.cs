﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : ObserverSubject
{
    public int score; //The player's current score
    public int lives; //The amount of lives the player has remaining

    public GameObject paddle; //The paddle game object
    public GameObject ball; //The ball game object

    //Prefabs
    public GameObject brickPrefab; //The prefab of the Brick game object which will be spawned

    public List<GameObject> bricks = new(); //List of all the bricks currently on the screen
    public int brickCountX; //The amount of bricks that will be spawned horizontally (Odd numbers are recommended)
    public int brickCountY; //The amount of bricks that will be spawned vertically

    // The color array of the bricks. This can be modified to create different brick color patterns
    public Color[] colors;

    private void Start()
    {
        StartGame(); //Starts the game by setting values and spawning bricks
    }

    //Called when the game starts
    public void StartGame()
    {
        score = 0;
        lives = 3;
        paddle.SetActive(true);
        ball.SetActive(true);
        paddle.GetComponent<Paddle>().ResetPaddle();
        CreateBrickArray();
    }

    //Spawns the bricks and sets their colours
    public void CreateBrickArray()
    {
        var
            colorId = 0; //'colorId' is used to tell which color is currently being used on the bricks. Increased by 1 every row of bricks

        for (var y = 0; y < brickCountY; y++)
        {
            for (var x = -(brickCountX / 2); x < brickCountX / 2; x++)
            {
                var pos = new Vector3(0.8f + x * 1.6f, 1 + y * 0.4f,
                    0); //The 'pos' variable is where the brick will spawn at
                var brick = Instantiate(brickPrefab, pos,
                    Quaternion.identity); //Creates a new brick game object at the 'pos' value

                brick.GetComponent<Brick>().observers.AddListener(OnNotify);

                //Gets the 'SpriteRenderer' component of the brick object and sets the color
                brick.GetComponent<SpriteRenderer>().color = colors[colorId];
                bricks.Add(brick); //Adds the new brick object to the 'bricks' list
            }

            colorId++; //Increases the 'colorId' by 1 as a new row is about to be made

            // If the 'colorId' is equal to the 'colors' array length. This means there is no more colors left
            if (colorId == colors.Length) colorId = 0;
        }
    }

    //Called when there is no bricks left and the player has won
    public void WinGame()
    {
        paddle.SetActive(false);
        ball.SetActive(false);
        NotifyObservers(GameUpdates.GameWon);
    }

    //Called when the ball goes under the paddle and "dies"
    public void LiveLost()
    {
        lives--;

        if (lives <= 0)
        {
            //Are the lives less than 0? Are there no lives left?
            paddle.SetActive(false);
            ball.SetActive(false);
            NotifyObservers(GameUpdates.GameOver, score);

            foreach (var brick in bricks)
                Destroy(brick); //Destroy the brick

            bricks = new List<GameObject>(); //Resets the 'bricks' list variable
        }
    }

    public void TryAgain()
    {
        StartGame();
        NotifyObservers(GameUpdates.GameStart);
    }

    public void OnNotify(string payload)
    {
        var message = payload.Split("::")[0];
        var value = int.Parse(payload.Split("::")[1]);

        if (message == GameUpdates.BrickDestroyed.ToString())
        {
            score++;
            NotifyObservers(GameUpdates.ScoreChanged, score);
            var obj = EditorUtility.InstanceIDToObject(value) as GameObject;
            bricks.Remove(obj);
            if (bricks.Count == 0) WinGame();
        }

        if (message == GameUpdates.BallLost.ToString())
        {
            LiveLost();
            NotifyObservers(GameUpdates.LivesChanged, lives);
        }
    }
}