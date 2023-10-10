using UnityEngine;
using UnityEngine.SceneManagement;
using BlockBreakerTemplate.Scripts.observer.scripts;
using UnityEngine.UI;

internal class GameOverScreenConfig
{
    public string title;
    public string score;
    public string playButton;
    public string menuButton;
}

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Text gameOverScreenTitle;
    [SerializeField] private Text gameOverScreenScore;
    [SerializeField] private Text gameOverScreenPlayButton;
    [SerializeField] private Text gameOverScreenMenuButton;

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    //Called when the quit button is pressed
    public void QuitButton()
    {
        Application.Quit(); //Quits the game
    }

    public void OnNotify(string payload) // TODO: Inherit from ObserverObject
    {
        var message = payload.Split("::")[0];
        var value = payload.Split("::")[1];

        print($"MESSAGE {message} VALUE {value}");

        if (message == ConfigUpdatesEnum.GameOverScreen.ToString())
            try
            {
                var gameOverScreenConfig = JsonUtility.FromJson<GameOverScreenConfig>(value);

                // gameOverScreenTitle = GameObject.Find("GameOverText").GetComponent<Text>();
                // gameOverScreenScore = GameObject.Find("GameOverScoreText").GetComponent<TextMeshProUGUI>();
                // gameOverScreenPlayButton = GameObject.Find("TryAgainButton").GetComponentInChildren<TextMeshProUGUI>();
                // gameOverScreenMenuButton = GameObject.Find("MenuButton").GetComponentInChildren<TextMeshProUGUI>();

                print($"gameOverScreenConfig {gameOverScreenConfig.title} textmeshpro {gameOverScreenTitle}");

                gameOverScreenTitle.text = gameOverScreenConfig.title;
                gameOverScreenScore.text = gameOverScreenConfig.score;
                gameOverScreenPlayButton.text = gameOverScreenConfig.playButton;
                gameOverScreenMenuButton.text = gameOverScreenConfig.menuButton;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exception {e}");
            }
    }
}