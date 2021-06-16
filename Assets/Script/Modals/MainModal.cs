using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MainModal : Modal
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject upObject;
    [SerializeField]
    private GameObject downObject;

    private Button upButton;
    private Button downButton;

    private void Awake()
    {
        upButton = upObject.GetComponent<Button>();
        downButton = downObject.GetComponent<Button>();

        upButton.onClick.AddListener(RecommendBooks);
        downButton.onClick.AddListener(TopBooks);

        RecommendBooks();
    }

    // TODO: click upButton to Recommed Book API
    private void RecommendBooks()
    {
        SwitchToUpButtons(false);
        text.text = StringAsset.Main.recommend;
    }

    // TODO:click downButton to Top Book API
    private void TopBooks()
    {
        SwitchToUpButtons(true);
        text.text = StringAsset.Main.top;
    }

    private void SwitchToUpButtons(bool isOpen)
    {
        upObject.SetActive(isOpen);
        downObject.SetActive(!isOpen);
    }
}
