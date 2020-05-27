using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBallController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform ballTransform;
    public GameObject targetSprite;
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
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        ballTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (served == true)
        {
            if (count < 1.0f)
            {
                count += 0.7f * Time.deltaTime;

                m1 = Vector3.Lerp(startPoint, middlePoint, count);
                m2 = Vector3.Lerp(middlePoint, endPoint, count);
                transform.position = Vector3.Lerp(m1, m2, count);
            }

            if (count >= 1.0f)
            {
                count = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Court")
        {
            //Ball hit the court, was it served?
            if (served == true)
            {
                //Ball was served
                if (matchManager.GetMatchState() == MatchManager.matchState.PlayerHit)
                {
                    //Player last hit the ball and now it has hit the court. Give the player a point
                    matchManager.incrementPlayerScore(1);
                }
                else if (matchManager.GetMatchState() == MatchManager.matchState.AIHit)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BallTarget")
        {
            Debug.Log("Ball hit the target");
            //The ball has hit the ball target
            if (ballTargetController.getCharacterColliding() == false)
            {
                Debug.Log("Ball hit the target but no one received it");
                if (matchManager.GetMatchState() == MatchManager.matchState.PlayerHit)
                {
                    //Player last hit the ball and now it has hit the court. Give the player a point
                    matchManager.incrementPlayerScore(1);
                }
                else if (matchManager.GetMatchState() == MatchManager.matchState.AIHit)
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
    }
    public void Move(Vector3 startPositon, Vector3 endPosition)
    {
        //Turn off the collider so we don't accidently collide with our own side.
        objectCollider.enabled = false;
        resetVelocity();
        count = 0.0f;
        startPoint = startPositon;
        endPoint = new Vector3(endPosition.x, endPosition.y + 4, endPosition.z);
        middlePoint = new Vector3((endPosition.x - startPositon.x) / 2, 10, 0);
        targetSprite.transform.position = new Vector3(endPosition.x, 0.1f ,endPosition.z);
        StartCoroutine(colliderTimer());
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

    public void setTransform(Vector3 newTransform)
    {
        ballTransform.position = newTransform;
    }

    private IEnumerator colliderTimer()
    {
        yield return new WaitForSeconds(1);
        objectCollider.enabled = true;
    }
}
