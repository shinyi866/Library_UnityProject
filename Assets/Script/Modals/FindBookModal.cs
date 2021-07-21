using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class FindBookModal : Modal
{
    [SerializeField]
    private Transform bookContainerUp;
    [SerializeField]
    private Transform bookContainerDown;
    [SerializeField]
    private Transform moodcontainer;
    [SerializeField]
    private Transform bookItem;
    [SerializeField]
    private Transform moodItem;
    [SerializeField]
    private Button searchButton;
    [SerializeField]
    private InputField inputField;

    private AllItemObj.BookTitleItem[] titleItems;
    private AllItemObj.MoodItem[] moodData;
    private List<Button> bookButtons = new List<Button>();
    private List<Button> moodButtons = new List<Button>();

    private void Start()
    {
        CreateItem();

        searchButton.onClick.AddListener(() => {

            if (string.IsNullOrEmpty(inputField.text))
                return;

            var txt = inputField.text;
            inputField.text = "";
            Search(txt);
        });
    }

    private void CreateItem()
    {
        titleItems = MainApp.Instance.itemData.booksTitleItems;
        moodData = MainApp.Instance.itemData.moodItems;
        var bookLength = titleItems.Length;
        var moodLength = moodData.Length;

        for(int i = 0; i < bookLength; i++)
        {
            if( i < bookLength / 2)
            {
                var itemObj = Instantiate(bookItem, bookContainerDown);
                var button = itemObj.gameObject.GetComponent<Button>();
                var image = itemObj.gameObject.GetComponent<Image>();
                var txt = itemObj.gameObject.GetComponentInChildren<Text>();

                image.sprite = titleItems[i].image;
                txt.text = titleItems[i].name;
                bookButtons.Add(button);
            }
            else
            {
                var itemObj = Instantiate(bookItem, bookContainerUp);
                var button = itemObj.gameObject.GetComponent<Button>();
                var image = itemObj.gameObject.GetComponent<Image>();
                var txt = itemObj.gameObject.GetComponentInChildren<Text>();

                image.sprite = titleItems[i].image;
                txt.text = titleItems[i].name;
                bookButtons.Add(button);
            }
        }

        for(int i = 0; i < moodLength; i++)
        {
            var itemObj = Instantiate(moodItem, moodcontainer);
            var button = itemObj.gameObject.GetComponent<Button>();
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            image.sprite = moodData[i].image;
            txt.text = moodData[i].name;
            moodButtons.Add(button);
        }

        BookButtonClick();
        MoodButtonClick();
    }

    private void BookButtonClick()
    {
        for(int i = 0; i < bookButtons.Count; i++)
        {
            int closureIndex = i;

            bookButtons[closureIndex].onClick.AddListener(() =>
            {
                var modal = Modals.instance.OpenModal<FindBookClassifyModal>();
                modal.CreateItem(closureIndex, titleItems[closureIndex].name);
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
                var txt = moodData[closureIndex].name;
                Search(txt);
            });
        }
    }

    private void Search(string txt)
    {
        string getBooksUrl = StringAsset.GetSearchAPIUrl(txt);
        var modal = Modals.instance.OpenModal<FindBookResultModal>();
        modal.MoodAndNameSearchResult(getBooksUrl);
    }
}
