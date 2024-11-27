using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerClickHandler
{
    // �������, ������� ����� ���������� ��� ������� �� ������
    public UnityEvent onClick;
    public CellManager cellManager;
    public GameObject reception;
    private Camera mainCamera;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;


    // ���� ��� ������������� � ��� ������
    private LayerMask ignoreLayer = LayerMask.GetMask("Cell"); // ����, ������� ����� ������������ (Cell)
    private LayerMask uiDialogLayer = LayerMask.GetMask("UIDialog"); // ���� ������ (UIDialog)

    void Start()
    {
        mainCamera = Camera.main;
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);

        if (graphicRaycaster == null)
        {
            Debug.LogError("GraphicRaycaster �� ������. ���������, ��� �� �������� �� Canvas.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ������� �����, ������� ���������� ���� Cell
            int layerMask = ~(1 << ignoreLayer.value);

            // ��������� ������� ��� ������������� ���� Cell
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider == null)
            {
                // ���� ������� �� ����� � ������ �� ���� Cell, ��������� UI-��������
                pointerEventData.position = Input.mousePosition;

                // ������� ������ ��� ����������� ��������
                var results = new System.Collections.Generic.List<RaycastResult>();

                // ��������� ������� �� UI-���������
                graphicRaycaster.Raycast(pointerEventData, results);

                foreach (var result in results)
                {
                    // ���������, �������� �� ������ ������� �� ���� UIDialog
                    if (result.gameObject.layer == LayerMask.NameToLayer("UIDialog"))
                    {
                        Button button = result.gameObject.GetComponent<Button>();
                        if (button != null)
                        {
                            Debug.Log("������ ������: " + button.name);
                            // �������� ������� ��� ������� �� ������
                            onClick.Invoke();
                            break; // ��������� ����, ��� ��� ������ ��� ������
                        }
                    }
                }
            }
            else
            {
                Debug.Log("������� ����� � ������ �� ���� Cell: " + hit.collider.name);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ���� ����� ����� ���������� ��� ������� �� ������ ����� ��������� IPointerClickHandler
        // �������� ������� ��� ������� �� ������
        onClick.Invoke();
    }

    // ������� �������, ������� ����� ���������� �� �������
    public void OnButton1Clicked()
    {
        Debug.Log("������ 1 ������");
        // �������� ������ ��� ������ 1
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
                        else Debug.LogWarning("roomProcedure �� ������");

                        patient.gameObject.GetComponent<MoveInStage>().cellManager = cellManager;
                        patient.gameObject.GetComponent<MoveInStage>().moveLift = FindObjectOfType<MoveLift>();

                        patient.gameObject.GetComponent<MoveInStage>().currentCell = currentPatient.FindCharacter(currentPatient);

                    }
                }
            }
        }
    }
}