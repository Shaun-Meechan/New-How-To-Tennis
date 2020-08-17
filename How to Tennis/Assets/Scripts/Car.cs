using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    //Variable to store the min X the car can animate to
    public float minX = 0.0f;
    //Variable to store the max X the car can animate to
    public float maxX = 1.0f;
    //Variables to control the animation speed
    private float horizontalAnimation = 0.0f;
    private float animationSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Horizontal animation is used to chage the X position each frame. The value is based on 1.0 * animationSpeed   
        horizontalAnimation = 1.0f * animationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if we are over the max X
        if (transform.position.x >= maxX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x + horizontalAnimation, transform.position.y, transform.position.z);
    }
}
