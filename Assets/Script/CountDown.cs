using UnityEngine;
using TMPro;
using System.Collections;

public class RaceCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject player;
    public GameObject[] aiCars;
    public float countdownTime = 4f;

    private Player_Controller playerController;
    private Ai_Controller[] aiControllers;

    void Start()
    {
        playerController = player.GetComponent<Player_Controller>();

        aiControllers = new Ai_Controller[aiCars.Length];
        for (int i = 0; i < aiCars.Length; i++)
        {
            aiControllers[i] = aiCars[i].GetComponent<Ai_Controller>();
        }

        if (playerController != null)
        {
            playerController.canMove = false;
        }

        foreach (var aiController in aiControllers)
        {
            if (aiController != null)
            {
                aiController.enabled = false;
            }
        }

        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        countdownText.text = "Ready?";
        yield return new WaitForSeconds(1f);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        if (playerController != null)
        {
            playerController.canMove = true;
        }

        foreach (var aiController in aiControllers)
        {
            if (aiController != null)
            {
                aiController.enabled = true;
            }
        }

        countdownText.text = "";
    }
}
