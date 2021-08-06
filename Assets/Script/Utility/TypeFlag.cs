using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeFlag
{
    public enum UIColorType
    {
        Title,
        Lias,
        Green,
        Orange,
        AquaGreen,
        BenzoBlue,
        Purple,
        White
    }

    public enum GuideType
    {
        main,
        ARfindBook,
        classify
    }

    [System.Serializable]
    public class BookDatabaseType
    {
        public string book_id;
        public List<string> classify;
        public string info;
        public int mood;
        public string name;
        public string picture;

        public Classify GetClassify(string _classifyString)
        {
            Classify _classify = new Classify();

            string[] splitArray = _classifyString.Split(char.Parse("-"));
            _classify.major = int.Parse(splitArray[0]);
            _classify.minor = int.Parse(splitArray[1]);


            return _classify;
        }
    }

    [System.Serializable]
    public class Classify
    {
        public int major;
        public int minor;
    }

    [System.Serializable]
    public class CatForm
    {
        public string cat;
        public string book_id;
    }

    [System.Serializable]
    public class ReadForm
    {
        public string user_id;
        public string book_id;
    }

    [System.Serializable]
    public class MoodForm
    {
        public string user_id;
        public string book_id;
        public int mood;
    }

    [System.Serializable]
    public class CoverForm
    {
        public string book_id;
        public byte[] f;
    }
}
