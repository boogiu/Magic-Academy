using UnityEngine;

public class EditModeUI : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void OpenEditUI()
    {
        gameObject.SetActive(true);
    }
    public void CloseEditUI()
    {
        gameObject.SetActive(false);
    }
}
