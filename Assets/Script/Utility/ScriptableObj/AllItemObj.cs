using UnityEngine;

public class AllItemObj : ScriptableObject
{
    [System.Serializable]
    public class BookItem
    {
        public string[] name;
        public Sprite[] image;
    }

    [System.Serializable]
    public class BookTitleItem
    {
        public string name;
        public Sprite image;
    }

    [System.Serializable]
    public class MoodItem
    {
        public string name;
        public Sprite image;
        public Sprite image_noShadow;
    }

    [System.Serializable]
    public class RankItems
    {
        public Sprite[] topImage;
    }

    [System.Serializable]
    public struct PetsItem
    {
        public string name;
        public Sprite image;
        public string info;
        public Sprite bgImage;
    }

    [SerializeField]
    public BookTitleItem[] booksTitleItems;
    public BookItem[] booksItems;
    public MoodItem[] moodItems;
    public RankItems rankItems;
    public PetsItem[] petsItems;
    public PetsItem currentPet;
}