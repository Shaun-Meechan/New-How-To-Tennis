using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public NewBallController ball;
    public GameObject player;
    public MatchManager matchManager;
    public PlayerMovement playerMovement;
    private bool pointerDown;
    private float pointerDownTimer;
    public float requiredHoldTime;

    [SerializeField]
    public Image fillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        playerMovement.animateRacket("PrepareToServe");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerDownTimer < 0.45f)
        {
            Debug.Log("Speed was less than 0.45, fail?");
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

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                Reset();
            }
            fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
        }
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImage.fillAmount = 0;
    }
}
