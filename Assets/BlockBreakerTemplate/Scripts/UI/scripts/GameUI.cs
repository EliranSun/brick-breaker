using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // public GameManager manager;		//The GameManager

    public Text scoreText; //The Text component that will display the score
    public Text livesText; //The Text component that will display the lives

    public GameObject gameOverScreen; //The game over screen game object
    public Text gameOverScoreText; //The Text component that will display the score when the player lost

    public GameObject winScreen; //The win screen game object

    private void OnNotify(string payload)
    {
        var message = payload.Split(":")[0];
        var value = payload.Split(":")[1];

        if (message == GameUpdates.ScoreUpdated.ToString())
            //Sets the scoreText to display the words 'SCORE' in bold and then the score value on a new line which is located in the GameManager class
            scoreText.text = "<b>SCORE</b>\n" + value;

        if (message == GameUpdates.LivesUpdated.ToString())
            //Sets the scoreText to display the words 'SCORE' in bold and then the score value on a new line which is located in the GameManager class
            scoreText.text = "<b>LIVES</b>\n" + value;

        if (message == GameUpdates.GameOver.ToString() || message == GameUpdates.GameWon.ToString())
        {
            scoreText.text = "";
            livesText.text = "";
        }

        if (message == GameUpdates.GameOver.ToString())
        {
            gameOverScreen.SetActive(true);
            //Sets the gameOverScoreText to display the words 'YOU ACHIEVED A SCORE OF' in bold and then the score value on a new line which is located in the GameManager class
            gameOverScoreText.text = "<b>YOU ACHIEVED A SCORE OF</b>\n" + value;
        }

        if (message == GameUpdates.GameWon.ToString())
            winScreen.SetActive(true);

        if (message == GameUpdates.GameStart.ToString())
        {
            gameOverScreen.SetActive(false);
            winScreen.SetActive(false);
            // StartGame()?
        }
    }

    // //Called when the 'TRY AGAIN' button is pressed
    // public void TryAgainButton()
    // {
    //     gameOverScreen.active = false;
    //     winScreen.active = false;
    //     manager.StartGame();
    // }

    //Called when the 'MENU' button is pressed
    public void MenuButton()
    {
        Application.LoadLevel(0); //Loads the 'Menu' scene
    }
}