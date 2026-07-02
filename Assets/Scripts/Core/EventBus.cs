using UnityEngine;
using System.Collections.Generic;
using System;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> handlers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (handlers.TryGetValue(type, out var exist))
        {
            handlers[type] = Delegate.Combine(exist, handler);
        }
        else
        {
            handlers[type] = handler;
        }
    }

    public static void UnSubscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (!handlers.TryGetValue(type, out var exist))
            return;

        var result = Delegate.Remove(exist, handler);
        if (result == null)
            handlers.Remove(type);
        else
            handlers[type] = result;
    }

    public static void Raise<T>(T eventData)
    {
        if (handlers.TryGetValue(typeof(T), out var exist))
            ((Action<T>)exist)?.Invoke(eventData);
    }
}
