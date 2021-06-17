using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGenerate : Singleton<ButtonGenerate>
{
    protected ButtonGenerate() { }

    [SerializeField]
    private GameObject remindObject;

    private GameObject _remindObject;
    private Button button;
    private Text text;
    private Image image;

    public Button SetModalButton(string txt, TypeFlag.UIColorType colorType)
    {
        _remindObject = Instantiate(remindObject);

        var _remindRect = _remindObject.GetComponent<RectTransform>();
        _remindRect.sizeDelta = new Vector2(280, 100);

        button = _remindObject.GetComponent<Button>();
        text = _remindObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        image = _remindObject.GetComponent<Image>();

        text.text = txt;
        image.color = MainApp.Instance.uiColorData.GetUIColor(colorType).color;

        return button;
    }

    public Button SetViewButton(string txt, TypeFlag.UIColorType colorType)
    {
        _remindObject = Instantiate(remindObject);

        var _remindRect = _remindObject.GetComponent<RectTransform>();
        _remindRect.sizeDelta = new Vector2(220, 75);

        button = _remindObject.GetComponent<Button>();
        text = _remindObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        image = _remindObject.GetComponent<Image>();

        text.text = txt;
        image.color = MainApp.Instance.uiColorData.GetUIColor(colorType).color;

        return button;
    }
}
