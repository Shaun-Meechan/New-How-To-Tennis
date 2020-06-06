using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballTargetController : MonoBehaviour
{
    public AIController AI;
    public PlayerMovement player;
    public MatchManager matchManager;
    private bool characterColliding = false;
    private bool AIColliding = false;
    private bool playerColliding = false;
    private void OnTriggerEnter(Collider other)
    {
        //Did we collide with the AI
        if (other.gameObject.name == "AI")
        {
            characterColliding = true;
            AIColliding = true;
            //We collided with the AI
        }

        if (other.gameObject.name == "Human")
        {
            //The player collided with the ball target
            characterColliding = true;
            playerColliding = true;
        }

        if (other.gameObject.name == "ball" && AIColliding == true)
        {
            //We have collided with the AI and ball. Hit the ball back
            AI.hitBall();
        }

        if (other.gameObject.name == "ball" && playerColliding == true)
        {
            //We have collided with the AI and ball. Hit the ball back
            player.hitBall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If any trigger leaves then we need to reset all the variables.
        resetCollisionVariables();
    }

    public void resetCollisionVariables()
    {
        AIColliding = false;
        playerColliding = false;
        characterColliding = false;
    }

    public bool getCharacterColliding()
    {
        return characterColliding;
    }
}
