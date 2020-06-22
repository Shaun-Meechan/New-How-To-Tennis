using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectators : MonoBehaviour
{
    public float maxY = 1.0f;
    public float minY = 0.0f;
    private bool animDown = false;
    private bool animUp = false;
    private float verticalAnimation = 0.0f;
    private readonly float animationSpeed = 0.05f;

    void Update()
    {
        verticalAnimation = 1.0f * animationSpeed;

        if (transform.position.y <= maxY && animDown == false)
        {
            animDown = false;
            animUp = true;
        }
        else if (transform.position.y >= maxY)
        {
            animUp = false;
            animDown = true;
        }
        else if (transform.position.y <= minY)
        {
            animDown = false;
            animUp = true;
        }

        //Make the player go up and down (fake an animation)
        if (animUp == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + verticalAnimation, transform.position.z);
        }
        else if (animDown == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - verticalAnimation, transform.position.z);
        }
    }
}
