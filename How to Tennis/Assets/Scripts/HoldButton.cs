﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public NewBallController ball;
    public GameObject player;
    public MatchManager matchManager;
    public PlayerMovement playerMovement;
    private bool pointerDown;
    private float pointerDownTimer;
    public float requiredHoldTime;

    public UnityEvent onLongClick;

    [SerializeField]
    private Image fillImage;

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
                if (onLongClick != null)
                {
                    onLongClick.Invoke();
                }
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
