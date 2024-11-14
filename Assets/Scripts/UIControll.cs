using UnityEngine;

public class UIControll : MonoBehaviour
{
    public GameObject presentPanel;

    public void ClosedPanel()
    {
        presentPanel.SetActive(false);
    }
}
