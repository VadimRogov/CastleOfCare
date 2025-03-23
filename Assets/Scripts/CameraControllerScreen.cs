using UnityEngine;

public class CameraControllerScreen : MonoBehaviour
{
    public float moveSpeed = 10f; // Скорость перемещения камеры
    public float rotationSpeed = 100f; // Скорость вращения камеры
    public float zoomSpeed = 10f; // Скорость изменения FOV
    public float minFOV = 10f; // Минимальное поле зрения
    public float maxFOV = 100f; // Максимальное поле зрения

    private float _currentFOV = 60f; // Текущее поле зрения
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _currentFOV = _camera.fieldOfView;
    }

    void Update()
    {
        // Перемещение камеры
        MoveCamera();

        // Вращение камеры
        RotateCamera();

        // Управление шириной и высотой захвата
        AdjustCaptureSize();
    }

    void MoveCamera()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // A и D
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // W и S

        // Перемещение камеры
        transform.Translate(new Vector3(moveX, 0, moveZ));
    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(1)) // Вращение при зажатой правой кнопки
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Вращение камеры
            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.left, mouseY, Space.Self);
        }
    }

    void AdjustCaptureSize()
    {
        // Управление шириной захвата (Q и E)
        if (Input.GetKey(KeyCode.Q))
        {
            _currentFOV = Mathf.Clamp(_currentFOV - zoomSpeed * Time.deltaTime, minFOV, maxFOV);
        }
        if (Input.GetKey(KeyCode.E))
        {
            _currentFOV = Mathf.Clamp(_currentFOV + zoomSpeed * Time.deltaTime, minFOV, maxFOV);
        }

        // Управление высотой захвата (R и F)
        if (Input.GetKey(KeyCode.R))
        {
            _camera.rect = new Rect(_camera.rect.x, _camera.rect.y, _camera.rect.width, Mathf.Clamp(_camera.rect.height + 0.01f, 0.1f, 1f));
        }
        if (Input.GetKey(KeyCode.F))
        {
            _camera.rect = new Rect(_camera.rect.x, _camera.rect.y, _camera.rect.width, Mathf.Clamp(_camera.rect.height - 0.01f, 0.1f, 1f));
        }

        // Применяем изменения FOV
        _camera.fieldOfView = _currentFOV;
    }
}
