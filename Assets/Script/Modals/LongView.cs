using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class LongView : BoxView
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
    private AllItemObj AllItemObj;
    private List<string> nameList = new List<string>();
    private List<Button> moodButtons = new List<Button>();
    private List<Button> bookButtons = new List<Button>();
    private TypeFlag.MoodForm moodForm = new TypeFlag.MoodForm();

    private void Awake()
    {
        transformSelf = this.transform;
        AllItemObj = MainApp.Instance.itemData;
    }

    public void ShowMoodView()
    {
        CreateView(StringAsset.BookRemind.moodView);
        CreateMoodItem();
    }

    public void ShowClassifyView(TypeFlag.BookDatabaseType bookData)
    {
        CreateView(StringAsset.BookRemind.classifyView);
        CreateClassifyItem(bookData);
    }

    private void CreateClassifyItem(TypeFlag.BookDatabaseType bookData)
    {
        var count = bookData.classify.Count;

        for (int i = 0; i < count; i++)
        {
            var itemObj = Instantiate(item, contentTransform);
            var button = itemObj.gameObject.GetComponent<Button>();
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            var classifyString = bookData.classify[i];
            var classify = bookData.GetClassify(classifyString);

            try
            {
                txt.text = AllItemObj.booksItems[classify.major].name[classify.minor];
                image.sprite = AllItemObj.booksItems[classify.major].image[classify.minor];
                SendFeedBack(button, false);
                bookButtons.Add(button);
            }
            catch
            {
                Debug.Log("Can not find minor or major");
            }
        }

        if (count < 5)
            CreateAddButton();

        BookButtonClick();
    }

    private void CreateMoodItem()
    {
        var moodData = AllItemObj.moodItems;
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

        SendFeedBack(button, true);
    }

    private void SendFeedBack(Button button, bool goToCatModal)
    {
        button.onClick.AddListener(() =>
        {
            if (goToCatModal)
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
                    modal.ChangeReadStatus(true);
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
                    PostMood(closureIndex);

                    Modals.instance.GetModel<MineModal>().AddScore(50);

                    var modal = Modals.instance.GetModel<BookInfoModal>();
                    modal.ChangeReadStatus(true);
                });
            });
        }
    }

    private void PostMood(int moodIndex)
    {
        moodForm.book_id = MainApp.Instance.currentBookData.book_id;
        moodForm.user_id = "123"; // TODO: get user id
        moodForm.mood = moodIndex;

        string jsonString = JsonUtility.ToJson(moodForm);
        string moodUrl = string.Format(StringAsset.API.Read, StringAsset.API.Mood);

        string url = StringAsset.GetFullAPIUrl(moodUrl);
        StartCoroutine(APIRequest.PostJson(url, UnityWebRequest.kHttpVerbPOST, StringAsset.PostType.json, jsonString, (bool result) =>
        {
            if (result)
            {
                Debug.Log("Post success");
            }
            else
            {
                Debug.Log("Post fail");
            }
        }));
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