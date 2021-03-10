using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFeedback : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer planeMesh;

    [SerializeField]
    private Material blueMat, redMat;

    public void Hide()
    {
        planeMesh.gameObject.SetActive(false);
    }

    public void Show(int wantedColor)
    {
        switch(wantedColor)
        {
            case 1:
                planeMesh.material = blueMat;
                break;
            case 2:
                planeMesh.material = redMat;
                break;
        }
        planeMesh.gameObject.SetActive(true);
    }
}
