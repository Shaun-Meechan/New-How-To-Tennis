﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.VR;

public class MatchManager : MonoBehaviour
{
	//Enum to store the current state of the game
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

	//Variable storing the current state of the game
	private matchState MatchState;
	//Variable storing if the game has finished or not
	private bool matchFinished = false;
	//Variable to store the failed attempts to serve by the player
	private int failedServes = 0;
	//Variable to store the player's serving camera
	public GameObject playerServeCamera;
	//Variable to store the normal camera
	public GameObject mainCamera;
	//Variable storing the joystick
	public GameObject joyStickObject;
	//Variable storing the player's HUD, only visible during serves?
	public Canvas playerHUDCanvas;
	//Variable storing the ball
	public NewBallController ball;
	//Variable storing the player object
	public GameObject human;
	//Variable storing the AI
	public AIController AI;
	//Variable storing the player script
	public PlayerMovement playerObject;
	//Variable to store the joystick
	public Joystick joystick;
	//Variable storing the ball's target sprite
	public GameObject ballTargetSprite;
	//Variables to store the scores
	public short playerScore = 0;
	public short AIScore = 0;
	//Variables to store score texts and text shown at the end of a match
	public TextMeshProUGUI playerScoreText;
	public TextMeshProUGUI AIScoreText;
	public TextMeshProUGUI endMatchText;
	public TextMeshProUGUI creditsText;
	//Varibles to store the canvas's used during gameplay
	public Canvas scoreCanvas;
	public GameObject endGameCanvas;
	//Variable to store the audio manager
	public AudioManager audioManager;
	//Variable to store the player data
	public Player player;
	//Variable to store the skin loader
	public SkinLoader skinLoader;
	//Varriable to store the pause menu
	public GameObject pauseMenu;

	private void Start()
	{
		//Set the score text to defaults
		updateScoreText();
		//Start a couroutine to hide the canvas
		StartCoroutine(hideScoreCanvas());
		//Change our state to allow the player to serve
		ChangeState(matchState.PlayerServe);

		//Load the player data
		PlayerData data = SaveSystem.LoadPlayer();
		player.credits = data.credits;
		Material tempSkin = skinLoader.getSkinMaterial(data.skinID);
		//Set the players skin to their selected skin
		playerObject.setSkin(tempSkin);
	}

	/// <summary>
	/// Update will check for user input.
	/// </summary>

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
				//Player pressed the back button. Show the pause menu
				pauseMenu.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Function that resets the match to allow another round to be player
    /// </summary>
    public void resetMatch()
	{
		//Reset the player, joystick and AI position
		joystick.resetHorizontalAndVerticalValue();
		playerObject.resetVelocity();
		human.transform.position = new Vector3(0, 1, -32);
		AI.transform.position = new Vector3(0, 1, 32);
		AI.resetVelocity();
		//Stop the ball from moving or animating
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

	/// <summary>
	/// Function to chnage the active camera
	/// </summary>
	private void ChangeCamera()
	{
		switch (MatchState)
		{
			case matchState.PlayerServe:
				playerServeCamera.SetActive(true);
				playerHUDCanvas.enabled = true;
				mainCamera.SetActive(false);
				joyStickObject.SetActive(false);
				break;
			case matchState.PlayerServed:
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			case matchState.PlayerHit:
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			case matchState.AIServe:
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			case matchState.AIServed:
				mainCamera.SetActive(true);
				playerServeCamera.SetActive(false);
				playerHUDCanvas.enabled = false;
				joyStickObject.SetActive(true);
				break;
			default:
				Debug.LogError("ERROR: Unable to change camera as game is in the incorrect state. Should be PlayerServe or AIServe. It is currently" + MatchState);
				break;
		}
	}

	/// <summary>
	/// Function to change the state of the match
	/// </summary>
	/// <param name="newState">State to change the match to</param>
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
				ChangeCamera();
				break;		
			case matchState.PlayerServed:
				ball.setServed(false);
				ballTargetSprite.SetActive(true);
				AI.wake();
				ChangeCamera();
				break;
			case matchState.AIServe:
				ball.setServed(false);
				ballTargetSprite.SetActive(false);
				ChangeCamera();
				break;
			case matchState.AIServed:
				ball.setServed(false);
				ballTargetSprite.SetActive(true);
				ChangeCamera();
				break;
			case matchState.PlayerHit:
				ball.setServed(true);
				ballTargetSprite.SetActive(true);
				AI.wake();
				ChangeCamera();
				break;
			case matchState.AIHit:
				ball.setServed(true);
				ballTargetSprite.SetActive(true);
				break;
			case matchState.Finished:
				audioManager.playApplause();
				matchFinished = true;
				endGameCanvas.SetActive(true);
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

	/// <summary>
	/// Function to return the current state of the match
	/// </summary>
	/// <returns></returns>
	public matchState GetMatchState()
	{
		return MatchState;
	}

	/// <summary>
	/// Function to increment the player's score by a value
	/// </summary>
	public void incrementPlayerScore(short value)
	{
		playerScore += value;
		if (playerScore > 5)
		{
			//Player has more than 5 points, they won, end game
			ChangeState(matchState.Finished);
			endMatchText.text = "You won!";
		}
		updateScoreText();
		ChangeState(matchState.AIServe);
		resetMatch();
	}

	/// <summary>
	/// Function to return the player's score
	/// </summary>
	public short getPlayerScore()
	{
		return playerScore;
	}

	/// <summary>
	/// Function to increment the AI's score by a value
	/// </summary>
	public void incrementAIScore(short value)
	{
		AIScore += value;
		if (AIScore > 5)
		{
			//AI has more than 5 points, they won, end game
			ChangeState(matchState.Finished);
			endMatchText.text = "AI won!";
		}
		ChangeState(matchState.PlayerServe);
		updateScoreText();
		resetMatch();
	}

	/// <summary>
	/// Returns the AI's score
	/// </summary>
	public short getAIScore()
	{
		return AIScore;
	}
	
	/// <summary>
	/// Updates score text. Should be called after a score has been incremented.
	/// </summary>
	private void updateScoreText()
	{
		playerScoreText.text = "Player Score: " + playerScore;
		AIScoreText.text = "AI Score: " + AIScore;
		showScoreCanvas();
	}

	/// <summary>
	/// Function to show the score canvas
	/// </summary>
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

	/// <summary>
	///	Hide the canvas after a set amount of time has passed.
	/// </summary>
	IEnumerator hideScoreCanvas()
	{
		yield return new WaitForSeconds(3);
		scoreCanvas.enabled = false;
	}

	/// <summary>
	/// Coroutine to make a thread wait 1 second
	/// </summary>
	IEnumerator wait()
    {
		yield return new WaitForSeconds(1);
    }

	/// <summary>
	/// Function to retrun a bool representing if the match is finished or not.
	/// </summary>
	public bool getMatchFinished()
    {
		return matchFinished;
    }

	/// <summary>
	/// Count how many times the player failed to serve the ball. If they fail twice give a point to the AI and let it serve.
	/// </summary>
	public void incrementFailedServes()
    {
		failedServes += 1;
        if (failedServes > 2)
        {
			//Player failed to serve twice. Give AI point and let it serve
			failedServes = 0;
			incrementAIScore(1);
			ChangeState(matchState.AIServe);
			resetMatch();
        }
        else
        {
			//First or second failed serve. Give the player another chance
			ChangeState(matchState.PlayerServe);
        }
    }

	/// <summary>
	/// Function to tell the audio manager to play a random hit clip.
	/// </summary>
	public void playHitSound()
    {
		audioManager.playRandomHitClip();
    }
}
