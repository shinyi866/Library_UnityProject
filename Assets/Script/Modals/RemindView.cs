using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class RemindView : BoxView
{
    [SerializeField]
    private GameObject viewObject;

    private GameObject _viewObject;
    private Image image;
    private Text text;
    private Transform contentTransform;

    [HideInInspector]
    public Button leftButton;
    [HideInInspector]
    public Button rightButton;
    [HideInInspector]
    public Button button;

    public void ShowOriginRemindView(string txt)
    {
        CreateView();
        SetRect(600, 485);

        _viewObject.transform.GetChild(0).gameObject.SetActive(false);

        text.text = txt;
        button = ButtonGenerate.Instance.SetViewButton(StringAsset.RemindButton.success, TypeFlag.UIColorType.Orange, contentTransform);
    }


    public void ShowChooseRemindView(string txt)
    {
        CreateView();
        SetRect(600, 485);

        _viewObject.transform.GetChild(0).gameObject.SetActive(false);

        text.text = txt;
        CreateButtons(StringAsset.RemindButton.goToStudy, StringAsset.RemindButton.back, contentTransform);
    }

    public void ShowImageRemindView_Pet(string _txt, Sprite _sprite)
    {
        CreateView();
        SetRect(550, 850);

        var imageRect = image.gameObject.GetComponent<RectTransform>();
        imageRect.sizeDelta = new Vector2(300, 300);

        var textRect = text.gameObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(500, 295);
        text.fontSize = 38;

        image.sprite = _sprite;
        text.text = _txt;

        CreateButtons(StringAsset.RemindButton.back, StringAsset.RemindButton.confirm, contentTransform);
    }

    public void ShowImageRemindView(string _txt, Sprite _sprite)
    {
        CreateView();
        SetRect(550, 665);

        image.sprite = _sprite;
        text.text = _txt;

        CreateButtons(StringAsset.RemindButton.back, StringAsset.RemindButton.confirm, contentTransform);
    }

    private void CreateView()
    {
        _viewObject = Instantiate(viewObject);
        _viewObject.transform.SetParent(this.transform);

        image = _viewObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        text = _viewObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        contentTransform = _viewObject.transform.GetChild(2);
    }

    private void CreateButtons(string leftString, string rightString, Transform _transform)
    {
        if(leftButton == null)
        {
            leftButton = ButtonGenerate.Instance.SetViewButton(leftString, TypeFlag.UIColorType.Green, _transform);

            var _viewRect = leftButton.GetComponent<RectTransform>();
            _viewRect.localScale = new Vector3(1, 1, 1);
        }

        if (rightButton == null)
        {
            rightButton = ButtonGenerate.Instance.SetViewButton(rightString, TypeFlag.UIColorType.Orange, _transform);

            var _viewRect = rightButton.GetComponent<RectTransform>();
            _viewRect.localScale = new Vector3(1, 1, 1);
        }            
    }

    private void SetRect(float x, float y)
    {
        var _viewRect = _viewObject.GetComponent<RectTransform>();

        _viewRect.sizeDelta = new Vector2(x, y);
        _viewRect.anchoredPosition = new Vector2(0, 0);
        _viewRect.localScale = new Vector3(1, 1, 1);
    }

    public void DestoryView()
    {
        Views.instance.CloseView();
        Destroy(_viewObject);

        leftButton = null;
        rightButton = null;
    }
}
