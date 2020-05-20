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
        float randomX = Random.Range(-7 + player.transform.position.x, player.transform.position.x + 7);

        while (randomX >= 20)
        {
            Debug.Log("Waffle was out of court right. Moved");
            randomX = Random.Range(-5 + player.transform.position.x, 0);

        }
        while (randomX <= -20)
        {
            Debug.Log("Waffle was out of court left. Moved");
            randomX = Random.Range(0, player.transform.position.x + 5);
        }

        int randomZ = Random.Range(8, 32);
        ball.Move(player.transform.position, new Vector3(randomX,0.1f,randomZ));
        matchManager.ChangeState(MatchManager.matchState.PlayerHit);

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
