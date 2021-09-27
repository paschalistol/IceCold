using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Options options;
    Camera myCamera;
    private void Start()
    {
        DifferentAspectRatioSize(5);
        options.toggleFullscreen += FullScreenCamera;
    }
    private void DifferentAspectRatioSize(float size)
    {
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        myCamera = GetComponent<Camera>();
        myCamera.orthographicSize /= scaleHeight;
        RectTransform rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, myCamera.orthographicSize - size, -10);
    }
    private void FullScreenCamera()
    {
        if (PlayerPrefs.GetInt("fullScreen") == 0)
        {
            myCamera.orthographicSize = 5;
            DifferentAspectRatioSize(5);
        }
        else
        {
            myCamera.orthographicSize = 4.1f;
            DifferentAspectRatioSize(4.1f);
        }
    }

}
