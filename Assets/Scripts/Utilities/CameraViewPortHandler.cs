using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewPortHandler : MonoBehaviour
{
    public enum Constraint
    {
        Landscape,
        Portrait
    }

    #region INITIALIZE COMPONENTS
    public Color wireColor = Color.white;
    public float unit = 1;
    public Constraint cons = Constraint.Portrait;
    public new UnityEngine.Camera cam;
    public static CameraViewPortHandler Instance;

    private float width, height;
    private Vector3 bottomLeft, bottomRight, bottomCenter, topLeft, topRight, topCenter, middleLeft, middleRight, middleCenter;
    #endregion

    #region FUNCTIONS

    private void ComputeCameraResolution()
    {
        float x_left, x_right, y_top, y_bottom;
        if (cons == Constraint.Landscape)
            cam.orthographicSize = 1.0f / cam.aspect * unit / 2.0f;
        else
            cam.orthographicSize = unit / 2.0f;

        height = cam.orthographicSize * 2.0f;
        width = height * cam.aspect;

        float x_camera = cam.transform.position.x, y_camera = cam.transform.position.y;
        x_left = x_camera - width / 2.0f;
        x_right = x_camera + width / 2.0f;
        y_top = y_camera + height / 2.0f;
        y_bottom = y_camera - height / 2.0f;

        bottomLeft = new Vector3(x_left, y_bottom, 0);
        bottomRight = new Vector3(x_right, y_bottom, 0);
        bottomCenter = new Vector3(x_camera, y_bottom, 0);

        topLeft = new Vector3(x_left, y_top, 0);
        topRight = new Vector3(x_right, y_top, 0);
        topCenter = new Vector3(x_camera, y_top, 0);

        middleLeft = new Vector3(x_left, y_camera, 0);
        middleRight = new Vector3(x_right, y_camera, 0);
        middleCenter = new Vector3(x_camera, y_camera, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = wireColor;

        Matrix4x4 tmp = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        if (cam.orthographic)
        {
            float spread = cam.farClipPlane - cam.nearClipPlane;
            float center = (cam.farClipPlane + cam.nearClipPlane) * 0.5f;
            Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(cam.orthographicSize * 2f * cam.aspect, cam.orthographicSize * 2, spread));
        }
        else
        {
            Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        }
        Gizmos.matrix = tmp;
    }

    #endregion

    private void Awake()
    {
        cam = GetComponent<UnityEngine.Camera>();
        Instance = this;
        ComputeCameraResolution();
    }

    private void Update()
    {
#if UNITY_EDITOR
        ComputeCameraResolution();
#endif
    }
}
