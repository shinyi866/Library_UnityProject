using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using View;
using System.Collections.Generic;
using System.Linq;

public class BookInfoModal : Modal
{
    [SerializeField]
    private GameObject mainViewObject;

    [SerializeField]
    private GameObject itemObject;

    private GameObject topBarObject;
    private GameObject bookConatainerObject;
    private GameObject classifyObject;
    private GameObject buttonsObject;

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
    private AllItemObj AllItemObj;

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

        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookResultModal>(); });

        AllItemObj = MainApp.Instance.itemData;
    }

    public void BookInfo()
    {
        string getBooksUrl = StringAsset.API.GetBookInfo;
        barText.text = StringAsset.BookInfo.info;

        DestoryView();

        StartCoroutine(
            APIRequest.GetRequest(StringAsset.GetFullAPIUrl(getBooksUrl), UnityWebRequest.kHttpVerbGET, (string rawJson) => {

                if (string.IsNullOrEmpty(rawJson))
                    return;

                var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
                var bookData = data.ToList()[0];
                var hasCover = !string.IsNullOrEmpty(bookData.picture);

                bookTitle.text = bookData.name;
                HaveBookCover(hasCover);

                moreInfoButton.onClick.AddListener(() =>
                {
                    var view = Views.instance.OpenView<MoreInfoView>();
                    view.ShowView(bookData.info);
                });

                var count = bookData.classify.Count;

                for(int i = 0; i <= count; i++)
                {
                    var item = Instantiate(itemObject, classifyObject.transform);
                    var itemTxt = item.transform.GetChild(0).GetComponent<Text>();
                    var itemImage = item.GetComponent<Image>();

                    item.GetComponent<Button>().enabled = false;
                    itemTxt.color = Color.white;

                    if (i == count)
                    {
                        var moodObj = AllItemObj.moodItems[bookData.mood];

                        itemImage.sprite = moodObj.image;
                        itemTxt.text = moodObj.name;
                    }
                    else
                    {
                        var titleString = bookData.classify[i].id;
                        var classifyIndex = bookData.classify[i].name;
                        var itemObj = AllItemObj.booksTitleItems.ToList();
                        var index = itemObj.FindIndex(x => x.name == titleString);
                        Debug.Log("index " + index);
                        Debug.Log("classifyIndex " + classifyIndex);
                        Debug.Log("AllItemObj.booksItems[index].name[classifyIndex] " + AllItemObj.booksItems[index].name[classifyIndex]);
                        itemTxt.text = AllItemObj.booksItems[index].name[classifyIndex];
                        itemImage.sprite = AllItemObj.booksItems[index].image[classifyIndex];
                    }
                }
            })
        );

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
                NotReadBook();
            });
        });

        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    public void NotReadBook()
    {
        barText.text = StringAsset.BookInfo.notRead;

        DestoryView();
        CreateButtons(StringAsset.BookInfo.finish, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<LongView>();
            view.ShowMoodView();
        });

        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    public void ReadBook()
    {
        barText.text = StringAsset.BookInfo.read;

        DestoryView();
        CreateButtons(StringAsset.BookInfo.change, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<LongView>();
            view.ShowClassifyView();
        });

        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    private void CreateButtons(string leftString, string rightString)
    {
        leftButton = ButtonGenerate.Instance.SetModalButton(leftString, TypeFlag.UIColorType.Green);
        leftButton.transform.SetParent(buttonTransform);
        var leftButtonRect = leftButton.GetComponent<RectTransform>();
        leftButtonRect.localScale = new Vector3(1, 1, 1);

        rightButton = ButtonGenerate.Instance.SetModalButton(rightString, TypeFlag.UIColorType.Orange);
        rightButton.transform.SetParent(buttonTransform);
        var rightButtonRect = rightButton.GetComponent<RectTransform>();
        rightButtonRect.localScale = new Vector3(1, 1, 1);
    }

    private void HaveBookCover(bool isOpen)
    {
        bookCoverImage.gameObject.SetActive(isOpen);
        noImageButton.gameObject.SetActive(!isOpen);

        noImageButton.onClick.AddListener(()=> {
            var view = Views.instance.OpenView<RemindView>();
            view.ShowOriginRemindView(StringAsset.TakePicture.takePicture);

            view.button.onClick.AddListener(() => {
                view.DestoryView();
                Modals.instance.OpenModal<TakePictureModal>();
                //TODO: close bar
                //TODO: open camera
            });
        });
    }

    private void DestoryView()
    {
        if(leftButton != null)
            Destroy(leftButton.gameObject);

        if (rightButton != null)
            Destroy(rightButton.gameObject);
    }
}
