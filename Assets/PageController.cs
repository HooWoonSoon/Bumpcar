using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public class PageController : MonoBehaviour
{
    public List<PageShader> images;
    private Vector3 initialMousePosition;
    private Vector3 currentMousePostion;
    private float distanceMove;
    private bool isFlipingFoward;
    [SerializeField] private float releaseduration = 300f;
    [SerializeField] private float sensitivity = 2f;
    public int currentPage { get; private set; }
    private float currentAngle;
   

    private void Start()
    {

    }

    private void Update()
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

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        if (Input.GetKeyDown(KeyCode.Mouse0))
            initialMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentMousePostion = Camera.main.ScreenToWorldPoint(mousePosition);
            distanceMove = (initialMousePosition.x - currentMousePostion.x) * sensitivity;

            if (distanceMove > 0 && currentPage < images.Count - 1)
            {
                images[currentPage].pageInstance.SetFloat("_Angle", Mathf.Lerp(0, 180, Mathf.Abs(distanceMove)));
                isFlipingFoward = true;
            }
            else if (distanceMove < 0 && currentPage > 0)
            {
                images[currentPage - 1].pageInstance.SetFloat("_Angle", Mathf.Lerp(180, 0, Mathf.Abs(distanceMove)));
                isFlipingFoward = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (isFlipingFoward && currentPage < images.Count - 1)
            {
                currentAngle = images[currentPage].pageInstance.GetFloat("_Angle");
                if (currentAngle >= 65)
                    StartCoroutine(SmoothTransition(currentAngle, 180));
                else
                    StartCoroutine(SmoothTransition(currentAngle, 0));
            }
            else if (!isFlipingFoward && currentPage > 0)
            {
                currentAngle = images[currentPage - 1].pageInstance.GetFloat("_Angle");
                if (currentAngle <= 115)
                    StartCoroutine(SmoothTransition(currentAngle, 0));
                else
                    StartCoroutine(SmoothTransition(currentAngle, 180));
            }
        }
    }

    private IEnumerator SmoothTransition(float startAngle, float endAngle)
    {
        float currentAngle = startAngle;

        while (!Mathf.Approximately(currentAngle, endAngle))
        {
            currentAngle = Mathf.MoveTowards(currentAngle, endAngle, releaseduration * Time.deltaTime);
            images[currentPage].pageInstance.SetFloat("_Angle", currentAngle);
            yield return null;
        }

        if (isFlipingFoward)
        {
            images[currentPage].pageInstance.SetFloat("_Angle", endAngle);
            if (endAngle == 180 && currentPage < images.Count - 1)
            {
                currentPage++;
                images[currentPage].pageInstance.SetFloat("_Angle", 0);
            }
        }
        else
        {
            images[currentPage - 1].pageInstance.SetFloat("_Angle", endAngle);
            if (endAngle == 0)
            {
                currentPage--;
                images[currentPage].pageInstance.SetFloat("_Angle", 0);
            }
        }
    }

}
