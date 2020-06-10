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
		PlayerServed,
		AIServe,
		AIServed,
		PlayerHit,
		AIHit,
		Finished
	}

	private matchState MatchState;
	private bool matchFinished = false;
	public GameObject playerServeCamera;
	public GameObject mainCamera;
	public GameObject joyStickObject;
	public Canvas playerHUDCanvas;
	public NewBallController ball;
	public GameObject human;
	public AIController AI;
	public GameObject ballTargetSprite;
	public short playerScore = 0;
	public short AIScore = 0;
	public TextMeshProUGUI playerScoreText;
	public TextMeshProUGUI AIScoreText;
	public TextMeshProUGUI endMatchText;
	public TextMeshProUGUI creditsText;
	public Canvas scoreCanvas;
	public Canvas endGameCanvas;
	public AudioManager audioManager;
	public Player player;
	private void Start()
	{
		updateScoreText();
		StartCoroutine(hideScoreCanvas());
		ChangeState(matchState.PlayerServe);

		PlayerData data = SaveSystem.LoadPlayer();
		player.credits = data.credits;
	}

	public void resetMatch()
	{
		//Reset the player and AI position
		human.transform.position = new Vector3(0, 1, -32);
		AI.transform.position = new Vector3(0, 1, 32);
		AI.resetVelocity();
		//Stop the ball from moving
		ball.resetCounter();
		ball.resetVelocity();
		ball.setFirstServe(false);
		//Wait so that everything is back in place before we change our state
		StartCoroutine(wait());
		//Move the ball to whoever is serving
		switch (MatchState)
		{
			case matchState.PlayerServe:
				//Player is meant to serve. Move the ball to in front of the Player
				ball.setTransform(new Vector3(human.transform.position.x, human.transform.position.y, human.transform.position.z + 2));
				break;
			case matchState.AIServe:
				//AI is meant to serve. Move the ball to in front of the AI
				ball.setTransform(new Vector3(AI.transform.position.x, AI.transform.position.y, AI.transform.position.z + 2));
				AI.serveBall();
			break;
			default:
				Debug.LogError("ERROR: Unable to reset ball position");
				break;
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
        if (playerScore == 6 || AIScore == 6)
        {
			MatchState = matchState.Finished;
        }
        else
        {
			MatchState = newState;
        }

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
			case matchState.PlayerServed:
				ball.setServed(false);
				ballTargetSprite.SetActive(true);
				AI.wake();
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			case matchState.AIServe:
				ball.setServed(false);
				ballTargetSprite.SetActive(false);
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			case matchState.AIServed:
				ball.setServed(false);
				ballTargetSprite.SetActive(true);
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
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
			case matchState.Finished:
				audioManager.playApplause();
				matchFinished = true;
				ballTargetSprite.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(false);
				ball.gameObject.SetActive(false);
				player.credits += 5;
				SaveSystem.SavePlayer(player);
				creditsText.text = "You have " + player.credits + " credits.";
				break;
			default:
				Debug.LogError("ERROR: State was changed to an invalid state.");
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
		if (playerScore > 5)
		{
			//Player has more than 5 points, they won, end game
			ChangeState(matchState.Finished);
			endGameCanvas.enabled = true;
			endMatchText.text = "You won!";
		}
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
		if (AIScore > 5)
		{
			//AI has more than 5 points, they won, end game
			ChangeState(matchState.Finished);
			endGameCanvas.enabled = true;
			endMatchText.text = "AI won!";
		}
		ChangeState(matchState.PlayerServe);
		updateScoreText();
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

	IEnumerator wait()
    {
		yield return new WaitForSeconds(1);
    }

	public bool getMatchFinished()
    {
		return matchFinished;
    }

	//REMOVE ME 
	public void reloadLevel()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
