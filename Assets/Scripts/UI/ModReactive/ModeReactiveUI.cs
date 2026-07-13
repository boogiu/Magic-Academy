using UnityEngine;

public abstract class ModeReactiveUI<TEvent> : MonoBehaviour where TEvent: struct
{
    protected virtual void OnEnable()
    {
        EventBus.Subscribe<TEvent>(HandleModeChanged);
    }

    protected virtual void OnDisable()
    {
        EventBus.UnSubscribe<TEvent>(HandleModeChanged);
    }

    protected abstract void HandleModeChanged(TEvent evt);
}