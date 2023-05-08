using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Slider _width;
    [SerializeField] private Slider _height;
    [SerializeField] private TMP_Text _widthText;
    [SerializeField] private TMP_Text _heightText;

    [Header("Settings")]
    [SerializeField] private GameSettings _gameSettings;

    private void Start()
    {
        // Check if settings already exist
        bool doSettingsExist =  _gameSettings.Load();
        if (!doSettingsExist)
        {
            // If not, set default settings
            SetDefaultSettings(); return;
        }

        // If they do, set the UI to the previously set settings
        _width.value = _gameSettings.Width;
        _height.value = _gameSettings.Height;

        _widthText.text = _gameSettings.Width.ToString();
        _heightText.text = _gameSettings.Height.ToString();
    }

    ///<summary>Sets width when user slides slider</summary>
    ///<param name="value">Dynamic: value from slider</param>
    public void SetWidth(float value)
    {
        _gameSettings.Width = (uint)value;
        _widthText.text = value.ToString();
    }

    ///<summary>Sets height when user slides slider</summary>
    ///<param name="value">Dynamic: value from slider</param>
    public void SetHeight(float value)
    {
        _gameSettings.Height = (uint)value;
        _heightText.text = value.ToString();
    }

    ///<summary>Gets called whenever the user edits their settings.</summary>
    public void Save()
    {
        _gameSettings.Save();
    }

    ///<summary>Sets default settings if the user does not have them.</summary>
    private void SetDefaultSettings()
    {
        // Change scriptable object
        _gameSettings.SetDefaultSettings((uint)_width.minValue, (uint)_height.minValue);

        // Change UI
        _width.value = _gameSettings.Width;
        _height.value = _gameSettings.Height;

        _widthText.text = _gameSettings.Width.ToString();
        _heightText.text = _gameSettings.Height.ToString();
    }
}
