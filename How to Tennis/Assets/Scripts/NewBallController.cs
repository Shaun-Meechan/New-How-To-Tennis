using System.Collections;
using UnityEngine;

public class NewBallController : MonoBehaviour
{
    //Variable to store the rigid body
    private Rigidbody rb;
    //Variable to store the transform
    private Transform ballTransform;
    //Variable to store the target sprite
    public GameObject targetSprite;
    //Variable to store the first target sprite
    public GameObject firstTargetSprite;
    //Variable to represent if the ball has been served or not
    private bool served = false;
    //Variable to store the match manager
    public MatchManager matchManager;
    //Variable to store the ball Target controller
    public ballTargetController ballTargetController;
    //Variable used to control the speed of lerp animations
    private float count = 0.0f;
    //Vectors 3s used for lerping between points
    private Vector3 startPoint = new Vector3(0,0,0);
    private Vector3 endPoint = new Vector3(0, 0, 0);
    private Vector3 middlePoint = new Vector3(0, 0, 0);
    private Vector3 m1 = new Vector3(0, 0, 0);
    private Vector3 m2 = new Vector3(0, 0, 0);
    //Variable to store the collider
    private Collider objectCollider;
    //Bool to represent if it is the first time the ball has been served in a round
    private bool firstServe = false;
    //Float to represent the increased lerp animation speed
    private float countIncreaseSpeed = 1.0f;
    //Vector 3s for the first serve bounce lerp
    Vector3 firstEndPoint = new Vector3(0, 0, 0);
    Vector3 FirstBounceToEnd = new Vector3(0, 0, 0);
    //Bools to see if the ball needs to do a bounce, used in serving
    private bool doFirstPartOfBounce = false;
    private bool doSecondPartOfBounce = false;
    public void Start()
    {
        //Get the rigid body and store it
        rb = GetComponent<Rigidbody>();
        //Get the collider and store it
        objectCollider = GetComponent<Collider>();
        //Get the transform and store it
        ballTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (matchManager.getPaused() == true)
        {
            return;
        }
        //If it is the first serve then the ball needs to do a bounce for it to be legal
        if (firstServe == true)
        {
            if (count < 1.0f)
            {
                count += countIncreaseSpeed * Time.deltaTime;

                if (doFirstPartOfBounce == true)
                {
                    //Lerp between where the ball was hit from and where it needs to bounce from
                    m1 = Vector3.Lerp(startPoint, middlePoint, count);
                    m2 = Vector3.Lerp(middlePoint, firstEndPoint, count);
                    transform.position = Vector3.Lerp(m1, m2, count);
                }

                if (doSecondPartOfBounce == true)
                {
                    //Lerp from the bounce point to the end point
                    setCountIncreaseSpeed(1.1f);
                    m1 = Vector3.Lerp(firstEndPoint, FirstBounceToEnd, count);
                    m2 = Vector3.Lerp(FirstBounceToEnd, endPoint, count);
                    transform.position = Vector3.Lerp(m1, m2, count);
                }

                //If the ball reaches the end point it has performed the first serve
                if (transform.position == endPoint)
                {
                    firstServe = false;
                }

            }
        }
        //If the ball has already been served it doesn't need to bounce to be legal
        if (served == true)
        {
            if (count < 1.0f)
            {
                count += 0.7f * Time.deltaTime;

                //Lerp between where the ball was hit from and it's end point at a set speed
                m1 = Vector3.Lerp(startPoint, middlePoint, count);
                m2 = Vector3.Lerp(middlePoint, endPoint, count);
                transform.position = Vector3.Lerp(m1, m2, count);
            }
        }
    }

    /// <summary>
    /// Calculate first end point, used for the bounce
    /// </summary>
    private void doFirstMove()
    {
        firstEndPoint = new Vector3(endPoint.x - middlePoint.x, 0.5f, (endPoint.z - middlePoint.z)/2);
        FirstBounceToEnd = new Vector3(endPoint.x - firstEndPoint.x, 3, endPoint.z - firstEndPoint.z);
        firstTargetSprite.transform.position = firstEndPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BallTarget")
        {
            //The ball has hit the ball target
            if (ballTargetController.getCharacterColliding() == false)
            {
                if (matchManager.GetMatchState() == MatchManager.matchState.PlayerHit || matchManager.GetMatchState() == MatchManager.matchState.PlayerServed)
                {
                    //Player last hit the ball and now it has hit the court. Give the player a point
                    matchManager.incrementPlayerScore(1);
                }
                else if (matchManager.GetMatchState() == MatchManager.matchState.AIHit || matchManager.GetMatchState() == MatchManager.matchState.AIServed)
                {
                    //The AI was last to hit the ball and it has now hit the court. Give the AI a point
                    matchManager.incrementAIScore(1);
                }
                else
                {
                    Debug.LogError("ERROR: Unexpected match state! Match state is: " + matchManager.GetMatchState());
                }
            }
        }
        if (other.gameObject.name == "ballFirstTarget")
        {
            //Tell the update function todo the section part of the movement
            doFirstPartOfBounce = false;

            count = 0.0f;   

            doSecondPartOfBounce = true;
        }
    }

    /// <summary>
    /// Function to setup the data need to lerp the ball from a start point to an end point
    /// </summary>
    public void Move(Vector3 startPositon, Vector3 endPosition)
    {
        //Turn off the collider so we don't accidently collide with our own side.
        objectCollider.enabled = false;
        //Make sure the ball isn't moving or animating
        resetVelocity();
        count = 0.0f;
        //Setup the local variables
        startPoint = startPositon;
        endPoint = new Vector3(endPosition.x, endPosition.y, endPosition.z);
        middlePoint = new Vector3((endPosition.x - startPositon.x)/2, 10, 0);
        //Move the target sprite to the end point
        targetSprite.transform.position = new Vector3(endPosition.x, 0.2f ,endPosition.z);
        //Re-enable the collider once the ball is off the ground
        StartCoroutine(colliderTimer());

        //If this is the first serve set the varibles accordingly
        if (firstServe == true)
        {
            doFirstMove();
            doFirstPartOfBounce = true;
            doSecondPartOfBounce = false;
        }
    }

    /// <summary>
    /// Function to reset the ball's velocity. Could use Vector3.zero but this is more optimal and recommended by Unity.
    /// </summary>
    public void resetVelocity()
    {
            rb.velocity = new Vector3(0,0,0);
    }

    /// <summary>
    /// Function to retrun the end point of the ball
    /// </summary>
    /// <returns></returns>
    public Vector3 getEndPoint()
    {
        return endPoint;
    }

    /// <summary>
    /// Function to return if the ball has been served or not
    /// </summary>
    /// <returns></returns>
    public bool getServed()
    {
        return served;
    }
    
    /// <summary>
    /// Function to set the value of served to a passed in value
    /// </summary>
    public void setServed(bool value)
    {
        served = value;
    }

    /// <summary>
    /// Function to set first served to a passed in value
    /// </summary>
    public void setFirstServe(bool value)
    {
        firstServe = value;
    }

    /// <summary>
    /// Function to set the balls transform
    /// </summary>
    public void setTransform(Vector3 newTransform)
    {
        ballTransform.position = newTransform;
    }

    /// <summary>
    /// Function to reset the lerps animation counter
    /// </summary>
    public void resetCounter()
    {
        count = 0.0f;
    }

    /// <summary>
    /// Coroutine to re-enable the collider after 1 second.
    /// </summary>
    /// <returns></returns>
    private IEnumerator colliderTimer()
    {
        yield return new WaitForSeconds(1);
        objectCollider.enabled = true;
    }

    /// <summary>
    /// Set the speed at which count increase in the update function.
    /// </summary>
    /// <param name="value"></param>
    public void setCountIncreaseSpeed(float value)
    {
        countIncreaseSpeed = value;
    }

}
