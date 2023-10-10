using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text scoreText; //The Text component that will display the score
    public Text livesText; //The Text component that will display the lives

    public GameObject gameOverScreen; //The game over screen game object
    public Text gameOverScoreText; //The Text component that will display the score when the player lost

    public GameObject winScreen; //The win screen game object

    public void OnNotify(string payload)
    {
        var message = payload.Split("::")[0];
        var value = payload.Split("::")[1];

        print($"MESSAGE {message} VALUE {value}");

        if (message == GameUpdates.ScoreChanged.ToString())
            //Sets the scoreText to display the words 'SCORE' in bold and then the score value on a new line which is located in the GameManager class
            scoreText.text = $"<b>SCORE</b>\n {value}";

        if (message == GameUpdates.LivesChanged.ToString())
            //Sets the scoreText to display the words 'SCORE' in bold and then the score value on a new line which is located in the GameManager class
            livesText.text = $"<b>LIVES</b>\n {value}";

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
            scoreText.text = "<b>SCORE</b> \n 0";
            livesText.text = "<b>LIVES</b> \n 3";
            gameOverScreen.SetActive(false);
            winScreen.SetActive(false);
        }
    }

    //Called when the 'MENU' button is pressed
    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}