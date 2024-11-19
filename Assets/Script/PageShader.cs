using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PageShader : MonoBehaviour
{
    private Material pageMaterial;
    public Material pageInstance { get; set; }
    void Start()
    {
        pageMaterial = GetComponent<Renderer>().material;
        pageInstance = new Material(pageMaterial);
        GetComponent<Renderer>().material = pageInstance;
    }

    void Update()
    {
        if (pageInstance.GetFloat("_Angle") > 65)
        {
            Debug.Log("Angle is greater than 90");
        }
    }
}
