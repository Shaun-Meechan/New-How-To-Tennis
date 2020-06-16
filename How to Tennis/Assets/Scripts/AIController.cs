using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public NewBallController ball;
    public MatchManager matchManager;
    public GameObject targetObject;
    public Animator animatorL;
    public Animator animatorR;
    public GameObject RacketL;
    public GameObject RacketR;
    private Rigidbody rb;
    private float power = 0.5f;
    private Vector3 ballPosition;
    private float animationSpeed = 0.05f;
    private float verticalAnimation = 0.0f;
    private bool animUp = false;
    private bool animDown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        verticalAnimation = 1.0f * animationSpeed;

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

        //Make the AI go up and down (fake an animation)
        if (animUp == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + verticalAnimation, transform.position.z);
        }
        else if (animDown == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - verticalAnimation, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BallTarget")
        {
            rb.velocity = new Vector3(0,0,0);
        }

        if (other.gameObject.name == "AI Target")
        {
            resetVelocity();
        }
    }

    public void wake()
    {
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }
        else
        {
            int randomNumber = Random.Range(0, 2);

            //If the random number = 1 then we want to go to the ball
            if (randomNumber == 1)
            {
                ballPosition = ball.getEndPoint();
                Vector3 directionVector = new Vector3(ballPosition.x - transform.position.x, 0, ballPosition.z - transform.position.z);

                if (directionVector.x < 0 )
                {
                    //going left, enable left racket
                    RacketL.SetActive(true);
                    RacketR.SetActive(false);
                }
                else
                {
                    //going right, enable right racket
                    RacketL.SetActive(false);
                    RacketR.SetActive(true);
                }
                rb.AddForce(directionVector * power, ForceMode.Impulse);
            }
            else
            {
                int randomX = Random.Range(-20, 20);
                int randomZ = Random.Range(5, 20);

                targetObject.SetActive(true);
                targetObject.transform.position = new Vector3(randomX, 0, randomZ);
                Vector3 directionVector = new Vector3(randomX - transform.position.x, 0, randomZ - transform.position.z);

                rb.AddForce(directionVector * power, ForceMode.Impulse);
            }
        }
    }

    public void resetVelocity()
    {
        rb.velocity = new Vector3(0,0,0);
    }

    public void serveBall()
    {
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }

        ball.setFirstServe(true);

        float randomX = Random.Range(-6 + transform.position.x, transform.position.x + 6);

        while (randomX >= 20)
        {
            randomX = Random.Range(-5 + transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            randomX = Random.Range(0, transform.position.x + 5);
        }

        int randomZ = Random.Range(-8, -32);
        ball.resetVelocity();
        ball.Move(transform.position, new Vector3(randomX, 0.1f, randomZ));
        matchManager.ChangeState(MatchManager.matchState.AIServed);
        matchManager.playHitSound();
        animateRacket("Serve");
    }

    public void hitBall()
    {
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }

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

        int randomZ = Random.Range(-8, -32);
        ball.Move(transform.position, new Vector3(randomX, 0.1f, randomZ));
        matchManager.ChangeState(MatchManager.matchState.AIHit);
        matchManager.playHitSound();
        ball.setFirstServe(false);
        StartCoroutine(resetReturnAnimation());
    }
    public void animateRacket(string direction)
    {
        if (direction == "Serve")
        {
            animatorL.SetBool("DoBackToFront", true);
            animatorL.SetBool("DoResetBack", true);
            animatorL.SetBool("DoMiddleToBack", false);

            animatorR.SetBool("DoBackToFront", true);
            animatorR.SetBool("DoResetBack", true);
            animatorR.SetBool("DoMiddleToBack", false);
        }
        else if (direction == "PrepareToServe")
        {
            animatorL.SetBool("DoMiddleToBack", true);
            animatorL.SetBool("DoBackToFront", false);
            animatorL.SetBool("DoResetBack", false);

            animatorR.SetBool("DoMiddleToBack", true);
            animatorR.SetBool("DoBackToFront", false);
            animatorR.SetBool("DoResetBack", false);
        }
        else if (direction == "AbortPrepareToServe")
        {
            animatorL.SetBool("DoBackToMiddle", true);
            animatorL.SetBool("DoMiddleToBack", false);

            animatorR.SetBool("DoBackToMiddle", true);
            animatorR.SetBool("DoMiddleToBack", false);
        }
        else if (direction == "Return")
        {
            animatorL.SetBool("DoMiddleToFront", true);

            animatorR.SetBool("DoMiddleToFront", true);
        }
        else if (direction == "Returned")
        {
            animatorL.SetBool("DoFrontToMiddle", true);
            animatorL.SetBool("DoMiddleToFront", false);

            animatorR.SetBool("DoFrontToMiddle", true);
            animatorR.SetBool("DoMiddleToFront", false);
        }
        else if (direction == "Served")
        {
            animatorL.SetBool("DoFrontToMiddle", true);
            animatorL.SetBool("DoBackToFront", false);

            animatorR.SetBool("DoFrontToMiddle", true);
            animatorR.SetBool("DoBackToFront", false);
        }
        else
        {
            Debug.LogError("ERROR: Input not valid options are 'Serve','PrepareToServe','AbortPrepareToServe','Return','Returned','Served'");
        }
    }

    IEnumerator resetReturnAnimation()
    {
        yield return new WaitForSeconds(1);
        animateRacket("Returned");
    }

}
