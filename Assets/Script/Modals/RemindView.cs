using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class RemindView : Modal
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
        /*
        var _viewRect = _viewObject.GetComponent<RectTransform>();

        _viewRect.sizeDelta = new Vector2(600, 485);
        _viewRect.anchoredPosition = new Vector2(0, 0);
        _viewRect.localScale = new Vector3(1, 1, 1);
        */
        _viewObject.transform.GetChild(0).gameObject.SetActive(false);

        text.text = txt;
        button = ButtonGenerate.Instance.SetViewButton(StringAsset.RemindButton.success, TypeFlag.UIColorType.Orange);
        button.transform.SetParent(contentTransform);
    }


    public void ShowChooseRemindView(string txt)
    {
        CreateView();
        SetRect(600, 485);
        /*
        var _viewRect = _viewObject.GetComponent<RectTransform>();

        _viewRect.sizeDelta = new Vector2(600, 485);
        _viewRect.anchoredPosition = new Vector2(0, 0);
        _viewRect.localScale = new Vector3(1, 1, 1);
        */
        _viewObject.transform.GetChild(0).gameObject.SetActive(false);

        text.text = txt;
        CreateButtons();
    }

    public void ShowImageRemindView(string _txt, Sprite _sprite)
    {
        CreateView();
        SetRect(550, 665);
        /*
        var _viewRect = _viewObject.GetComponent<RectTransform>();

        _viewRect.sizeDelta = new Vector2(550, 665);
        _viewRect.anchoredPosition = new Vector2(0, 0);
        _viewRect.localScale = new Vector3(1, 1, 1);
        */
        image.sprite = _sprite;
        text.text = _txt;

        CreateButtons();
    }

    private void CreateView()
    {
        _viewObject = Instantiate(viewObject);
        _viewObject.transform.SetParent(this.transform);

        image = _viewObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        text = _viewObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        contentTransform = _viewObject.transform.GetChild(2);
    }

    private void CreateButtons()
    {
        if(leftButton == null)
        {
            leftButton = ButtonGenerate.Instance.SetViewButton(StringAsset.RemindButton.goToStudy, TypeFlag.UIColorType.Green);
            leftButton.transform.SetParent(contentTransform);
        }

        if (rightButton == null)
        {
            rightButton = ButtonGenerate.Instance.SetViewButton(StringAsset.RemindButton.back, TypeFlag.UIColorType.Orange);
            rightButton.transform.SetParent(contentTransform);
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
