using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public BallController ballController;
    public GameManager gameManager;

    public void Play(int ID)
    {
        //Handles all the AI data
        float delayTime = 0f;
        switch (ID)
        {
            case 0:
                delayTime = 0.6f;
                break;
            case 1:
                delayTime = 0.6f;
                break;
            case 2:
                delayTime = 0.8f;
                break;
            default:
                Debug.LogError("ERROR: Unable to set delay time");
                break;
        }
        StartCoroutine(playRoutine(delayTime));
        return;
    }

    IEnumerator playRoutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //Find out if the AI should play, if 1 then yes if 0 then no.
        int shouldGo = Random.Range(0, 10);
        if (shouldGo >= 1)
        {
            Debug.Log("AI is playing");
            gameManager.setPlaying("AI");
            //Find out what court has been targeted;
            int targetedCourt = ballController.getTargetedCourtID();
            int targetCourt = Random.Range(0, 3);

            switch (targetedCourt)
            {
                case 0:
                    //left court
                    Debug.Log("Ball is going for the left court");
                    //send our "character" to that court               
                    break;
                case 1:
                    //Right court
                    Debug.Log("Ball is going for the right court");
                    //send character there
                    break;
                case 2:
                    //Back court
                    Debug.Log("Ball is going for the back court");
                    //send character there
                    break;
                default:
                    Debug.LogError("Unable to find targeted court!");
                    break;
            }
            //Hit the ball back
            ballController.resetVelocity();
            Debug.Log("AI is hitting back!");
            switch (targetCourt)
            {
                case 0:
                    ballController.createDirectionVector(ballController.playerSideTargets[0]);
                    break;
                case 1:
                    ballController.createDirectionVector(ballController.playerSideTargets[1]);
                    break;
                case 2:
                    ballController.createDirectionVector(ballController.playerSideTargets[2]);
                    break;
                default:
                    Debug.LogError("ERROR: Unable to fire ball");
                    break;
            }
        }
        else
        {
            Debug.Log("AI is not playing");
        }
    }
}
