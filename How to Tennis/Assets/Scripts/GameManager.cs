using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    private int playerScore;
    private int AIScore;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI AIScoreText;
    public BallController ballController;
    private string playing;
    private int AIAdvantageCount;
    private int PlayerAdvantageCount;
    public GameObject endGameCanvasPrefab;
    public Canvas endGameCanvas;
    public TextMeshProUGUI winnerText;
    public GameObject[] buttons;
    private bool gameFinished;
    public Player player;
    public TextMeshProUGUI creditsText;
    public Button adButton;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        gameFinished = false;
        playerScore = 0;
        AIScore = 0;
        AIAdvantageCount = 0;
        PlayerAdvantageCount = 0;
        playerScoreText.text = playerScore.ToString();
        AIScoreText.text = AIScore.ToString();

        PlayerData data = SaveSystem.LoadPlayer();

        creditsText.text = data.credits.ToString();
        player.credits = data.credits;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFinished == false)
        {
            if (ballController.reachedTarget() == true)
            {
                //end this round
                endRound();
            }
        }
    }

    private void addPlayerScore(int valueToAdd)
    {
        if (playerScore < 15)
        {
            //Play 15 sound effect
            playerScore += valueToAdd;
            playerScoreText.text = playerScore.ToString();
            audioManager.playScoreClip(0);
            return;
        }

        if (playerScore < 30)
        {
            //Play 30 sfx
            playerScore += valueToAdd;
            playerScoreText.text = playerScore.ToString();
            audioManager.playScoreClip(1);
            return;
        }

        if (playerScore == 30)
        {
            playerScore = 40;
            playerScoreText.text = playerScore.ToString();
            audioManager.playScoreClip(2);
            return;
        }

        if (playerScore == 40 && PlayerAdvantageCount == 0)
        {
            PlayerAdvantageCount = 1;
            playerScoreText.text = "A";

            AIAdvantageCount = 0;
            AIScoreText.text = AIScore.ToString();

            audioManager.playScoreClip(3);
            return;
        }

        if (playerScore == 40 && PlayerAdvantageCount == 1)
        {
            //Player won the game
            winnerText.text = "You won!";
            gameOver();
            return;
        }
    }

    private void addAIScore(int valueToAdd)
    {
        if (AIScore < 15)
        {
            //play 15 sfx
            AIScore += valueToAdd;
            AIScoreText.text = AIScore.ToString();
            audioManager.playScoreClip(0);
            return;
        }

        if (AIScore < 30)
        {
            //play 30 sfx
            AIScore += valueToAdd;
            AIScoreText.text = AIScore.ToString();
            audioManager.playScoreClip(1);
            return;
        }

        if (AIScore == 30)
        {
            AIScore = 40;
            AIScoreText.text = AIScore.ToString();
            audioManager.playScoreClip(2);
            return;
        }

        if (AIScore == 40 && AIAdvantageCount == 0)
        {
            AIAdvantageCount = 1;
            AIScoreText.text = "A";

            PlayerAdvantageCount = 0;
            playerScoreText.text = playerScore.ToString();

            audioManager.playScoreClip(3);
            return;
        }

        if (AIScore == 40 && AIAdvantageCount == 1)
        {
            //AI Won the game;
            winnerText.text = "AI won!";
            gameOver();
            return;
        }
    }

    public void resetScene()
    {
        ballController.Start();
    }

    private void endRound()
    {
        if (playing == "Human")
        {
            addPlayerScore(15);
            resetScene();
        }
        else
        {
            addAIScore(15);
            resetScene();
        }
    }

    public void setPlaying(string player)
    {
        if (player == "AI" || player == "Human")
        {
            playing = player;
        }
        else
        {
            Debug.LogError("ERROR: setPlaying received an incorrect parameter. The options are AI and Human");
        }
    }

    //Returns who is playing
    public string getPlaying()
    {
        return playing;
    }

    private void gameOver()
    {
        gameFinished = true;
        audioManager.playApplause();
        endGameCanvas.gameObject.SetActive(true);
        if (adButton == null)
        {
            Debug.LogError("ERROR: unable to find ad button");
        }
        adButton.gameObject.SetActive(true);
        ballController.setSpeed(0f);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        player.credits += 5;
        creditsText.text = "You have " + player.credits + " credits";
        SaveSystem.SavePlayer(player);
    }

    public bool getFinished()
    {
        return gameFinished;
    }

    public void reloadLevel()
    {
        Destroy(endGameCanvas);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void hideAdButton()
    {
        //if (adButton == null)
        //{
        //    Debug.LogError("ERROR: unable to find ad button");
        //}
        //adButton.gameObject.SetActive(false);
    }
}
