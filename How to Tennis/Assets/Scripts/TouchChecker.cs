using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchChecker : MonoBehaviour
{
    public BallController ballController;
    private bool collidingWithBall;
    public AIManager AIManager;
    public GameManager gameManager;
    public int ID;
    public AudioManager audioManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            collidingWithBall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            collidingWithBall = false;
        }
    }

    public void hitBall()
    {
        if (collidingWithBall == true)
        {
            gameManager.setPlaying("Human");
            ballController.resetVelocity();
            int randomNumber = Random.Range(0, 3);
            switch (randomNumber)
            {
                case 0:
                    ballController.createDirectionVector(ballController.AISideTargets[0]);
                    ballController.setTargetCourtID(0);
                    break;
                case 1:
                    ballController.createDirectionVector(ballController.AISideTargets[1]);
                    ballController.setTargetCourtID(1);
                    break;
                case 2:
                    ballController.createDirectionVector(ballController.AISideTargets[2]);
                    ballController.setTargetCourtID(2);
                    break;
                default:
                    break;
            }
            AIManager.Play(ID);
            audioManager.playHitClip(randomNumber);
        }
        if (collidingWithBall == false)
        {
            Debug.Log("NOTE: Unable to hit ball as ball was not colliding with this side.");
        }
    }
}
