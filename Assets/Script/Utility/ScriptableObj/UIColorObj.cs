using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UIColorObj", menuName = "ScriptableObjects/UIColorObj", order = 1)]
public class UIColorObj : ScriptableObject
{
    [SerializeField]
    List<UIColorStruct> colors = new List<UIColorStruct>();

    public UIColorStruct GetUIColor(TypeFlag.UIColorType type)
    {
        int index = colors.FindIndex(x => x.name == type);

        if (index > 0)
            return colors[index];

        return default(UIColorStruct);
    }

    [System.Serializable]
    public struct UIColorStruct
    {
        public TypeFlag.UIColorType name;
        public Color color;
    }
}