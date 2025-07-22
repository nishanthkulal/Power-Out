using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class KeyManager
{
    private static Dictionary<string, string> key;

    public static void LoadSecrets()
    {
        string path = Path.Combine(Application.dataPath, "../Key.env");
        key = new Dictionary<string, string>();

        if (!File.Exists(path))
        {
            Debug.LogError("Secrets file not found!");
            return;
        }

        foreach (var line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
            var parts = line.Split('=');
            if (parts.Length == 2)
                key[parts[0].Trim()] = parts[1].Trim();
        }
    }

    public static string Get(string key)
    {
        if (KeyManager.key == null) LoadSecrets();
        return KeyManager.key.ContainsKey(key) ? KeyManager.key[key] : null;
    }
}
