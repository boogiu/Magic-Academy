using System.Collections;
using UnityEngine;

public class MovealbePanel : MonoBehaviour, IClosableUI
{
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private RectTransform visualRoot;

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

    private enum openState { Opened, Closed };
    openState nowState = openState.Closed;

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

        moveDistance = visualRoot.sizeDelta.x;
        moveDirection = Vector2.Normalize(moveDirection) * -1;
        openedPosition = visualRoot.anchoredPosition;
        closedPosition = openedPosition + moveDirection * moveDistance;

        visualRoot.anchoredPosition = closedPosition;
    }

    public void OpenPanel()
    {
        if (nowState == openState.Opened) return;
        gameObject.SetActive(true);

        StartMove(
            closedPosition,
            openedPosition,
            true
        );

        uiManager?.AddStack(this);
    }

    public void Close()
    {
        if (nowState == openState.Closed) return;
        StartMove(
            visualRoot.anchoredPosition,
            closedPosition,
            false
        );
    }

    private void StartMove(
        Vector2 startPos,
        Vector2 targetPos,
        bool isInteractableAfterMove)
    {
        if (moveCoroutine != null)
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
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        visualRoot.anchoredPosition = startPos;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(elapsedTime / moveDuration);
            float easedRatio = EaseHelper.Evaluate(easeType, ratio);
            visualRoot.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedRatio);
            yield return null;
        }

        visualRoot.anchoredPosition = targetPos;
        gameObject.SetActive(isInteractableAfterMove);
        moveCoroutine = null;
        nowState = isInteractableAfterMove ? openState.Opened : openState.Closed;
    }
}