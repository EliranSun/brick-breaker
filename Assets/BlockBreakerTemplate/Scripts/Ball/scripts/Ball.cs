﻿using System;
using System.Collections;
using UnityEngine;

public class Ball : ObserverSubject
{
    public float speed; //The amount of units that the ball will move each second
    public float maxSpeed; //The maximum speed that the ball can travel at
    public Vector2 direction; //The Vector2 direction that the ball will move in (eg: diagonal = Vector2(1, 1))
    public Rigidbody2D rig; //The ball's Rigidbody 2D component
    public bool goingLeft; //Set to true when the ball is going left
    public bool goingDown; //Set to true xwhen the ball is going down
    public int ballSpeedIncrement; //The amount of speed the ball will increase by everytime it hits a brick

    private void Start()
    {
        // Sets the ball position to the middle of the screen
        transform.position = Vector3.zero;
        // Sets the ball's direction to go down
        direction = Vector2.down;
        // Starts the 'ResetBallWaiter' coroutine to have the ball wait 1 second before moving
        StartCoroutine(nameof(ResetBallWaiter));
    }

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        direction = Vector2.down;
        StartCoroutine(nameof(ResetBallWaiter));
    }

    private void Update()
    {
        rig.velocity =
            direction * (speed *
                         Time.deltaTime); //Sets the object's rigidbody velocity to the direction multiplied by the speed

        if (transform.position.x > 5 && !goingLeft)
        {
            //Is the ball at the right border and is not going left (heading towards the right border)
            direction = new Vector2(-direction.x,
                direction.y); //Set the ball's x direction to the opposite so that it moves away from the right border (bouncing look)
            goingLeft = true; //Sets goingLeft to true as the ball is now moving left
        }

        if (transform.position.x < -5 && goingLeft)
        {
            //Is the ball at the left border and is going left (heading towards the left border)
            direction = new Vector2(-direction.x,
                direction.y); //Set the ball's x direction to the opposite so that it moves away from the left border (bouncing look)
            goingLeft = false; //Sets goingLeft to false as the ball is now moving right
        }

        if (transform.position.y > 3 && !goingDown)
        {
            //Is the ball at the top border and not going down (heading towards the top border)
            direction = new Vector2(direction.x,
                -direction.y); //Set the ball's y direction to the opposite so that it moves away from the top border (bouncing look)
            goingDown = true; //Sets goingDown to true as the ball is now moving down
        }

        if (transform.position.y < -5) //Has the ball gone down past the paddle
            ResetBall(); // Call the 'ResetBall()' function to reset the ball in the middle of the screen
    }

    //Called when the ball needs to change direction (hit paddle, hit brick). The target parameter is the position of the object that the ball has hit
    public void SetDirection(Vector3 target)
    {
        var
            dir = new Vector2(); //Creating a variable called 'dir' which is a new vector2. This will be the new direction that will be set

        dir = transform.position -
              target; //'dir' is set to the difference between the ball position and the target. This is now the direction.
        dir.Normalize(); //Since the difference could be any size, it will be converted to a magnitude of 1.

        direction = dir; //Sets the ball's direction to the 'dir' variable

        speed += ballSpeedIncrement;

        if (speed > maxSpeed) //Is the speed of the ball more than the 'maxSpeed' value
            speed = maxSpeed;

        if (dir.x > 0) //If the x direction of the ball is more than 0, set 'goingLeft' to false
            goingLeft = false;
        if (dir.x < 0) //If the x direction of the ball is less than 0, set 'goingLeft' to true
            goingLeft = true;
        if (dir.y > 0) //If the y direction of the ball is more than 0, set 'goingDown' to false
            goingDown = false;
        if (dir.y < 0) //If the y direction of the ball is less than 0, set 'goingDown' to true
            goingDown = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Brick" || col.gameObject.tag == "Paddle")
            SetDirection(col.gameObject.transform.position);
    }

    //Called when the ball goes underneath the paddle and "dies"
    public void ResetBall()
    {
        transform.position = Vector3.zero; //Sets the ball position to the middle of the screen
        direction = Vector2.down; //Sets the ball's direction to go down
        StartCoroutine(
            "ResetBallWaiter"); //Starts the 'ResetBallWaiter' coroutine to have the ball wait 1 second before moving

        NotifyObservers(GameUpdates.BallLost);
    }

    // Called to make the ball wait a second before moving.
    // Called when the ball dies and is respawned at the middle of the screen
    private IEnumerator ResetBallWaiter()
    {
        speed = 0;
        yield return new WaitForSeconds(1.0f); //Wait 1 second
        speed = 1000;
    }
}