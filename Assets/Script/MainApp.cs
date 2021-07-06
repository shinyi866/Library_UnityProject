using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

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

    private int playerGuide;

    private void Start()
    {
        playerGuide = PlayerPrefs.GetInt("hasPlay");
        itemData.currentPet = itemData.petsItems[5]; // set default pet

        if (playerGuide != 1)
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView((int)TypeFlag.GuideType.main);
            // TODO: last button

            PlayerPrefs.SetInt("hasPlay", 1);
        }
    }
}
