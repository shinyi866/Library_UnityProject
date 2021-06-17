using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MoreInfoView : Modal
{
    [SerializeField]
    private GameObject viewObject;

    private GameObject _viewObject;
    private Text text;
    private Button closeButton;
    private RectTransform containerRect;
    private RectTransform TextRect;

    public void ShowView(string txt)
    {
        _viewObject = Instantiate(viewObject);
        _viewObject.transform.SetParent(this.transform);

        var _viewRect = _viewObject.GetComponent<RectTransform>();
        _viewRect.anchoredPosition = new Vector2(0, 0);
        _viewRect.localScale = new Vector3(1, 1, 1);

        var container = _viewObject.transform.GetChild(0);
        containerRect = container.gameObject.GetComponent<RectTransform>();
        text = container.GetChild(0).gameObject.GetComponent<Text>();
        TextRect = text.gameObject.GetComponent<RectTransform>();
        closeButton = _viewObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Button>();

        var addHeight = (txt.Length)/10 * 50;
        TextRect.sizeDelta = new Vector2(475, addHeight);

        if (addHeight > 1100)
            containerRect.offsetMin = new Vector2(0, -300 - (addHeight-1000));

        text.text = txt;
        closeButton.onClick.AddListener(DestoryView);
    }

    private void DestoryView()
    {
        Views.instance.CloseView();
        Destroy(_viewObject);
    }
}
