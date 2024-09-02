using System;
using UnityEngine;

[Serializable] public struct ClampedFloat {
    public event Action<float> OnIncreaseValue;
    public event Action<float> OnDecreaseValue;
    public event Action<float> OnValueChanged;

    public float minValue;

    [SerializeField] private float maxValue;

    public float MaxValue {
        get => maxValue;
        set {
            maxValue = value;
            OnValueChanged?.Invoke(Value);
        }
    }

    [SerializeField] private float value;

    public float Value {
        get => value;
        set {
            var oldValue = this.value;
            this.value = Mathf.Clamp(value, minValue, MaxValue);
            // if (this.value == oldValue) return; // if we don't want anything to happen when the value is set to the same value

            OnValueChanged?.Invoke(this.value);

            if (value > oldValue) {
                OnIncreaseValue?.Invoke(this.value);
            }
            else if (value < oldValue) {
                OnDecreaseValue?.Invoke(this.value);
            }
        }
    }

    public ClampedFloat(float minValue = -Mathf.Infinity, float maxValue = Mathf.Infinity, float initialValue = 0) {
        this.minValue = minValue;
        this.maxValue = maxValue;

        value = Mathf.Clamp(initialValue, minValue, maxValue);

        OnValueChanged = null;
        OnDecreaseValue = null;
        OnIncreaseValue = null;
    }

    public static implicit operator float(ClampedFloat clampedFloat) {
        return clampedFloat.Value;
    }

    public static implicit operator ClampedFloat(float value) {
        return new ClampedFloat(value, float.MinValue, float.MaxValue);
    }

    public static ClampedFloat operator +(ClampedFloat clampedFloat, float amount) {
        clampedFloat.Value += amount;
        return clampedFloat;
    }

    public static ClampedFloat operator -(ClampedFloat clampedFloat, float amount) {
        clampedFloat.Value -= amount;
        return clampedFloat;
    }
}