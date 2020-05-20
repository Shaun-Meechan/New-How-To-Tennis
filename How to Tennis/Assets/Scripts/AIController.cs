using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public NewBallController ball;
    public MatchManager matchManager;
    public GameObject targetObject;
    private Rigidbody rb;

    private float power = 0.5f;
    private Vector3 ballPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BallTarget")
        {
            rb.velocity = Vector3.zero;
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

            targetObject.SetActive(true);
            targetObject.transform.position = new Vector3(randomX, 0, randomZ);
            Vector3 directionVector = new Vector3(randomX - transform.position.x, 0, randomZ - transform.position.z);

            rb.AddForce(directionVector * power, ForceMode.Impulse);
        }
    }

    public void resetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public void serveBall()
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
        ball.Move(transform.position, new Vector3(randomX, 0.1f, randomZ));
        ball.resetVelocity();
        matchManager.ChangeState(MatchManager.matchState.AIHit);
    }
}
