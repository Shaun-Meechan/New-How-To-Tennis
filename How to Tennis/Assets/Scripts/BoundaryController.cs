using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Human")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.resetVelocity();
        }
    }
}
