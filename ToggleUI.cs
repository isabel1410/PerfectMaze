using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
   [Tooltip("Panel to toggle")] [SerializeField] private GameObject _panel;
   [Tooltip("Button(s) to toggle.")] [SerializeField] private Button[] _buttons;

   [Tooltip("Input script to listen to event to")] [SerializeField] private Input _input;

    private bool _panelVisibility;
    private bool _buttonVisiblity;

    void Start()
    {
        // Add listener to event in Input
        _input.OnTogglingSettings.AddListener(ToggleSettingScreen);
    }

    ///<summary>Getts called when the player presses the escape button, or when the player presses the settings/cancel button</summary>
    public void ToggleSettingScreen()
    {
        // Toggle settings panel
        _panelVisibility = _panel.activeInHierarchy;
        _panel.SetActive(!_panelVisibility);

        // Toggle buttons
        ToggleButtons();
    }

    ///<summary>Getts called in ToggleSettingsScreen, and when the player presses the generate/stop button.</summary>
    public void ToggleButtons()
    {
        // Toggle buttons
        foreach (Button button in _buttons)
        {
            _buttonVisiblity = button.IsActive();
            button.gameObject.SetActive(!_buttonVisiblity);
        }
    }
}
