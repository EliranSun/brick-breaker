using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed; // The amount of units the paddle will move a second
    public float minX; // The minimum x position that the paddle can move to
    public float maxX; // The maximum x position that the paddle can move to
    public bool canMove; // Determines whether or not the paddle can move
    public Rigidbody2D rig; // The paddle's rigidbody 2D component

    internal void Update()
    {
        if (canMove)
        {
            //Is the paddle able to move?
            if (Input.GetKey(KeyCode.LeftArrow)) //Is the left arrow key currently being pressed
                rig.velocity =
                    new Vector2(-1 * speed * Time.deltaTime, 0); //Set the paddle's rigidbody velocity to move left
            else if (Input.GetKey(KeyCode.RightArrow)) //Is the right arrow key currently being pressed
                rig.velocity =
                    new Vector2(1 * speed * Time.deltaTime, 0); //Set the paddle's rigidbody velocity to move right
            else
                rig.velocity = Vector2.zero; //If those keys arn't being pressed, set the velocity to 0

            transform.position =
                new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y,
                    0); //Clamps the position so that it doesn't go below the 'minX' or past the 'maxX' values
        }
    }

    //Called when the paddle needs to be reset to the middle of the screen
    public void ResetPaddle()
    {
        transform.position = new Vector3(0, transform.position.y, 0); //Sets the paddle's x position to 0
    }
}