using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectators : MonoBehaviour
{
    //Variable to store the max Y the spectators can animate to
    public float maxY = 1.0f;
    //Variable to store the min Y the spectators can animate to
    public float minY = 0.0f;
    //Bool to control the animation direction
    private bool animDown = false;
    private bool animUp = false;
    //Variables to control animation speed
    private float verticalAnimation = 0.0f;
    private readonly float animationSpeed = 0.05f;

    private void Start()
    {
        //Vertical animation is used to change our Y position each frame. The value is based on 1.0 * our defined animation speed.
        verticalAnimation = 1.0f * animationSpeed;
    }

    void Update()
    {
        //Check to see if we are below our max Y position and we are able to go up.
        if (transform.position.y <= maxY && animDown == false)
        {
            animDown = false;
            animUp = true;
        }
        //Check to see if we are above our max Y position. If so flip the variables and go down.
        else if (transform.position.y >= maxY)
        {
            animUp = false;
            animDown = true;
        }
        //Check to see if we are below our min Y position. If so flip the variables and go up.
        else if (transform.position.y <= minY)
        {
            animDown = false;
            animUp = true;
        }

        //Make the specator go up and down (fake an animation)
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
