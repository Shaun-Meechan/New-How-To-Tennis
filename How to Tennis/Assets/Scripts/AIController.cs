using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public NewBallController ball;
    public MiddlePoint middlePoint;
    public MatchManager matchManager;

    private Rigidbody rb;

    private float power = 0.5f;
    private Vector3 ballPosition;
    private bool onTarget = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BallTarget")
        {
            rb.velocity = Vector3.zero;
            onTarget = true;
        }
        else if (other.gameObject.name == "ball" && onTarget == true)
        {
            ball.resetVelocity();

            float randomX = Random.Range(-6 + transform.position.x, transform.position.x + 6);

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

            int randomZ = Random.Range(-7, -20);
            middlePoint.setTarget(new Vector3(randomX, 0, randomZ));
            ball.setEndPoint(new Vector3(randomX, 0, randomZ));
            middlePoint.moveToMiddle(transform.position, new Vector3(randomX, 0, randomZ));
            ball.Move(transform.position, middlePoint.getPosition(), 1.1f);
            matchManager.ChangeState(MatchManager.matchState.AIHit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BallTarget")
        {
            onTarget = false;
        }
    }


    public void wake()
    {
        int randomNumber = Random.Range(0, 2);

        //If the random number = 1 then we want to go to the ball
        if (randomNumber == 1)
        {
            Debug.Log("We are waking up the AI");
            ballPosition = ball.getEndPoint();
            Vector3 directionVector = new Vector3(ballPosition.x - transform.position.x, 0, ballPosition.z - transform.position.z);

            rb.AddForce(directionVector * power, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("We are not waking the AI");

            int randomX = Random.Range(-20, 20);
            int randomZ = Random.Range(5, 20);
            Vector3 directionVector = new Vector3(randomX, 0, randomZ);

            rb.AddForce(directionVector * power, ForceMode.Impulse);
        }
    }
}
