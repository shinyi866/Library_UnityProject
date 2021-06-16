using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainApp : Singleton<MainApp>
{
    protected MainApp() { } // guarantee this will be always a singleton only - can't use the constructor!

    [SerializeField]
    private AllItemObj _itemData;
    public AllItemObj itemData => _itemData;

    [SerializeField]
    private GuideItemObj _guideData;
    public GuideItemObj guideData => _guideData;

    [SerializeField]
    private UIColorObj _uiColorData;
    public UIColorObj uiColorData => _uiColorData;
}
