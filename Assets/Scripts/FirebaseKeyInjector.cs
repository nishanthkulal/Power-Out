using System.Collections.Generic;
using Google.MiniJSON;
using UnityEngine;

public class FirebaseKeyInjector : MonoBehaviour
{
    [SerializeField] private TextAsset googleServicesJson;

    void Start()
    {
        if (googleServicesJson == null)
        {
            Debug.LogError("TextAsset is not assigned.");
            return;
        }

        string apiKey = KeyManager.Get("API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API_KEY not found in .env!");
            return;
        }

        string json = googleServicesJson.text;

        var jsonDict = Json.Deserialize(json) as Dictionary<string, object>;
        if (jsonDict == null || !jsonDict.ContainsKey("client"))
        {
            Debug.LogError("Invalid JSON format.");
            return;
        }

        var clientList = jsonDict["client"] as List<object>;
        var clientDict = clientList[0] as Dictionary<string, object>;
        var apiKeyList = clientDict["api_key"] as List<object>;
        var apiKeyDict = apiKeyList[0] as Dictionary<string, object>;

        apiKeyDict["current_key"] = apiKey;

        string updatedJson = Json.Serialize(jsonDict);
        Debug.Log("Updated JSON:\n" + updatedJson);
    }
}
