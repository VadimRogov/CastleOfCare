using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Скорость перемещения камеры
    public float zoomSpeed = 10f; // Скорость приближения/отдаления камеры
    private Vector3 lastMousePosition; // Последнее положение мыши

    private Camera mainCamera; // Ссылка на компонент Camera

    void Start()
    {
        // Получаем компонент Camera
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // Управление камерой с помощью правой кнопки мыши
        if (Input.GetMouseButtonDown(1))
        {
            // Запоминаем начальное положение мыши
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            // Получаем текущее положение мыши
            Vector3 currentMousePosition = Input.mousePosition;

            // Вычисляем разницу между текущим и последним положением мыши
            Vector3 delta = currentMousePosition - lastMousePosition;

            // Перемещаем камеру в зависимости от движения мыши
            transform.Translate(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);

            // Обновляем последнее положение мыши
            lastMousePosition = currentMousePosition;
        }

        // Приближение/отдаление камеры с помощью колесика мыши
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCamera.fieldOfView -= scroll * zoomSpeed;

        // Ограничиваем FOV, чтобы он не стал слишком маленьким или слишком большим
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 15f, 90f);
    }
}