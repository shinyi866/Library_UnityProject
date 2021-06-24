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
        public int id;
        public string name;
        public string picture;
        public List<ClassifyType> classify;
        public int mood;
        public string info;
    }

    [System.Serializable]
    public class ClassifyType
    {
        public string id;
        public int name;
    }
}
