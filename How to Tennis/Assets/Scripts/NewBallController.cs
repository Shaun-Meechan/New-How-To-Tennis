using System.Collections;
using System.Collections.Generic;
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
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ERROR: Rigidbody on ball was null!");
        }

        ballTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (served == true)
        {
            if (count < 1.0f)
            {
                count += 0.5f * Time.deltaTime;

                Vector3 m1 = Vector3.Lerp(startPoint, middlePoint, count);
                Vector3 m2 = Vector3.Lerp(middlePoint, endPoint, count);
                transform.position = Vector3.Lerp(m1, m2, count);
            }

            if (count >= 1.0f)
            {
                count = 0.0f;
            }
        }
    }
    public void Move(Vector3 startPositon, Vector3 endPosition)
    {
        startPoint = startPositon;
        endPoint = endPosition;
        middlePoint = new Vector3((endPosition.x - startPositon.x) / 2, 10, 0);
        targetSprite.transform.position = new Vector3(endPosition.x, 0.1f ,endPosition.z);
    }

    public void resetVelocity()
    {
            rb.velocity = Vector3.zero;
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
