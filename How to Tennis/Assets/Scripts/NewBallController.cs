using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBallController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform ballTransform;
    private Vector3 endPoint;
    public GameObject targetSprite;
    private bool served = false;
    public MatchManager matchManager;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ERROR: Rigidbody on ball was null!");
        }

        ballTransform = GetComponent<Transform>();
    }
    public void Move(Vector3 startPositon, Vector3 endPosition, float power)
    {
        Vector3 force = new Vector3(endPosition.x - startPositon.x, endPosition.y - startPositon.y, endPosition.z - startPositon.z);

        rb.AddForce(force * power, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Detect if the ball hit the court
        if (collision.gameObject.name == "Court")
        {
            Debug.Log("Ball hit the court.");
            if (served == true)
            {
                //Ball was served and hit the floor. Someone earned a point
                if (matchManager.GetMatchState() == MatchManager.matchState.PlayerHit)
                {
                    //The player was the last person to hit the ball. Give a point to the player.
                    matchManager.incrementPlayerScore(1);
                }
                else if (matchManager.GetMatchState() == MatchManager.matchState.AIHit)
                {
                    //The AI was the last person to hit the ball. Give a point to the AI
                    matchManager.incrementAIScore(1);
                }
            }
        }
    }

    public void resetVelocity()
    {
            rb.velocity = Vector3.zero;
    }

    public void setEndPoint(Vector3 transform)
    {
        endPoint = transform;
        targetSprite.transform.position = transform;
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
}
