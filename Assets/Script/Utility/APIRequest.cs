using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIRequest
{
    public static IEnumerator GetRequest(string url, string httpMethods, System.Action<string> success)
    { 
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 40;
            webRequest.method = httpMethods;

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if(webRequest.isNetworkError)
            {
                success(null);
                Debug.Log("Web Error: " + webRequest.error);

                yield break;
            }

            try
            {
                string rawJSON = webRequest.downloadHandler.text;                

                if (webRequest.downloadHandler.text != null)
                {                    
                    success(rawJSON);
                }
            }
            catch
            {
                success(null);
                Debug.Log("Web Error2: " + webRequest.error);
            }
            
        }
    }
}

