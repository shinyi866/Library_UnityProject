using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class GuideModal : Modal
{
    [SerializeField]
    private GameObject BarObject;

    private Image guideImage;
    private Button nextButton;
    private int count = 0;

    private void Awake()
    {
        guideImage = this.transform.GetChild(0).gameObject.GetComponent<Image>();
        nextButton = this.transform.GetChild(1).gameObject.GetComponent<Button>();
    }

    public void ShowView(TypeFlag.GuideType type)
    {
        nextButton.onClick.AddListener(()=> {
            count++;
            ButtonClick(type);
        });
    }

    private void ButtonClick(TypeFlag.GuideType type)
    {
        var data = MainApp.Instance.guideData.guideItems[(int)type];
        guideImage.sprite = data.image1;

        if (count == 1)
            guideImage.sprite = data.image2;

        if (count == 2)
            guideImage.sprite = data.image3;

        if (count == 3)
        {
            switch (type)
            {
                case TypeFlag.GuideType.main:
                    Modals.instance.OpenModal<MainModal>();
                    break;
                case TypeFlag.GuideType.ARfindBook:
                    Modals.instance.CloseAllModal();
                    BarObject.SetActive(false);
                    // TODO: open AR camera
                    break;
                case TypeFlag.GuideType.classify:
                    Modals.instance.OpenModal<BookClassifyModal>();
                    break;
            }
        }
    }
}
