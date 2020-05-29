using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public NewBallController ball;
    public MatchManager matchManager;
    public AudioManager audioManager;

    float horizontalMove = 0.0f;
    float verticalMove = 0.0f;
    float verticalAnimation = 0.0f;
    private bool animUp = false;
    private bool animDown = false;
    public float speed = 1.0f;
    private float animationSpeed = 0.05f;
    void Update()
    {
        horizontalMove = joystick.Horizontal * speed;
        verticalMove = joystick.Vertical * speed;
        verticalAnimation = 1.0f * animationSpeed;
        transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z + verticalMove);

        if (transform.position.y <= 2.0f && animDown == false)
        {
            animDown = false;
            animUp = true;
        }
        else if (transform.position.y >= 2.0f)
        {
            animUp = false;
            animDown = true;
        }
        else if (transform.position.y <= 1.0)
        {
            animDown = false;
            animUp = true;
        }

        //Make the player go up and down (fake an animation)
        if (animUp == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + verticalAnimation, transform.position.z);
        }
        else if(animDown == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - verticalAnimation, transform.position.z);
        }

    }

    public void serveBall()
    {
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }

        float randomX = Random.Range(-7 + transform.position.x, transform.position.x + 7);

        while (randomX >= 20)
        {
            Debug.Log("Waffle was out of court right. Moved");
            randomX = Random.Range(-5 + transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            Debug.Log("Waffle was out of court left. Moved");
            randomX = Random.Range(0, transform.position.x + 5);
        }

        int randomZ = Random.Range(7, 20);
        ball.resetVelocity();
        ball.Move(transform.position, new Vector3(randomX, 0, randomZ));
        matchManager.ChangeState(MatchManager.matchState.PlayerHit);
        audioManager.playRandomHitClip();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "BallTarget")
        {
            Debug.LogError("Player collided with ball target");
        }

    }
}
