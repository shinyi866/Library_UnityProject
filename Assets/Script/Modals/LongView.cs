using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class LongView : Modal
{
    [SerializeField]
    private GameObject viewObject;
    [SerializeField]
    private Transform item;

    private GameObject _viewObject;
    private Transform contentTransform;
    private Transform transformSelf;
    private Text text;
    private Button closeButton;
    private AllItemObj.MoodItem[] moodData;
    private AllItemObj.BookItem classifyData;
    private List<string> nameList = new List<string>();
    private List<Button> moodButtons = new List<Button>();
    private List<Button> bookButtons = new List<Button>();

    private void Awake()
    {
        transformSelf = this.transform;
    }

    public void ShowMoodView()
    {
        CreateView(StringAsset.BookRemind.moodView);
        CreateMoodItem();
    }

    public void ShowClassifyView()
    {
        // TODO: get book classify API
        CreateView(StringAsset.BookRemind.classifyView);
        CreateClassifyItem();
    }

    private void CreateClassifyItem()
    {
        classifyData = MainApp.Instance.itemData.booksItems[10]; //TODO: API book classifyData
        var bookLength = classifyData.image.Length;

        for (int i = 0; i < bookLength; i++)
        {
            var itemObj = Instantiate(item, contentTransform);
            var button = itemObj.gameObject.GetComponent<Button>();
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            image.sprite = classifyData.image[i];
            txt.text = classifyData.name[i];
            bookButtons.Add(button);
        }

        if (bookLength < 5)
            CreateAddButton();

        BookButtonClick();
    }

    private void CreateMoodItem()
    {
        moodData = MainApp.Instance.itemData.moodItems;
        var moodLength = moodData.Length;

        for (int i = 0; i < moodLength; i++)
        {
            var itemObj = Instantiate(item, contentTransform);
            var button = itemObj.gameObject.GetComponent<Button>();
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            image.sprite = moodData[i].image;
            txt.text = moodData[i].name;
            moodButtons.Add(button);
            nameList.Add(moodData[i].name);
        }

        MoodButtonClick();
    }

    private void CreateAddButton()
    {
        var itemObj = Instantiate(item, contentTransform);
        var button = itemObj.gameObject.GetComponent<Button>();
        var image = itemObj.gameObject.GetComponent<Image>();
        var txt = itemObj.gameObject.GetComponentInChildren<Text>();

        image.sprite = MainApp.Instance.itemData.booksItems[11].image[0];
        txt.text = MainApp.Instance.itemData.booksItems[11].name[0];

        button.onClick.AddListener(() =>
        {
            Modals.instance.OpenModal<BookClassifyModal>();
            DestoryView();
            Views.instance.CloseView();
        });
    }

    private void BookButtonClick()
    {
        for (int i = 0; i < bookButtons.Count; i++)
        {
            int closureIndex = i;

            bookButtons[closureIndex].onClick.AddListener(() =>
            {
                DestoryView();
                var view = Views.instance.OpenView<RemindView>();
                view.ShowOriginRemindView(StringAsset.BookRemind.receiveSuggest);

                view.button.onClick.AddListener(() =>
                {
                    view.DestoryView();

                    var modal = Modals.instance.OpenModal<BookInfoModal>();
                    modal.ReadBook();
                });

                // TODO: sent suggest
            });
        }
    }

    private void MoodButtonClick()
    {
        for (int i = 0; i < moodButtons.Count; i++)
        {
            int closureIndex = i;

            moodButtons[closureIndex].onClick.AddListener(() =>
            {
                DestoryView();
                var view = Views.instance.OpenView<RemindView>();
                var str = string.Format(StringAsset.BookRemind.mood, nameList[closureIndex]);
                
                view.ShowOriginRemindView(str);

                view.button.onClick.AddListener(() =>
                {                    
                    view.DestoryView();

                    var modal = Modals.instance.OpenModal<BookInfoModal>();
                    modal.ReadBook();
                });

                // TODO: add mood 50 power
            });
        }
    }

    private void CreateView(string titleText)
    {        
        _viewObject = Instantiate(viewObject, transformSelf);
        _viewObject.transform.SetParent(this.transform);

        var _viewRect = _viewObject.GetComponent<RectTransform>();
        _viewRect.anchoredPosition = new Vector2(0, 0);

        text = _viewObject.transform.GetChild(0).GetComponent<Text>();
        contentTransform = _viewObject.transform.GetChild(1);
        closeButton = _viewObject.transform.GetChild(2).gameObject.GetComponent<Button>();

        text.text = titleText;
        closeButton.onClick.AddListener(DestoryView);
    }

    private void DestoryView()
    {
        Views.instance.CloseView();
        Destroy(_viewObject);
    }
}