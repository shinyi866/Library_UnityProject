using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }

            try
            {
                string rawJSON = webRequest.downloadHandler.text;
                Debug.Log("rawJSON: " + rawJSON);

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

    public static IEnumerator GetImage(string url, System.Action<Sprite> success)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            webRequest.timeout = 40;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Picture Error: " + webRequest.error);
            }

            try
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                Sprite webSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                if (webRequest.downloadHandler.text != null)
                {
                    success(webSprite);
                }

            }
            catch
            {
                success(null);
                Debug.Log("Picture Error2: " + webRequest.error);
            }
        }
    }

    public static IEnumerator PostJson(string url, string httpMethods, string type, string rawJsonObject, System.Action<bool> callback)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 40;
            webRequest.method = httpMethods;

            if(rawJsonObject != null)
            {
                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(rawJsonObject));
                webRequest.uploadHandler.contentType = type;
            }

            yield return webRequest.SendWebRequest();

            if(webRequest.isNetworkError || webRequest.isHttpError)
            {
                callback(false);
                Debug.Log("Web  Error " + webRequest.error);

                yield break;
            }
            else
            {
                callback(true);
                Debug.Log("Form upload complete!");
            }
        }
    }

    public static IEnumerator PostFormData(string url, string type, TypeFlag.CoverForm coverForm, System.Action<bool> callback)
    {
        WWWForm form = new WWWForm();

        if (coverForm != null)
        {
            string picNmae = DateTime.Now + ".png";
            form.AddField("book_id", coverForm.book_id);
            form.AddBinaryData("f", coverForm.f, picNmae, "image/png");
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.timeout = 40;
            webRequest.uploadHandler.contentType = type;

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                callback(false);
                Debug.Log("Web  Error " + webRequest.error);

                yield break;
            }
            else
            {
                callback(true);
                Debug.Log("Form upload complete!");
            }
        }

    }
}

