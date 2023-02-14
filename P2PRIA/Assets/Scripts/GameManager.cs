using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //Start a coroutine to call the API
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/observacion/jsonCamaras.action"));
    }

    IEnumerator GetRequest(string uri)
    {
       using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Gives feedback.
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("ConnectionError ");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
