using UnityEngine;

public enum EaseType
{
    Linear,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseOutBack
}

public static class EaseHelper
{
    public static float Evaluate(EaseType easeType, float ratio)
    {
        ratio = Mathf.Clamp01(ratio);

        switch (easeType)
        {
            case EaseType.EaseInCubic:
                return EaseInCubic(ratio);

            case EaseType.EaseOutCubic:
                return EaseOutCubic(ratio);

            case EaseType.EaseInOutCubic:
                return EaseInOutCubic(ratio);

            case EaseType.EaseOutBack:
                return EaseOutBack(ratio);

            case EaseType.Linear:
            default:
                return ratio;
        }
    }

    public static float EaseInCubic(float ratio)
    {
        return ratio * ratio * ratio;
    }

    public static float EaseOutCubic(float ratio)
    {
        float inverseRatio = 1f - ratio;
        return 1f - inverseRatio * inverseRatio * inverseRatio;
    }

    public static float EaseInOutCubic(float ratio)
    {
        if (ratio < 0.5f)
        {
            return 4f * ratio * ratio * ratio;
        }

        float adjustedRatio = -2f * ratio + 2f;
        return 1f - adjustedRatio * adjustedRatio * adjustedRatio / 2f;
    }

    public static float EaseOutBack(float ratio)
    {
        float overshoot = 1.70158f;
        float adjustedRatio = ratio - 1f;

        return 1f + (overshoot + 1f) * adjustedRatio * adjustedRatio * adjustedRatio
            + overshoot * adjustedRatio * adjustedRatio;
    }
}