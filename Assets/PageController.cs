using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour
{
    public List<PageShader> images;
    private bool flipFoward;
    [SerializeField] private float releaseduration = 300f;
    public int currentPage { get; private set; }
    private float currentAngle;
    private bool isFlipping;

    private void Start()
    {
        UpdatePagePositions();
    }

    public void FlipFoward()
    {
        if (!isFlipping && currentPage < images.Count - 1)
        {
            flipFoward = true;
            isFlipping = true;
            StartCoroutine(SmoothTransition(0, 180));
        }
    }

    public void FlipBack()
    {
        if (!isFlipping && currentPage > 0)
        {
            flipFoward = false;
            isFlipping = true;
            StartCoroutine(SmoothTransition(180, 0));
        }
    }

    private IEnumerator SmoothTransition(float startAngle, float endAngle)
    {
        float currentAngle = startAngle;

        while (!Mathf.Approximately(currentAngle, endAngle))
        {
            currentAngle = Mathf.MoveTowards(currentAngle, endAngle, releaseduration * Time.deltaTime);
            if (flipFoward && currentPage < images.Count - 1)
            {
                images[currentPage].pageInstance.SetFloat("_Angle", currentAngle);
                if (currentAngle == 180)
                {
                    currentPage++;
                    images[currentPage].pageInstance.SetFloat("_Angle", 0);
                    UpdatePagePositions();
                }
            }
            else if (!flipFoward && currentPage > 0)
            {
                images[currentPage - 1].pageInstance.SetFloat("_Angle", currentAngle);
                if (currentAngle == 0)
                {
                    currentPage--;
                    images[currentPage].pageInstance.SetFloat("_Angle", 0);
                    UpdatePagePositions();
                }
            }
            yield return null;
        }

        isFlipping = false;
    }

    private void UpdatePagePositions()
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (i == currentPage)
                images[i].transform.localPosition = new Vector3(0, 0, 0);
            else if (i < currentPage)
                images[i].transform.localPosition = new Vector3(0, 0, -0.01f * (currentPage - i)); // page front change to back
            else
                images[i].transform.localPosition = new Vector3(0, 0, 0.01f * (currentPage - i)); // senquene front to back
        }
    }
}
