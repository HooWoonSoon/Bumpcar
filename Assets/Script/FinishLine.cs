using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI starsText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI totalPointsText;

    public Player_Controller playerController;
    public Transform playerTransform;

    public GameObject scorePanel; 
    public Button continueButton; 

    public GameObject buttonPanel; 
    public Button retryButton;
    public Button stageSelectionButton;
    public Button mainMenuButton;

    public AudioSource bgmAudioSource; 
    public AudioClip victoryBGM; 

    private bool hasTriggered = false;
    private float maxTimePoints = 100000f;
    private float timePenaltyPerSecond = 100f;
    private int starPoints = 1000;
    private int deathPenalty = 5000;

    private float startTime;

    private void Start()
    {
        victoryText.gameObject.SetActive(false);
        starsText.gameObject.SetActive(false);
        durationText.gameObject.SetActive(false);
        deathCountText.gameObject.SetActive(false);
        totalPointsText.gameObject.SetActive(false);
        scorePanel.SetActive(false);
        buttonPanel.SetActive(false);

        continueButton.onClick.AddListener(ShowButtonPanel); 
        retryButton.onClick.AddListener(RetryLevel);
        stageSelectionButton.onClick.AddListener(SelectStage);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        startTime = Time.time;
    }

    private void Update()
    {
        if (!hasTriggered && Vector3.Distance(transform.position, playerTransform.position) <= 5f)
        {
            TriggerFinish();
        }
    }

    private void TriggerFinish()
    {
        int totalCheckpoints = FindObjectsOfType<Checkpoint>().Length; 

        if (!Checkpoint.PlayerClearedAllCheckpoints(totalCheckpoints))
        {
            Debug.Log("Player has not cleared all checkpoints!");
            return;
        }

        hasTriggered = true;

        if (bgmAudioSource != null && victoryBGM != null)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = victoryBGM;
            bgmAudioSource.loop = false;
            bgmAudioSource.Play();
        }

        victoryText.gameObject.SetActive(true);
        playerController.canMove = false;
        Time.timeScale = 0;

        StartCoroutine(ShowVictoryText());
    }

    private IEnumerator ShowVictoryText()
    {
        yield return new WaitForSecondsRealtime(3);

        victoryText.gameObject.SetActive(false);
        CalculateAndDisplayPoints();
    }

    private void CalculateAndDisplayPoints()
    {
        float timeTaken = Time.time - startTime;

        int playerIndex = 0;
        if (playerIndex < Game_Data.Instance.characters.Count)
        {
            var character = Game_Data.Instance.characters[playerIndex];
            character.Timer(timeTaken);

            int starsEarned = character.StarCount;
            float gameDuration = character.gameDuration;
            int deathCount = character.PlayerSpawnCount;

            int starsPoints = starsEarned * starPoints;
            int durationPoints = Mathf.Max(0, (int)(maxTimePoints - (gameDuration * timePenaltyPerSecond)));
            int deathPoints = deathCount * deathPenalty;
            int totalPoints = starsPoints + durationPoints - deathPoints;

            scorePanel.SetActive(true); 
            StartCoroutine(DisplayPoints(starsPoints, durationPoints, deathPoints, totalPoints));
        }
    }

    private IEnumerator DisplayPoints(int starsPoints, int durationPoints, int deathPoints, int totalPoints)
    {
        starsText.text = $"Stars Earned: {starsPoints} points";
        starsText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        durationText.text = $"Duration: {durationPoints} points";
        durationText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        deathCountText.text = $"Death Count: -{deathPoints} points";
        deathCountText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        totalPointsText.text = $"Total Points: {totalPoints} points";
        totalPointsText.gameObject.SetActive(true);

        continueButton.gameObject.SetActive(true); 
    }

    private void ShowButtonPanel()
    {
        scorePanel.SetActive(false); 
        buttonPanel.SetActive(true); 
    }

    private void RetryLevel()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void SelectStage()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameSetup");
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
