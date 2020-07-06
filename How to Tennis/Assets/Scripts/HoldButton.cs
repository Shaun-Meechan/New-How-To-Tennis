using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Variable to store the ball
    public NewBallController ball;
    //Variable to store the player
    public GameObject player;
    //Variable to store the match manager
    public MatchManager matchManager;
    //Variable to store the player script
    public PlayerMovement playerMovement;
    //Bool to store if the player is holding the cursor down
    private bool pointerDown;
    //Float to represent the time the cursor has been held down for
    private float pointerDownTimer;
    //Float to represent the max amount of time the cursor can be down for.
    private float maxHoldTime = 1.0f;
    //Variable to store the power bar
    public Image fillImage;
    //Bool to find out if we should accept input
    public bool acceptInput = false;

    /// <summary>
    /// Function called when the player first clicks the cursor down
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (acceptInput == false)
        {
            return;
        }
        else
        {
            pointerDown = true;
            playerMovement.animateRacket("PrepareToServe");
        }
    }

    /// <summary>
    /// Function called when the player releases the mouse. Resets the data and fires the ball if the mouse was down for enough time
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (acceptInput == false)
        {
            return;
        }
        else
        {
            if (pointerDownTimer < 0.45f)
            {
                matchManager.incrementFailedServes();
            }
            else
            {
                ball.setCountIncreaseSpeed(pointerDownTimer);
                playerMovement.DoFirstServe();
                playerMovement.animateRacket("Serve");
            }
            Reset();
        }
    }

    private void Update()
    {
        if (acceptInput == false)
        {
            return;
        }
        else
        {
            //If the pointer is down increment the time variable
            if (pointerDown)
            {
                pointerDownTimer += Time.deltaTime;
                //If the cursor has been down for longer than the max time then stop and reset.
                if (pointerDownTimer >= maxHoldTime)
                {
                    Reset();
                }
                fillImage.fillAmount = pointerDownTimer / maxHoldTime;
            }

        }
    }

    /// <summary>
    /// Function to reset all data.
    /// </summary>
    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImage.fillAmount = 0;
    }

    /// <summary>
    /// Function to change the value of acceptInput
    /// </summary>
    /// <param name="value"></param>
    public void setAcceptInput(bool value)
    {
        acceptInput = value;
    }
}
