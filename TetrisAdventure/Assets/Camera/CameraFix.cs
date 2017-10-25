using UnityEngine;
[ExecuteInEditMode]

public class CameraFix : MonoBehaviour {
    [Range(1, 4)]
    public int pixelScale = 1;

    private Camera _camera;

    // Recalculates camera to be pixel perfect
    void Update() {
        if (_camera == null) {
            _camera = GetComponent<Camera>();
            _camera.orthographic = true;
        }
        _camera.orthographicSize = Screen.height * (0.005f / pixelScale);
    }
}