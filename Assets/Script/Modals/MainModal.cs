using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using View;
using System.Linq;
using DanielLochner.Assets.SimpleScrollSnap;

public class MainModal : Modal
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;
    [SerializeField]
    private GameObject upObject;
    [SerializeField]
    private GameObject downObject;
    [SerializeField]
    private GameObject itemObject;
    [SerializeField]
    private Transform contentTransform;
    [SerializeField]
    private SimpleScrollSnap scrollSnap;
    [SerializeField]
    private GameObject nullObject;

    private Button upButton;
    private Button downButton;
    private AllItemObj allItem;
    private List<GameObject> list = new List<GameObject>();
    //private List<float> centerPosX = new List<float>();

    private void Awake()
    {
        upButton = upObject.GetComponent<Button>();
        downButton = downObject.GetComponent<Button>();

        upButton.onClick.AddListener(RecommendBooks);
        downButton.onClick.AddListener(TopBooks);

        allItem = MainApp.Instance.itemData;
    }

    private void Start()
    {
        ChangePet(allItem.currentPet);
        //RecommendBooks();
        TopBooks();
    }

    public void ChangePet(AllItemObj.PetsItem data)
    {
        image.sprite = data.image;
    }

    private void RecommendBooks()
    {
        SwitchToUpButtons(false);
        text.text = StringAsset.Main.recommend;
        
        string getBooksUrl = StringAsset.GetFullAPIUrl(StringAsset.API.Recommend);
        FindBooks(getBooksUrl, false);
    }

    private void TopBooks()
    {
        SwitchToUpButtons(true);
        text.text = StringAsset.Main.top;

        string getBooksUrl = StringAsset.GetFullAPIUrl(StringAsset.API.MostView);
        FindBooks(getBooksUrl, true);
    }

    private void FindBooks(string url, bool isTopView)
    {
        StartCoroutine(APIRequest.GetRequest(url, UnityWebRequest.kHttpVerbGET, (string rawJson) => {
            try
            {
                var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
                var bookData = data.ToList();
                var count = bookData.Count;

                for (int i = count - 1; i >= count / 2; i--)
                {
                    var item = scrollSnap.AddToFront(itemObject);
                    CreateItem(item, i, bookData, isTopView);
                }

                for (int i = 0; i < count / 2; i++)
                {
                    var item = scrollSnap.AddToBack(itemObject);
                    CreateItem(item, i, bookData, isTopView);
                }

                scrollSnap.startingPanel = count / 2;
                scrollSnap.enabled = true;
                scrollSnap.ReSet();
            }
            catch
            {
                var item = Instantiate(nullObject, contentTransform);
                list.Add(item);
            }

        }));
    }

    private void CreateItem(GameObject item, int i, List<TypeFlag.BookDatabaseType> bookData, bool isTopView)
    {
        //var item = Instantiate(itemObject, contentTransform);
        var itemImage = item.transform.GetChild(0).GetComponent<Image>();
        var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
        var itemButton = item.GetComponent<Button>();
        var topImage = item.transform.GetChild(3).GetChild(0).GetComponent<Image>();
        var moodImage = item.transform.GetChild(3).GetChild(1).GetComponent<Image>();
        var closureIndex = i;
        var bookInfo = bookData[closureIndex];

        itemTxt.text = bookInfo.name;
        moodImage.gameObject.SetActive(true);
        moodImage.sprite = allItem.moodItems[bookInfo.mood].image;
        topImage.gameObject.SetActive(isTopView);

        if (isTopView)
            topImage.sprite = allItem.rankItems.topImage[i];

        if (bookInfo.picture != null)
        {
            StartCoroutine(APIRequest.GetImage(bookInfo.picture, (Sprite texture) => {
                itemImage.sprite = texture;
            }));
        }

        list.Add(item);
        
        itemButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<BookInfoModal>().ShowBookInfo(bookInfo);
        });
    }

    private void CleanList()
    {
        for (int i = 0; i < list.Count / 2; i++)
        {
            scrollSnap.RemoveFromFront();
            scrollSnap.RemoveFromBack();
        }

        if (list != null)
        {            
            list.Clear();
        }
    }

    private void SwitchToUpButtons(bool isOpen)
    {
        scrollSnap.enabled = false;
        CleanList();
        upObject.SetActive(isOpen);
        downObject.SetActive(!isOpen);
    }
}
