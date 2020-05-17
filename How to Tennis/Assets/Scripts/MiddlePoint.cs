using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePoint : MonoBehaviour
{
    Vector3 target;
    public NewBallController ball;
    public void setTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public Vector3 getTarget()
    {
        return target;
    }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "ball")
        {
            ball.Move(this.transform.position, target, 0.25f);
        }
    }

    public void moveToMiddle(Vector3 startPoint, Vector3 endPoint)
    {
        //Debug.Log("Input was: " + startPoint + " " + endPoint);
        //Debug.Log("Target was: " + target);
        this.transform.position = new Vector3((endPoint.x + startPoint.x) / 2, 7, 0);
    }
}
