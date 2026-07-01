using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovableButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform visualRoot;
    [SerializeField]
    private Button button;

    [SerializeField]
    private float moveDistance = 100f;

    [SerializeField]
    private float moveDuration = 0.2f;

    [SerializeField]
    private float delay = 0f;

    [SerializeField]
    private EaseType easeType = EaseType.Linear;
    [SerializeField]
    private Vector2 moveDirection = Vector2.down;

    private Vector2 openedPosition;
    private Vector2 closedPosition;

    private Coroutine moveCoroutine;

    public float TotalDuration
    {
        get
        {
            return delay + moveDuration;
        }
    }

    private void Awake()
    {
        if (visualRoot == null)
        {
            visualRoot = GetComponent<RectTransform>();
        }
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        moveDirection = Vector2.Normalize(moveDirection) * -1;
        openedPosition = visualRoot.anchoredPosition;
        closedPosition = openedPosition + moveDirection * moveDistance;

        visualRoot.anchoredPosition = closedPosition;
        SetInteractable(false);
    }

    public void PlayOpen()
    {
        StartMove(closedPosition, openedPosition, true);
    }

    public void PlayClose()
    {
        StartMove(visualRoot.anchoredPosition, closedPosition, false);
    }

    private void StartMove(
        Vector2 startPos, 
        Vector2 targetPos,
        bool isInteractableAfterMove)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        moveCoroutine = StartCoroutine(
            MoveCoroutine(startPos, targetPos, isInteractableAfterMove)
        );
    }

    private IEnumerator MoveCoroutine(
        Vector2 startPos,
        Vector2 targetPos,
        bool isInteractableAfterMove)
    {
        SetInteractable(false);

        if(delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        visualRoot.anchoredPosition = startPos;
        float elapsedTime = 0f;

        while(elapsedTime < moveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(elapsedTime / moveDuration);
            float easedRatio = EaseHelper.Evaluate(easeType, ratio);
            visualRoot.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedRatio);
            yield return null;
        }
        visualRoot.anchoredPosition = targetPos;
        SetInteractable(isInteractableAfterMove);
        moveCoroutine = null;
    }
    private void SetInteractable(bool isInteractable)
    {
        if (button != null)
        {
            button.interactable = isInteractable;
        }
    }
}