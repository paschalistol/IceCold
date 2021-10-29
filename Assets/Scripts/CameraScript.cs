using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Options options;
    Camera myCamera;
    [SerializeField] private CanvasScaler[] scalers;
    private void Start()
    {
        myCamera = GetComponent<Camera>();
        DifferentAspectRatioSize(5);
        options.toggleFullscreen += FullScreenCamera;
    }
    private void DifferentAspectRatioSize(float size)
    {
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        if (targetAspect > windowAspect)
        {
            
            myCamera.orthographicSize /= scaleHeight;
            foreach (var scaler in scalers)
            {
                scaler.matchWidthOrHeight = 0;
            }
        }
        else
        {
            foreach (var scaler in scalers)
            {
                scaler.matchWidthOrHeight = 1;
            }
        }
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
            DifferentAspectRatioSize(4.1f - 0.07f);
        }
    }

}
