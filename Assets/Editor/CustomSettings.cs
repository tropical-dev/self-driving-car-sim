using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomSettings
{
    static CustomSettings()
    {
        Debug.Log("custom memory settings");
        PlayerSettings.WebGL.memorySize = 1024;
    }
}
