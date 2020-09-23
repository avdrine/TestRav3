using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : MonoBehaviour
{
    public static NetManager Instance;
    [SerializeField] private string _serverURL = "https://dev3r02.elysium.today/inventory/status";
    [SerializeField] private string _authorizationKey = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";

    private void Awake()
    {
        Instance = this;
    }

    public void SendEquipMessage(PhysicalItem item)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>
        {
            { "itemID", item.ItemData.Id.ToString() }
        };

        StartCoroutine(SendMessageToServer("eqip", keyValues));
    }
    public void SendUnequipMessage(PhysicalItem item)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>
        {
            { "itemID", item.ItemData.Id.ToString() }
        };

        StartCoroutine(SendMessageToServer("uneqip", keyValues));
    }

    private IEnumerator SendMessageToServer(string messageType, Dictionary<string, string> keyValues)
    {
        WWWForm data = new WWWForm();
        data.AddField("auth", "\"" + _authorizationKey + "\"");
        data.AddField("messageType", messageType);



        //string sendData = "auth=" + _authorizationKey + "&"+ "messageType=" + messageType;
        foreach(var curr in keyValues)
        {
            data.AddField(curr.Key, curr.Value);
            //sendData += "&" + curr.Key + "=" + curr.Value;
        }

        UnityWebRequest www = UnityWebRequest.Post(_serverURL, data);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Send message - error: " + www.error);
        }
        else
        {
            Debug.Log("Send message success!");
        }
    }


}
