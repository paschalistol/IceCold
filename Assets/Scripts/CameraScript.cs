using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Options options;
    Camera camera;
    private void Start()
    {
        float targetAspect = 9.0f / 16.0f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        camera = GetComponent<Camera>();
        camera.orthographicSize /= scaleHeight;
        RectTransform rect = GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, camera.orthographicSize - 5, -10);
        options.toggleFullscreen += FullScreenCamera;
        
    }

    private void FullScreenCamera()
    {
        if (PlayerPrefs.GetInt("fullScreen") == 0)
        {
            camera.orthographicSize = 5;
        }
        else
        {
            camera.orthographicSize = 4.18f;
        }
    }

}
