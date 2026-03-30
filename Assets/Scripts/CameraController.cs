using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private Vector2 screenOffset = new Vector2(3f, 2f);

    private InputAction _lookAction;
    private Vector3 _initialOffset;
    private float _initialPitch;
    private float _yaw;

    private void Awake()
    {
        _lookAction = InputSystem.actions.FindAction("Player/Look");
        _initialOffset = transform.localPosition;
        _initialPitch = transform.localEulerAngles.x;
    }

    private void LateUpdate()
    {
        float lookX = _lookAction.ReadValue<Vector2>().x;
        _yaw += lookX * sensitivity;

        Quaternion orbit = Quaternion.Euler(0f, _yaw, 0f);
        transform.localPosition = orbit * _initialOffset;
        transform.localRotation = Quaternion.Euler(
            _initialPitch - screenOffset.y,
            _yaw + screenOffset.x,
            0f);
    }
}