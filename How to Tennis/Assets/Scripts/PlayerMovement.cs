using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public NewBallController ball;
    public MiddlePoint middlePoint;
    public MatchManager matchManager;

    float horizontalMove = 0.0f;
    float verticalMove = 0.0f;
    public float speed = 1.0f;
    void Update()
    {
        horizontalMove = joystick.Horizontal * speed;
        verticalMove = joystick.Vertical * speed;

        transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z + verticalMove);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ball")
        {
            if (ball.getServed() == true)
            {
                //Hit the ball again
                ball.resetVelocity();

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
                middlePoint.setTarget(new Vector3(randomX, 0, randomZ));
                ball.setEndPoint(new Vector3(randomX, 0, randomZ));
                middlePoint.moveToMiddle(transform.position, new Vector3(randomX, 0, randomZ));
                ball.Move(transform.position, middlePoint.getPosition(), 1.1f);
                matchManager.ChangeState(MatchManager.matchState.PlayerHit);

            }
        }
    }
}
