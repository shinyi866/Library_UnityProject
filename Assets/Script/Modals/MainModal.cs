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
        FindBooks(getBooksUrl);
    }

    private void TopBooks()
    {
        SwitchToUpButtons(true);
        text.text = StringAsset.Main.top;

        string getBooksUrl = StringAsset.GetFullAPIUrl(StringAsset.API.MostView);
        FindBooks(getBooksUrl);
    }

    private void FindBooks(string url)
    {
        CleanList();

        StartCoroutine(APIRequest.GetRequest(url, UnityWebRequest.kHttpVerbGET, (string rawJson) => {
            if (string.IsNullOrEmpty(rawJson))
                return;

            Debug.Log("success");

            var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
            var bookData = data.ToList();
            var count = bookData.Count;

            for (int i = count - 1; i >= count / 2; i--)
            {
                var item = scrollSnap.AddToFront(itemObject);
                CreateItem(item, i, bookData);
            }

            for (int i = 0; i < count / 2; i++)
            {
                var item = scrollSnap.AddToBack(itemObject);
                CreateItem(item, i, bookData);
            }

            scrollSnap.startingPanel = count / 2;
            scrollSnap.enabled = true;

        }));
    }

    private void CreateItem(GameObject item, int i, List<TypeFlag.BookDatabaseType> bookData)
    {
        //var item = Instantiate(itemObject, contentTransform);
        var itemImage = item.transform.GetChild(0).GetComponent<Image>();
        var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
        var itemButton = item.GetComponent<Button>(); //item.transform.GetChild(2).GetComponent<Button>();
        var topImage = item.transform.GetChild(3).GetChild(0).GetComponent<Image>();
        var moodImage = item.transform.GetChild(3).GetChild(1).GetComponent<Image>();
        var closureIndex = i;
        var bookInfo = bookData[closureIndex];

        itemTxt.text = bookInfo.name;
        topImage.gameObject.SetActive(true);
        topImage.sprite = allItem.rankItems.topImage[i];
        moodImage.gameObject.SetActive(true);
        moodImage.sprite = allItem.moodItems[bookInfo.mood].image;

        if (bookInfo.picture != null)
        {
            StartCoroutine(APIRequest.GetTexture(bookInfo.picture, (Sprite texture) => {
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
        if (list != null)
        {
            foreach (var l in list) { Destroy(l); }
            list.Clear();
        }
    }

    private void SwitchToUpButtons(bool isOpen)
    {
        upObject.SetActive(isOpen);
        downObject.SetActive(!isOpen);
    }
}
