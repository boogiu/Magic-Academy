using System.Collections.Generic;
using UnityEngine;

public interface IClosableUI
{
    public bool Close(); 
}

public class UIManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputHandler inputHandler;

    [Header("UI_Canvas")]

    [SerializeField]
    private ContextMenu contextMenu;

    private Stack<IClosableUI> uiStack = new Stack<IClosableUI>();

    private void OnEnable()
    {
        inputHandler.CloseUIRequested += CloseUI;
        if (contextMenu != null) inputHandler.ContextMenuRequested += OpenContextMenu;
    }

    private void OnDisable()
    {
        inputHandler.CloseUIRequested -= CloseUI;
        if (contextMenu != null) inputHandler.ContextMenuRequested -= OpenContextMenu;
    }

    public void OpenContextMenu()
    {
        if (uiStack.Count == 0) //별도 유아이 없을 때만 동작
        {
            contextMenu.OpenContextMenu();
        }
    }

    public void AddStack(IClosableUI closableUI)
    {
        uiStack.Push(closableUI);
    }

    public void CloseUI()
    {
        if (uiStack.Count == 0) return; //나중에 설정 메뉴넣기

        Stack<IClosableUI> tmpStack = new();
        bool validClose = false;

        while(!validClose && uiStack.Count != 0)
        {
            IClosableUI topUI = uiStack.Pop();
            validClose = topUI.Close();
            if(!validClose)
            {
                tmpStack.Push(topUI);
            }
        }


        while(tmpStack.Count != 0)
        {
            uiStack.Push(tmpStack.Pop());
        }
    }
}
