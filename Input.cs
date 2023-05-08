using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    [Tooltip("Event to call when user presses the escape button")] public UnityEvent OnTogglingSettings;

    private Vector2 _moveInput;
    public Vector2 MoveInput { get { return _moveInput; } }

    private float _scrollInput;
    public float ScrollInput { get { return _scrollInput; } }

    ///<summary>Gets called when the user presses the escape button</summary>
    /// <param name="value">The value of the action (whether the escape button is pressed or not). In this case not applicable.</param>
    public void OnToggleSettings(InputValue value)
    {
        OnTogglingSettings?.Invoke();
    }

    ///<summary>Gets called when the user presses W, A, S, or D.</summary>
    /// <param name="value">The value of the action, which button(s) the user is pressing</param>
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    ///<summary>Gets called when the user scrolls with their mouse</summary>
    /// <param name="value">The value of the action, which way the user is scrolling</param>
    public void OnZoom(InputValue value)
    {
        _scrollInput = value.Get<float>();
    }
}
