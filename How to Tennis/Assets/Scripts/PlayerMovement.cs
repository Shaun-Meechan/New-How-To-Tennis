using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variable to store the joystick object
    public Joystick joystick;
    //Variable to store the ball
    public NewBallController ball;
    //Variable to store the match manager
    public MatchManager matchManager;
    //Variables to store the animators
    public Animator animatorL;
    public Animator animatorR;
    //Variables to store the rackets
    public GameObject RacketLeft;
    public GameObject RacketRight;
    //Variables to store movement data
    float horizontalMove = 0.0f;
    float verticalMove = 0.0f;
    float verticalAnimation = 0.0f;
    //Variables to store if the player should animate up or down
    private bool animUp = false;
    private bool animDown = false;
    //Float to store movement speed
    public float speed = 1.0f;
    //Float to store animation speed
    private readonly float animationSpeed = 0.05f;
    //Variable to store the rigidbody
    private Rigidbody rb;

    private void Start()
    {
        //Vertical animation is used to change our Y position each frame. The value is based on 1.0 * our defined animation speed.
        verticalAnimation = 1.0f * animationSpeed;
        rb = gameObject.GetComponent<Rigidbody>();
        joystick.setPlayer = this;
    }

    void Update()
    {
        //Get data from the joystick and use that to move the player
        if (joystick.Horizontal < 0)
        {
            //Joystick is pulling left, enable left racket
            RacketLeft.SetActive(true);
            RacketRight.SetActive(false);
        }
        else if(joystick.Horizontal > 0)
        {
            //Joystick is pulling right, enbale right racket
            RacketLeft.SetActive(false);
            RacketRight.SetActive(true);
        }
        horizontalMove = joystick.Horizontal * speed;
        verticalMove = joystick.Vertical * speed;
        transform.position = new Vector3(transform.position.x + horizontalMove, transform.position.y, transform.position.z + verticalMove);
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

    /// <summary>
    /// Function to reset the player's velocity to a vector 3 zero
    /// </summary>
    public void resetVelocity()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Sets the current skin
    /// </summary>
    public void setSkin(Material skinMaterial)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = skinMaterial;
    }

    /// <summary>
    /// Function to return the ball
    /// </summary>
    public void hitBall()
    {
        //If the match is over ignore spurious wake up calls
        if (matchManager.getMatchFinished() == true)
        {
            return;
        }

        //Generate a random target and make sure it is inside the court.
        float randomX = Random.Range(-7 + transform.position.x, transform.position.x + 7);

        //If the target is outside the court move it back in.
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

        int randomZ = Random.Range(7, 28);

        //Give the ball our transform and target so it can move
        ball.Move(transform.position, new Vector3(randomX, 0, randomZ));
        //Tell the match manager the player has hit the ball.
        matchManager.ChangeState(MatchManager.matchState.PlayerHit);
        //Get the manager to play a sfx
        matchManager.playHitSound();
        //Set the first serve variable to false on the ball. This allows it to behave properly.
        ball.setFirstServe(false);
        //Start our reset animation to return the racket to the correct position.
        StartCoroutine(resetReturnAnimation());
    }

    /// <summary>
    /// Function to serve the ball
    /// </summary>
    public void DoFirstServe()
    {
        //Set first serve on the ball to true
        ball.setFirstServe(true);

        //Generate a target to hit the ball towards
        float randomX = Random.Range(-7 + transform.position.x, transform.position.x + 7);

        //If the target is out of the court move it in
        while (randomX >= 20)
        {
            randomX = Random.Range(-5 + transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            randomX = Random.Range(0, transform.position.x + 5);
        }

        int randomZ = Random.Range(7, 28);

        //Make sure the ball isn't moving
        ball.resetVelocity();
        //Give the ball the player's position and the target position to allow the ball to move
        ball.Move(transform.position, new Vector3(randomX, 0, randomZ));
        //Tell the match manager the player has served the ball
        matchManager.ChangeState(MatchManager.matchState.PlayerServed);
        //Get the match manager to play a sfx
        matchManager.playHitSound();
    }

    /// <summary>
    /// Function to animate the racket
    /// </summary>
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
    /// Coroutine to reset the return animation
    /// </summary>
    /// <returns></returns>
    IEnumerator resetReturnAnimation()
    {
        yield return new WaitForSeconds(1);
        animateRacket("Returned");
    }
}
