using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private const string MOUSE_SCROLL = "Mouse ScrollWheel";
    private const float sensitivity = 20.0f;
    private const float damping     = 10.0f;
    private const float minFOV      = 35.0f;
    private const float maxFOV      = 65.0f;
    private Camera _camera;
    private float distance;

    void Awake ()
    {
        this._camera = GetComponent<Camera>();
        this.distance = this._camera.fieldOfView;
    }

    void Update ()
    {
        distance -= Input.GetAxis(MOUSE_SCROLL) * sensitivity;
        distance = Mathf.Clamp(this.distance, minFOV, maxFOV);
        this._camera.fieldOfView = Mathf.Lerp(
            this._camera.fieldOfView, this.distance, damping * Time.deltaTime);
    }
}
