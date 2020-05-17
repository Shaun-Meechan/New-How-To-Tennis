using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameManager gameManager;
    private Rigidbody2D rigidBody;
    public GameObject[] playerSideTargets;
    public GameObject[] AISideTargets;
    private Vector2 directionVector;
    private float speed;
    private int targetedCourtID;
    private Transform targetObjectTransform;
    private bool humanPlay;
    // Start is called before the first frame update
    public void Start()
    {
        transform.position = new Vector2(0, 15);
        speed = 1.2f;
        rigidBody = GetComponent<Rigidbody2D>();
        gameManager.setPlaying("AI");
        resetVelocity();
        targetedCourtID = 0;
        int randomNumber = Random.Range(0, 2);
        switch (randomNumber)
        {
            case 0:
                //Fire the ball towards target 1
                createDirectionVector(playerSideTargets[0]);
                break;
            case 1:
                createDirectionVector(playerSideTargets[1]);
                break;
            case 2:
                createDirectionVector(playerSideTargets[2]);
                break;
            default:
                Debug.LogError("ERROR: Unable to fire ball!");
                break;
        }
    }

    public void createDirectionVector(GameObject gameObject)
    {
        if (gameManager.getFinished() == false)
        {
            targetObjectTransform = gameObject.transform;
            directionVector = new Vector2((gameObject.transform.position.x - transform.position.x)*speed, (gameObject.transform.position.y - transform.position.y)*speed);
            rigidBody.AddRelativeForce(directionVector, ForceMode2D.Impulse);

            if (gameManager.getPlaying() == "AI")
            {
                humanPlay = false;
            }
            else
            {
                humanPlay = true;
            }
        }
        else
        {
            return;
        }
    }

    public void resetVelocity()
    {
        rigidBody.velocity = new Vector2(0, 0);
    }

    public void setTargetCourtID(int courtID)
    {
        targetedCourtID = courtID;
    }

    public int getTargetedCourtID()
    {
        return targetedCourtID;
    }

    public bool reachedTarget()
    {
        //If the ball is going towards the AI
        if (humanPlay == true)
        {
            if (transform.position.y > targetObjectTransform.position.y)
            {
                return true;
            }
            return false;
        }
        else
        {
            if (transform.position.y < targetObjectTransform.position.y)
            {
                return true;
            }
            return false;
        }
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
