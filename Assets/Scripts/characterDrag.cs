using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdsad : MonoBehaviour
{

    public float distanse = 6;

    void OnMouseDrag() {
        Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distanse);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
        transform.position = objPosition;
    }
}
