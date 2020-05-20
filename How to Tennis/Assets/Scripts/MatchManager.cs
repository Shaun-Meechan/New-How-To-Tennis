using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class MatchManager : MonoBehaviour
{
	public enum matchState
	{
		PlayerServe,
		AIServe,
		PlayerHit,
		AIHit
	}

	private matchState MatchState;
	public GameObject playerServeCamera;
	public GameObject mainCamera;
	public GameObject joyStickObject;
	public Canvas playerHUDCanvas;
	public NewBallController ball;
	public GameObject player;
	public AIController AI;
	public GameObject ballTargetSprite;
	public short playerScore = 0;
	public short AIScore = 0;
	public TextMeshProUGUI playerScoreText;
	public TextMeshProUGUI AIScoreText;
	public Canvas scoreCanvas;
	private void Start()
	{
		updateScoreText();
		StartCoroutine(hideScoreCanvas());
		ChangeState(matchState.PlayerServe);
	}

	public void resetMatch()
	{
		//Reset the player and AI position
		player.transform.position = new Vector3(0, 0, -23);
		AI.transform.position = new Vector3(0, 0, 23);
		AI.resetVelocity();
		//Stop the ball from moving
		ball.resetVelocity();

		//Move the ball to whoever is serving
		if (MatchState == matchState.AIServe)
		{
			//AI is meant to serve. Move the ball to in front of the AI
			ball.setTransform(new Vector3(0, 1, 20));
		}
		else if (MatchState == matchState.PlayerServe)
		{
			//Player is meant to serve. Move the ball to in front of the Player
			ball.setTransform(new Vector3(0, 1, -30));
		}
	}

	private void ChangeCamera()
	{
		switch (MatchState)
		{
			case matchState.PlayerServe:
				playerServeCamera.SetActive(true);
				playerHUDCanvas.enabled = true;
				mainCamera.SetActive(false);
				//joyStickObject.SetActive(false);
				break;
			case matchState.AIServe:
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			default:
				break;
		}
	}

	public void ChangeState(matchState newState)
	{
		MatchState = newState;

		switch (MatchState)
		{
			case matchState.PlayerServe:
				ball.setServed(false);
				ballTargetSprite.SetActive(false);
				playerServeCamera.SetActive(true);
				playerHUDCanvas.enabled = true;
				mainCamera.SetActive(false);
				joyStickObject.SetActive(false);
				break;
			case matchState.AIServe:
				ball.setServed(false);
				ballTargetSprite.SetActive(false);
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				AI.serveBall();
				break;
			case matchState.PlayerHit:
				ball.setServed(true);
				ballTargetSprite.SetActive(true);
				AI.wake();
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			case matchState.AIHit:
				ball.setServed(true);
				ballTargetSprite.SetActive(true);
				break;
			default:
				break;
		}
	}

	public matchState GetMatchState()
	{
		return MatchState;
	}

	public void incrementPlayerScore(short value)
	{
		playerScore += value;
		updateScoreText();
		ChangeState(matchState.AIServe);
		resetMatch();
	}

	public short getPlayerScore()
	{
		return playerScore;
	}

	public void incrementAIScore(short value)
	{
		AIScore += value;
		updateScoreText();
		ChangeState(matchState.PlayerServe);
		resetMatch();
	}

	public short getAIScore()
	{
		return AIScore;
	}
	
	//Function to update our score text. Could be done in update but better to do it this way to save power
	private void updateScoreText()
	{
		playerScoreText.text = "Player Score: " + playerScore;
		AIScoreText.text = "AI Score: " + AIScore;
		showScoreCanvas();
	}

	//Function to show the score canvas
	private void showScoreCanvas()
	{
		if (scoreCanvas.enabled == false)
		{
			scoreCanvas.enabled = true;
			StartCoroutine(hideScoreCanvas());
		}
		else
		{
			return;
		}
	}

	//Hide the canvas after a set amount of time has passed.
	IEnumerator hideScoreCanvas()
	{
		yield return new WaitForSeconds(3);
		scoreCanvas.enabled = false;
	}
}
