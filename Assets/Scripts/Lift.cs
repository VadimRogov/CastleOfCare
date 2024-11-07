using UnityEngine;
using System.Collections;
public class Lift : MonoBehaviour
{
    [SerializeField] private Transform lDoor; // ����� �����
    [SerializeField] private Transform rDoor; // ������ �����

    private Vector3 lDoorClosedPosition; // ������� �������� ����� �����
    private Vector3 rDoorClosedPosition; // ������� �������� ������ �����



    void Awake()
    {
        // �������� ��������� ���������� ������ ��� �������������
        lDoorClosedPosition = lDoor.localPosition;
        rDoorClosedPosition = rDoor.localPosition;
    }

    public void SetOpenDoor()
    {
        StartCoroutine(OpenDoors());
    }

    public void SetCloseDoor()
    {
        StartCoroutine(CloseDoors());
    }

    private IEnumerator OpenDoors()
    {
        float duration = 1.0f; // ����� �������� � ��������
        float elapsedTime = 0f;

        Vector3 targetLPos = new Vector3(lDoorClosedPosition.x - 1.6f, lDoorClosedPosition.y, lDoorClosedPosition.z);
        Vector3 targetRPos = new Vector3(rDoorClosedPosition.x + 1.6f, rDoorClosedPosition.y, rDoorClosedPosition.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // �������� ������������ ����� ������� � ������� ��������
            lDoor.localPosition = Vector3.Lerp(lDoorClosedPosition, targetLPos, t);
            rDoor.localPosition = Vector3.Lerp(rDoorClosedPosition, targetRPos, t);
            yield return null; // ���� ���������� �����
        }

        // ��������, ��� ����� ����������� � �������� �������
        lDoor.localPosition = targetLPos;
        rDoor.localPosition = targetRPos;
    }

    private IEnumerator CloseDoors()
    {
        float duration = 2.0f; // ����� �������� � ��������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // �������� ������������ ����� ������� � �������� ��������
            lDoor.localPosition = Vector3.Lerp(lDoor.localPosition, lDoorClosedPosition, t);
            rDoor.localPosition = Vector3.Lerp(rDoor.localPosition, rDoorClosedPosition, t);
            yield return null; // ���� ���������� �����
        }

        // ��������, ��� ����� ����������� � �������� �������
        lDoor.localPosition = lDoorClosedPosition;
        rDoor.localPosition = rDoorClosedPosition;
    }
}