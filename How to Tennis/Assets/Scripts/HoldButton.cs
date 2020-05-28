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

    public UnityEvent onLongClick;

    private float power = 0.0f;

    [SerializeField]
    private Image fillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerMovement.serveBall();
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
