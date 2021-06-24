using UnityEngine;

public static class JsonSerialization
{
    public static T[] FromJson<T>(string json)
    {
        json = "{ \"items\": " + json + "}";

        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] items;
    }
}
