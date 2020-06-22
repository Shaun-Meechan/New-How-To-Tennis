using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class NewBallController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform ballTransform;
    public GameObject targetSprite;
    public GameObject firstTargetSprite;
    private bool served = false;
    public MatchManager matchManager;
    public ballTargetController ballTargetController;
    private float count = 0.0f;
    private Vector3 startPoint = new Vector3(0,0,0);
    private Vector3 endPoint = new Vector3(0, 0, 0);
    private Vector3 middlePoint = new Vector3(0, 0, 0);
    private Vector3 m1 = new Vector3(0, 0, 0);
    private Vector3 m2 = new Vector3(0, 0, 0);
    private Collider objectCollider;
    private bool firstServe = false;
    private float countIncreaseSpeed = 1.5f;
    Vector3 firstEndPoint = new Vector3(0, 0, 0);
    Vector3 FirstBounceToEnd = new Vector3(0, 0, 0);
    private bool doFirstPartOfBounce = false;
    private bool doSecondPartOfBounce = false;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        ballTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (firstServe == true)
        {
            if (count < 1.0f)
            {
                count += countIncreaseSpeed * Time.deltaTime;

                if (doFirstPartOfBounce == true)
                {
                    m1 = Vector3.Lerp(startPoint, middlePoint, count);
                    m2 = Vector3.Lerp(middlePoint, firstEndPoint, count);
                    transform.position = Vector3.Lerp(m1, m2, count);
                }

                if (doSecondPartOfBounce == true)
                {
                    m1 = Vector3.Lerp(firstEndPoint, FirstBounceToEnd, count);
                    m2 = Vector3.Lerp(FirstBounceToEnd, endPoint, count);
                    transform.position = Vector3.Lerp(m1, m2, count);
                }

                if (transform.position == endPoint)
                {
                    firstServe = false;
                }

            }
        }
        if (served == true)
        {
            if (count < 1.0f)
            {
                count += 0.7f * Time.deltaTime;

                m1 = Vector3.Lerp(startPoint, middlePoint, count);
                m2 = Vector3.Lerp(middlePoint, endPoint, count);
                transform.position = Vector3.Lerp(m1, m2, count);
            }
        }
    }

    private void doFirstMove()
    {
        //Calculate first end point, used for the bounce
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
    public void Move(Vector3 startPositon, Vector3 endPosition)
    {
        //Turn off the collider so we don't accidently collide with our own side.
        objectCollider.enabled = false;
        resetVelocity();
        count = 0.0f;
        startPoint = startPositon;
        endPoint = new Vector3(endPosition.x, endPosition.y, endPosition.z);
        middlePoint = new Vector3((endPosition.x - startPositon.x)/2, 10, 0);
        targetSprite.transform.position = new Vector3(endPosition.x, 0.1f ,endPosition.z);
        StartCoroutine(colliderTimer());

        if (firstServe == true)
        {
            doFirstMove();
            doFirstPartOfBounce = true;
            doSecondPartOfBounce = false;
        }
    }

    public void resetVelocity()
    {
            rb.velocity = new Vector3(0,0,0);
    }

    public Vector3 getEndPoint()
    {
        return endPoint;
    }
    //Use this for locking the player
    public bool getServed()
    {
        return served;
    }
    //Set served to true or false
    //Should be used after serving the ball
    public void setServed(bool value)
    {
        served = value;
    }

    public void setFirstServe(bool value)
    {
        firstServe = value;
    }

    public void setTransform(Vector3 newTransform)
    {
        ballTransform.position = newTransform;
    }

    public void resetCounter()
    {
        count = 0.0f;
    }
    private IEnumerator colliderTimer()
    {
        yield return new WaitForSeconds(1);
        objectCollider.enabled = true;
    }

    /// <summary>
    /// Set the speed at which count increase in the update function. A value higher than 1 is undesirable.
    /// </summary>
    /// <param name="value"></param>
    public void setCountIncreaseSpeed(float value)
    {
        countIncreaseSpeed = value;
    }
}
