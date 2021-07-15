using UnityEngine;

[CreateAssetMenu(fileName = "GoalItemObj", menuName = "ScriptableObjects/GoalItemObj", order = 2)]
public class GoalItemObj : ScriptableObject
{
    [System.Serializable]
    public class GuideItem
    {
        public Sprite non;
        public Sprite book;
        public Sprite angry;
        public Sprite scared;
        public Sprite happy;
        public Sprite wow;
        public Sprite sad;
        public Sprite hate;
    }

    [SerializeField]
    public GuideItem guideItem;
}
