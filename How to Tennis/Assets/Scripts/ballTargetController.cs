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
            //We collided with the AI, set the relevant varibles to true
            characterColliding = true;
            AIColliding = true;
        }

        if (other.gameObject.name == "Human")
        {
            //The player collided with the ball target, set the relevant variables true
            characterColliding = true;
            playerColliding = true;
        }

        if (other.gameObject.name == "ball" && AIColliding == true)
        {
            //We have collided with the AI and ball. Hit the ball back
            AI.animateRacket("Return");
            AI.hitBall();
        }

        if (other.gameObject.name == "ball" && playerColliding == true)
        {
            //We have collided with the AI and ball. Hit the ball back
            player.animateRacket("Return");
            player.hitBall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If any trigger leaves then we need to reset all the variables.
        resetCollisionVariables();
    }

    /// <summary>
    /// Reset all collision variables the ball target is storing to false.
    /// </summary>
    public void resetCollisionVariables()
    {
        AIColliding = false;
        playerColliding = false;
        characterColliding = false;
    }

    /// <summary>
    /// Find out if the AI or player are colliding with the target.
    /// </summary>
    /// <returns>A bool representing if the player or AI are colliding with the target</returns>
    public bool getCharacterColliding()
    {
        return characterColliding;
    }
}
