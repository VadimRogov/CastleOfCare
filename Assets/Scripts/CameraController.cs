using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // Скорость перемещения камеры
    public float zoomSpeed = 10f; // Скорость приближения/отдаления камеры
    public float minX = -10f; // Минимальная координата X
    public float maxX = 10f; // Максимальная координата X
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

            // Вычисляем новую позицию камеры
            Vector3 newPosition = transform.position + new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);

            // Ограничиваем перемещение по оси X
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            // Применяем новую позицию камеры
            transform.position = newPosition;

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