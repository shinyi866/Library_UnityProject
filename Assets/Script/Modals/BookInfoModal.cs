﻿using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using View;

public class BookInfoModal : Modal
{
    [SerializeField]
    private GameObject mainViewObject;

    [SerializeField]
    private GameObject itemObject;

    private GameObject topBarObject;
    private GameObject bookConatainerObject;
    private GameObject classifyObject;

    private Text barText;
    private Text bookTitle;

    private Image bookCoverImage;

    private Button backButton;
    private Button leftButton;
    private Button rightButton;
    private Button remindLeftButton;
    private Button remindRightButton;    
    private Button noImageButton;
    private Button moreInfoButton;

    private Transform buttonTransform;
    private AllItemObj allItemObj;
    private List<GameObject> buttons = new List<GameObject>();
    private List<GameObject> items = new List<GameObject>();
    private TypeFlag.ReadForm readForm = new TypeFlag.ReadForm();
    private TypeFlag.BookDatabaseType currentBookData;

    //private TypeFlag.BookDatabaseType bookData;

    private void Awake()
    {
        topBarObject = mainViewObject.transform.GetChild(0).gameObject;
        bookConatainerObject = mainViewObject.transform.GetChild(1).gameObject;
        classifyObject = mainViewObject.transform.GetChild(2).gameObject;
        buttonTransform = mainViewObject.transform.GetChild(3);

        barText = topBarObject.transform.GetChild(0).GetComponent<Text>();
        backButton = topBarObject.transform.GetChild(1).GetComponent<Button>();
        bookTitle = bookConatainerObject.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        bookCoverImage = bookConatainerObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        noImageButton = bookConatainerObject.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        moreInfoButton = bookConatainerObject.transform.GetChild(1).GetComponent<Button>();

        backButton.onClick.AddListener(() =>
        {
            Modals.instance.LastModal();
            //CleanAllSetting();
        });

        allItemObj = MainApp.Instance.itemData;
    }

    public void ShowBookInfo(TypeFlag.BookDatabaseType bookData)
    {
        CleanAllSetting();
        //CleanElementSetting();
        barText.text = StringAsset.BookInfo.info;

        LoadBookInfo(bookData);

        CreateButtons(StringAsset.BookInfo.saveStudy, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<RemindView>();
            view.ShowChooseRemindView(StringAsset.BookRemind.successToStudy);

            remindLeftButton = view.leftButton;
            remindLeftButton.onClick.AddListener(() =>
            {
                Modals.instance.OpenModal<MyStudyModal>();
                view.DestoryView();
            });

            remindRightButton = view.rightButton;
            remindRightButton.onClick.AddListener(() =>
            {
                view.DestoryView();
                PostBooks();
                // TODO: check read or not
                var _read = false; // online

                ChangeReadStatus(_read);
            });
        });

        GoARViewButtonClick();
    }

    public void ChangeReadStatus(bool isRead)
    {
        CleanElementSetting();

        var txt = isRead ? StringAsset.BookInfo.read : StringAsset.BookInfo.notRead;
        var leftButtonTxt = isRead ? StringAsset.BookInfo.change : StringAsset.BookInfo.finish;
        
        barText.text = txt;
        CreateButtons(leftButtonTxt, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<LongView>();

            if(isRead)
                view.ShowClassifyView(currentBookData);
            else
                view.ShowMoodView();
        });

        GoARViewButtonClick();
    }

    public void LoadBookAndChangeStatus(bool isRead, TypeFlag.BookDatabaseType bookData)
    {
        //CleanElementSetting();
        CleanAllSetting();
        LoadBookInfo(bookData);
        ChangeReadStatus(isRead);
    }

    private void LoadBookInfo(TypeFlag.BookDatabaseType bookData)
    {
        MainApp.Instance.currentBookData = bookData;
        currentBookData = bookData;
        bookTitle.text = bookData.name;
        HaveBookCover(bookData.picture);

        MoreInfoButtonClick(bookData.info);

        var count = bookData.classify.Count;

        for (int i = 0; i <= count; i++)
        {
            var item = Instantiate(itemObject, classifyObject.transform);
            var itemTxt = item.transform.GetChild(0).GetComponent<Text>();
            var itemImage = item.GetComponent<Image>();

            item.GetComponent<Button>().enabled = false;
            itemTxt.color = Color.white;
            items.Add(item);

            if (i == count)
            {
                var moodObj = allItemObj.moodItems[bookData.mood];

                itemImage.sprite = moodObj.image;
                itemTxt.text = moodObj.name;
            }
            else
            {
                var classifyString = bookData.classify[i];
                var classify = bookData.GetClassify(classifyString);

                try
                {
                    itemTxt.text = allItemObj.booksItems[classify.major].name[classify.minor];
                    itemImage.sprite = allItemObj.booksItems[classify.major].image[classify.minor];
                }
                catch
                {
                    Debug.Log("Can not find minor or major");
                }
            }
        }
    }

    private void PostBooks()
    {
        readForm.book_id = MainApp.Instance.currentBookData.book_id;
        readForm.user_id = "123"; // TODO: get user id
        string jsonString = JsonUtility.ToJson(readForm);

        string url = StringAsset.GetFullAPIUrl(StringAsset.API.Read);
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

    private void CreateButtons(string leftString, string rightString)
    {
        leftButton = ButtonGenerate.Instance.SetModalButton(leftString, TypeFlag.UIColorType.Green, buttonTransform);
        rightButton = ButtonGenerate.Instance.SetModalButton(rightString, TypeFlag.UIColorType.Orange, buttonTransform);

        buttons.Add(leftButton.transform.parent.gameObject);
        buttons.Add(rightButton.transform.parent.gameObject);
    }

    private void HaveBookCover(string picture)
    {
        var isOpen = !string.IsNullOrEmpty(picture);

        bookCoverImage.gameObject.SetActive(isOpen);
        noImageButton.gameObject.SetActive(!isOpen);

        if (isOpen)
        {
            StartCoroutine(APIRequest.GetImage(picture, (Sprite texture) => {
                bookCoverImage.sprite = texture;
            }));
        }
        else
        {            
            noImageButton.onClick.AddListener(() => {
                var view = Views.instance.OpenView<RemindView>();
                view.ShowOriginRemindView(StringAsset.TakePicture.takePicture);

                view.button.onClick.AddListener(() => {
                    view.DestoryView();
                    Modals.instance.OpenModal<TakePictureModal>();
                    Modals.instance.CloseBar(true);
                });
            });
        }
    }

    private void MoreInfoButtonClick(string _info)
    {
        moreInfoButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<MoreInfoView>();
            view.ShowView(_info);
        });
    }

    private void GoARViewButtonClick()
    {
        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    // Use is same page
    private void CleanElementSetting()
    {
        if (buttons.Count != 0)
        {
            foreach (var b in buttons) { Destroy(b); }
            buttons.Clear();
        }

        if (leftButton != null) { leftButton.onClick.RemoveAllListeners(); }
        if (rightButton != null) { rightButton.onClick.RemoveAllListeners(); }
    }

    // Use back button
    private void CleanAllSetting()
    {
        if (buttons.Count != 0)
        {
            foreach (var b in buttons) { Destroy(b); }
            buttons.Clear();
        }

        if(items.Count != 0)
        {
            foreach (var i in items) { Destroy(i); }
            items.Clear();
        }

        if(leftButton != null) { leftButton.onClick.RemoveAllListeners(); }
        if (rightButton != null) { rightButton.onClick.RemoveAllListeners(); }

        moreInfoButton.onClick.RemoveAllListeners();
    }
}
