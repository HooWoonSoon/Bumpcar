using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PageShader : MonoBehaviour
{
    private Material pageMaterial;
    [SerializeField] private Texture2D frontTexture;
    [SerializeField] private Texture2D backTexture;
    private PageController pageController;
    [SerializeField] private List<GameObject> uis;
    private int pageIndex;
    public Material pageInstance { get; set; }
    void Start()
    {
        pageMaterial = GetComponent<Renderer>().material;
        pageInstance = new Material(pageMaterial);
        GetComponent<Renderer>().material = pageInstance;

        pageInstance.SetTexture("_FrontTex",frontTexture);
        pageInstance.SetTexture("_BackTex", backTexture);
        pageController = FindObjectOfType<PageController>();

        pageIndex = pageController.images.FindIndex(image => image == this);
    }

    void Update()
    {
        int controlPage = pageController.currentPage;
        foreach (GameObject ui in uis)
        {
            if (pageInstance.GetFloat("_Angle") <= 0 && pageIndex == controlPage)
            {
                ui.SetActive(true);
            }
            else if (pageIndex != controlPage)
            {
                ui.SetActive(false);
            }
            else if (pageInstance.GetFloat("_Angle") > 65)
            {
                ui.SetActive(false);
            }
            
        }
        
    }
}
