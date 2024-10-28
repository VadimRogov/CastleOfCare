using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private bool isEmpty = true;

    [SerializeField] private GameObject closed;
    [SerializeField] private GameObject open;

    public bool IsEmpty
    {
        get { return isEmpty; }
        private set { isEmpty = value; }
    }


    public void SetCondition()
    {
        if(isEmpty) 
        {
            closed.SetActive(false);
            open.SetActive(true);
        }
    }

    public void SetReturn(bool isBuild)
    {
        if(!isBuild)
        {
            closed.SetActive(true);
            open.SetActive(false);
        }
    }
}
