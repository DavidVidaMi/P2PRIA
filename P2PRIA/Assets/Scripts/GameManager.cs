using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Cameras cameras;
    public RawImage rawImage;
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
                    ShowImage(webRequest.downloadHandler.text);
                    break;
            }
        }

        void ShowImage(string jsonString)
        {
            cameras = JsonUtility.FromJson<Cameras>(jsonString);
            //rawImage.texture = UnityWebRequestTexture.GetTexture(cameras.listaCamaras[Random.Range(0, cameras.listaCamaras.Count)].imaxeCamara);

            StartCoroutine(GetTexture(cameras.listaCamaras[Random.Range(0, cameras.listaCamaras.Count)].imaxeCamara));
        }

        IEnumerator GetTexture(string texture)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(texture);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            }
        }
    }
}
