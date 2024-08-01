using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RequestExample : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RepeatGetRequest("http://127.0.0.1:5001/"));
    }

    IEnumerator RepeatGetRequest(string uri)
    {
        while (true)  // This creates an infinite loop
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);
                }
            }
            yield return new WaitForSeconds(5f);  // Wait for 5 seconds before the next request
        }
    }
}