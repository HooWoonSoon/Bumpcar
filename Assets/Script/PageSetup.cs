using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PageSetup : MonoBehaviour
{
    [SerializeField] private int aiCount;
    [SerializeField] private GameObject mascot;
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private TextMeshProUGUI progress;

    public void PressButtonLevel1()
    {
        int currentLevelindex = 2;
        StartCoroutine(LoadLevel(currentLevelindex));
    }

    public void PressButtonLevel2()
    {
        int currentLevelindex = 3;
        StartCoroutine(LoadLevel(currentLevelindex));
    }

    public void PressButtonLevel3()
    {
        int currentLevelindex = 4;
        StartCoroutine(LoadLevel(currentLevelindex));
    }

    private IEnumerator LoadLevel(int currentLevelindex)
    {
        loadScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(currentLevelindex);

        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            mascot.transform.Rotate(Vector3.forward);

            progress.color = Color.white;
            progress.text = (operation.progress * 100).ToString() + "%";

            if (operation.progress >= 0.9f)
            {
                progress.text = "Press Any Key to continue";

                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
