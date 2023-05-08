using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public uint Width;
    public uint Height;

    [Tooltip("Path where the settings are stored")] private string _jsonPath => Application.dataPath + "/" + name + ".txt";

    ///<summary>Saves settings to a text file.</summary>
    internal void Save()
    {
        string settings = JsonUtility.ToJson(this, true);
        File.WriteAllText(_jsonPath, settings);
    }

    ///<summary>Load settings if file exists.</summary>
    /// <returns>Returns true if loading is succesful, otherwise false</returns>
    internal bool Load()
    {
        if (File.Exists(_jsonPath))
        {
            string settingsText = File.ReadAllText(_jsonPath);
            JsonUtility.FromJsonOverwrite(settingsText, this);
            return true;
        }
        return false;
    }

    ///<summary>Sets default settings if they don't exist.</summary>
    /// <param name="width">Width to set</param>
    /// <param name="height">Height to set</param>
    internal void SetDefaultSettings(uint width, uint height)
    {
        Width = width;
        Height = height;
    }

}
