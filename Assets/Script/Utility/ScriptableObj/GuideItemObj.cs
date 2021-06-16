using UnityEngine;

public class GuideItemObj : ScriptableObject
{
    [System.Serializable]
    public class GuideItem
    {
        public Sprite image1;
        public Sprite image2;
        public Sprite image3;
    }

    [SerializeField]
    public GuideItem[] guideItems;
}
