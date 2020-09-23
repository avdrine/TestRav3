using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Сетевой контроллер
/// </summary>
public class NetManager : MonoBehaviour
{
    #region Переменные
    /// <summary>
    /// Singleton
    /// </summary>
    public static NetManager Instance;

    /// <summary>
    /// Флаг использовать ли сеть
    /// </summary>
    public bool useNet = true;

    /// <summary>
    /// Url для запросов на сервер
    /// </summary>
    [SerializeField] private string _serverURL = "https://dev3r02.elysium.today/inventory/status";

    /// <summary>
    /// Токен для авторизации
    /// </summary>
    [SerializeField] private string _authorizationKey = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";

    #endregion

    #region Функции

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Отправить сообщение на сервер, что предмет экипирован
    /// </summary>
    /// <param name="item">Одеваемый предмет</param>
    public void SendEquipMessage(PhysicalItem item)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>
        {
            { "itemID", item.ItemData.Id.ToString() }
        };

        StartCoroutine(SendMessageToServer("eqip", keyValues));
    }

    /// <summary>
    /// Отправить сообщение на сервер, что предмет снят
    /// </summary>
    /// <param name="item">Снимаемый предмет</param>
    public void SendUnequipMessage(PhysicalItem item)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>
        {
            { "itemID", item.ItemData.Id.ToString() }
        };

        StartCoroutine(SendMessageToServer("uneqip", keyValues));
    }

    /// <summary>
    /// Отправка сообщения на сервер
    /// </summary>
    /// <param name="messageType">Тип сообщения/операции</param>
    /// <param name="keyValues">Поля данных</param>
    /// <returns></returns>
    private IEnumerator SendMessageToServer(string messageType, Dictionary<string, string> keyValues)
    {
        if(useNet)
        {
            //Dictionary<string, string> headers = new Dictionary<string, string>();
            //headers.Add("Authorization", "Basic " + Base64Encode("auth:" + _authorizationKey));

            WWWForm data = new WWWForm();
            //data.AddField("Authorization", "Basic " + Base64Encode("auth:"+ _authorizationKey));
            data.AddField("messageType", messageType);
            foreach (var curr in keyValues)
            {
                data.AddField(curr.Key, curr.Value);
            }

            UnityWebRequest www = UnityWebRequest.Post(_serverURL, data);
            www.SetRequestHeader("Authorization", /*"Basic " + Base64Encode(*/"auth: " + _authorizationKey)/*)*/;
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

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    #endregion

}
