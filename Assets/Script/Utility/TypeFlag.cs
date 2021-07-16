using System.Collections.Generic;

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
        public List<ClassifyType> classify;
        public string info;
        public int mood;
        public string name;
        public string picture;
    }

    [System.Serializable]
    public class ClassifyType
    {
        public int id;
        public string name;
    }
}
