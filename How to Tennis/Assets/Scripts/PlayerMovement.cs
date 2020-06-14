using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    public NewBallController ball;
    public MatchManager matchManager;
    public AudioManager audioManager;
    public Animator animatorL;
    public Animator animatorR;
    public GameObject RacketLeft;
    public GameObject RacketRight;
    public SkinObject currentSkin;

    float horizontalMove = 0.0f;
    float verticalMove = 0.0f;
    float verticalAnimation = 0.0f;
    private bool animUp = false;
    private bool animDown = false;
    public float speed = 1.0f;
    private readonly float animationSpeed = 0.05f;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = currentSkin.skin;
    }
    void Update()
    {
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

    public void hitBall()
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

        int randomZ = Random.Range(7, 28);
        ball.resetVelocity();
        ball.Move(transform.position, new Vector3(randomX, 0, randomZ));
        matchManager.ChangeState(MatchManager.matchState.PlayerHit);
        audioManager.playRandomHitClip();
        ball.setFirstServe(false);
        StartCoroutine(resetReturnAnimation());
    }

    public void DoFirstServe()
    {
        ball.setFirstServe(true);

        float randomX = Random.Range(-7 + transform.position.x, transform.position.x + 7);

        while (randomX >= 20)
        {
            randomX = Random.Range(-5 + transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            randomX = Random.Range(0, transform.position.x + 5);
        }

        int randomZ = Random.Range(7, 28);
        ball.resetVelocity();
        ball.Move(transform.position, new Vector3(randomX, 0, randomZ));
        matchManager.ChangeState(MatchManager.matchState.PlayerServed);
        audioManager.playRandomHitClip();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "BallTarget")
        {
            Debug.LogError("Player collided with ball target");
        }
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
