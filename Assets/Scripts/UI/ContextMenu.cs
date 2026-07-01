using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour, IClosableUI
{
    [Header("Manager")]
    [SerializeField]
    private UIManager uiManager;

    [Header("Buttons")]
    [SerializeField]
    private Button facilityButton;

    private Coroutine moveCoroutine;

    [Header("Animations")]
    [SerializeField]
    private RectTransform visualRoot;
    [SerializeField]
    private float moveDistance = 100f;
    [SerializeField]
    private float moveDuration = 0.2f;
    [SerializeField]
    private EaseType easeType = EaseType.Linear;

    private Vector2 openedPosition;
    private Vector2 closedPosition;

    private void Awake()
    {
        if (visualRoot == null)
        {
            visualRoot = GetComponent<RectTransform>();
        }

        moveDistance = visualRoot.sizeDelta.y;
        openedPosition = visualRoot.anchoredPosition;
        closedPosition = openedPosition + Vector2.down * moveDistance;
        visualRoot.anchoredPosition = closedPosition;
    }
    private void OnEnable()
    {
        Debug.Log("ContextMenu OnEnable");
    }

    private void OnDisable()
    {
        Debug.Log($"ContextMenu OnDisable\n{System.Environment.StackTrace}");
    }
    public void OpenContextMenu()
    {
        gameObject.SetActive (true);
        Debug.Log($"activeSelf: {gameObject.activeSelf}, activeInHierarchy: {gameObject.activeInHierarchy}");
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        moveCoroutine= StartCoroutine(MoveCoroutine(closedPosition,openedPosition,true));
        uiManager?.AddStack(this);
    }
    public void Close()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        moveCoroutine = StartCoroutine(
            MoveCoroutine(visualRoot.anchoredPosition, closedPosition, false)
        );
    }

    private IEnumerator MoveCoroutine(
    Vector2 startPos,
    Vector2 targetPos,
    bool activeAfterMove)
    {
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
        moveCoroutine = null;
        gameObject.SetActive(activeAfterMove);
    }
}
