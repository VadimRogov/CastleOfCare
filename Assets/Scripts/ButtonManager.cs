using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerClickHandler
{
    // Событие, которое будет вызываться при нажатии на кнопку
    public UnityEvent onClick;
    public CellManager cellManager;
    public GameObject reception;
    private Camera mainCamera;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;


    // Слои для игнорирования и для кнопок
    private LayerMask ignoreLayer = LayerMask.GetMask("Cell"); // Слой, который нужно игнорировать (Cell)
    private LayerMask uiDialogLayer = LayerMask.GetMask("UIDialog"); // Слой кнопок (UIDialog)

    void Start()
    {
        mainCamera = Camera.main;
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);

        if (graphicRaycaster == null)
        {
            Debug.LogError("GraphicRaycaster не найден. Убедитесь, что он добавлен на Canvas.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Создаем маску, которая игнорирует слой Cell
            int layerMask = ~(1 << ignoreLayer.value);

            // Выполняем рейкаст для игнорирования слоя Cell
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider == null)
            {
                // Если рейкаст не попал в объект на слое Cell, проверяем UI-элементы
                pointerEventData.position = Input.mousePosition;

                // Создаем список для результатов рейкаста
                var results = new System.Collections.Generic.List<RaycastResult>();

                // Выполняем рейкаст по UI-элементам
                graphicRaycaster.Raycast(pointerEventData, results);

                foreach (var result in results)
                {
                    // Проверяем, является ли объект кнопкой на слое UIDialog
                    if (result.gameObject.layer == LayerMask.NameToLayer("UIDialog"))
                    {
                        Button button = result.gameObject.GetComponent<Button>();
                        if (button != null)
                        {
                            Debug.Log("Кнопка нажата: " + button.name);
                            // Вызываем событие при нажатии на кнопку
                            onClick.Invoke();
                            break; // Прерываем цикл, так как кнопка уже нажата
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Рейкаст попал в объект на слое Cell: " + hit.collider.name);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Этот метод будет вызываться при нажатии на кнопку через интерфейс IPointerClickHandler
        // Вызываем событие при нажатии на кнопку
        onClick.Invoke();
    }

    // Примеры методов, которые будут вызываться на кнопках
    public void OnButton1Clicked()
    {
        Debug.Log("Кнопка 1 нажата");
        // Добавьте логику для кнопки 1
    }

    public void SendOnProcedure()
    {
        Patient currentPatient = null;
        string doctor = "";
        MoveInStage move = null;
        foreach (Transform family in reception.transform)
        {
            if (family.CompareTag("FamilyKira") ||
                family.CompareTag("FamilyTosha") ||
                family.CompareTag("FamilyVitya"))
            {
                foreach (Transform patient in family.gameObject.transform.GetComponentsInChildren<Transform>())
                {
                    if (patient.CompareTag("Kira") ||
                        patient.CompareTag("Vitya") ||
                        patient.CompareTag("Tosha"))
                    {
                        currentPatient = patient.gameObject.GetComponent<Patient>();
                        doctor = patient.gameObject.GetComponent<Patient>().doctor;
                        Debug.LogWarning("doctor: " + doctor);

                        Cell roomProcedure = cellManager.FindProcedureRoom(doctor);
                        Debug.LogWarning("roomProcedure: " + roomProcedure.name);
                        Cell roomChamber = cellManager.FindRoomOnTag("Chamber");
                        Debug.LogWarning("roomChamber: " + roomChamber.name);

                        if (roomProcedure != null)
                        {
                            currentPatient.gameObject.GetComponent<Patient>().roomDoctor = roomProcedure;
                            currentPatient.gameObject.GetComponent<Patient>().chamber = roomChamber;
                        }
                        else Debug.LogWarning("roomProcedure не найден");

                        patient.gameObject.GetComponent<MoveInStage>().cellManager = cellManager;
                        patient.gameObject.GetComponent<MoveInStage>().moveLift = FindObjectOfType<MoveLift>();

                        patient.gameObject.GetComponent<MoveInStage>().currentCell = currentPatient.FindCharacter(currentPatient);

                    }
                }
            }
        }
    }
}