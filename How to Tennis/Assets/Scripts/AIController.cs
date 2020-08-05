using System.Collections;
using TMPro;
using UnityEngine;

public class AIController : MonoBehaviour
{
    //References to objects required to function correctly
    public NewBallController ball;
    public MatchManager matchManager;
    public GameObject targetObject;
    //References to the animators
    public Animator animatorL;
    public Animator animatorR;
    //References to the rackets
    public GameObject RacketL;
    public GameObject RacketR;
    private Rigidbody rb;
    //Used to push the AI towards a position
    private float power = 0.5f;
    //Stores the end point of the ball
    private Vector3 ballPosition;
    //Used to change the speed at which the AI animates up and down
    private float animationSpeed = 0.05f;
    //Used to animate the AI up and down
    private float verticalAnimation = 0.0f;
    //Bools for controlling our "animation"
    private bool animUp = false;
    private bool animDown = false;
    //Vector to resume movement of the AI after a pause
    Vector3 directionVector = new Vector3(0,0,0);
    //Bool to store whether the AI can move or not
    private bool canMove = true;

    private void Start()
    {
        //Get our rigidbody and store it.
        rb = GetComponent<Rigidbody>();

        //Vertical animation is used to change our Y position each frame. The value is based on 1.0 * our defined animation speed.
        verticalAnimation = 1.0f * animationSpeed;
    }

    private void Update()
    {
        //Animation controls.

        //Check to see if we are below our max Y position and we are able to go up.
        if (transform.position.y <= 2.0f && animDown == false)
        {
            animDown = false;
            animUp = true;
        }
        //Check to see if we are above our max Y position. If so flip the variables and go down.
        else if (transform.position.y >= 2.0f)
        {
            animUp = false;
            animDown = true;
        }
        //Check to see if we are below our min Y position. If so flip the variables and go up.
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
        //If we have collided with the ball target or our own target we want to stop.
        if (other.gameObject.name == "BallTarget")
        {
            resetVelocity();
            canMove = false;
        }

        if (other.gameObject.name == "AI Target")
        {
            resetVelocity();
            canMove = false;
        }
    }

    /// <summary>
    ///  Main function for the AI.
    /// </summary>
    public void wake()
    {
        //If the match has finished ignore any spurious wake up calls else start.
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }
        else
        {
            canMove = true;
            //Find out if we should go for the ball or not.
            int randomNumber = Random.Range(0, 2);

            //If the random number = 1 then we want to go to the ball
            if (randomNumber == 1)
            {
                ballPosition = ball.getEndPoint();
                //Create a direction vector from the balls end point to us, this is used later to move us.
                directionVector = new Vector3(ballPosition.x - transform.position.x, 0, ballPosition.z - transform.position.z);

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
                //Use the direction vector to push us towards the ball target.
                rb.AddForce(directionVector * power, ForceMode.Impulse);
            }
            else
            {
                canMove = true;
                //If we don't want to get the ball we want to go somewhere random on the court.
                //So we get two random numbers between the min and max points we can go
                int randomX = Random.Range(-20, 20);
                int randomZ = Random.Range(5, 20);

                //Enable our target object and move it to the point.
                targetObject.SetActive(true);
                targetObject.transform.position = new Vector3(randomX, 0, randomZ);

                //Create a direction vector between the point and us
                directionVector = new Vector3(randomX - transform.position.x, 0, randomZ - transform.position.z);

                //Use the vector to push towards the point.
                rb.AddForce(directionVector * power, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// Reset the AI's velocity
    /// </summary>
    public void resetVelocity()
    {
        //Could use Vector3.zero but this is more optimal and recommended by Unity.
        rb.velocity = new Vector3(0,0,0);
    }

    /// <summary>
    /// Function to set the AI to it's default state
    /// </summary>
    public void reset()
    {
        resetVelocity();
        directionVector = new Vector3(0, 0, 0);
        this.transform.position = new Vector3(0, 1, 32);
    }
    /// <summary>
    /// Function to handle the AI serving the ball
    /// </summary>
    public void serveBall()
    {
        //If the match is over ignore any spurious wake up calls.
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }

        //Make sure to tell the ball that this is the first serve.
        ball.setFirstServe(true);

        //Find a random point to fire the ball towards. -6 and +6 are used to make sure the target is not outside the court.
        float randomX = Random.Range(-6 + transform.position.x, transform.position.x + 6);

        //If the target is outside the bounds find a new position within the court using a different safe area bound size.
        while (randomX >= 20)
        {
            randomX = Random.Range(-5 + transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            randomX = Random.Range(0, transform.position.x + 5);
        }

        int randomZ = Random.Range(-8, -32);

        //Make sure the ball isn't moving
        //ball.resetVelocity();
        //Give the ball our position and the target to allow it to move.
        ball.Move(transform.position, new Vector3(randomX, 0.1f, randomZ));
        //Tell the match manger the AI has served.
        matchManager.ChangeState(MatchManager.matchState.AIServed);
        //Get the match managet to play a sfx.
        matchManager.playHitSound();
        //Animate the racket.
        animateRacket("Serve");
    }

    /// <summary>
    /// Function that deals with the AI hitting the ball back to the player
    /// </summary>
    public void hitBall()
    {
        //If the match is over ignore any spurious wake up calls.
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }

        //Generate a random target and make sure it is inside the court.
        float randomX = Random.Range(-6 + transform.position.x, transform.position.x + 6);

        //If the target is outside the court move it back in.
        while (randomX >= 20)
        {
            randomX = Random.Range(-5 + transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            randomX = Random.Range(0, transform.position.x + 5);
        }

        int randomZ = Random.Range(-8, -32);

        //Give the ball our transform and target so it can move
        ball.Move(transform.position, new Vector3(randomX, 0.1f, randomZ));
        //Tell the match manager the AI hit the ball.
        matchManager.ChangeState(MatchManager.matchState.AIHit);
        //Get the manager to play a sfx
        matchManager.playHitSound();
        //Set the first serve variable to false on the ball. This allows it to behave properly.
        ball.setFirstServe(false);
        //Start our reset animation to return the racket to the correct position.
        StartCoroutine(resetReturnAnimation());
    }

    /// <summary>
    /// Function to control the animation of the racket
    /// </summary>
    /// <param name="direction"></param>
    public void animateRacket(string direction)
    {
        //Check to see what direction we were given and perfrom the relevant action.
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

    /// <summary>
    /// Function to tell the AI the game has paused and should stop moving
    /// </summary>
    public void gamePaused()
    {
        resetVelocity();
    }

    /// <summary>
    /// Function to tell the AI the game is not paused anymore
    /// </summary>
    public void gameUnPaused()
    {
        if (canMove == true)
        {
            rb.AddForce(directionVector * power, ForceMode.Impulse);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Coroutine to make our reset animation play correctly.
    /// </summary>
    IEnumerator resetReturnAnimation()
    {
        yield return new WaitForSeconds(1);
        animateRacket("Returned");
    }
}
