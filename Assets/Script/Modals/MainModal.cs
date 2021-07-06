using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using View;
using System.Linq;

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
    private Transform transform;

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

    private void Start()
    {
        ChangePet(MainApp.Instance.itemData.currentPet);
    }

    public void ChangePet(AllItemObj.PetsItem data)
    {
        image.sprite = data.image;
    }

    // TODO: click upButton to Recommed Book API
    private void RecommendBooks()
    {
        SwitchToUpButtons(false);
        text.text = StringAsset.Main.recommend;

        string getBooksUrl = StringAsset.API.GetBookInfo;
        StartCoroutine(APIRequest.GetRequest(StringAsset.GetFullAPIUrl(getBooksUrl), UnityWebRequest.kHttpVerbGET, (string rawJson) => {
            if (string.IsNullOrEmpty(rawJson))
                return;

            var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
            var bookData = data.ToList();
            var count = bookData.Count;

            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(itemObject, transform);
                var itemImage = item.transform.GetChild(0).GetComponent<Image>();
                var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
                var itemButton = item.transform.GetChild(2).GetComponent<Button>();
                var closureIndex = i;
                var bookInfo = bookData[closureIndex];

                itemTxt.text = bookInfo.name;
                //itemImage.sprite = bookInfo.picture; //TODO: pic trun tp sprit
                itemButton.onClick.AddListener(()=> {
                    Debug.Log("i" + closureIndex);
                    Modals.instance.OpenModal<BookInfoModal>().BookInfoLoad(bookInfo);
                });
            }
        }));
    }

    // TODO:click downButton to Top Book API
    private void TopBooks()
    {
        SwitchToUpButtons(true);
        text.text = StringAsset.Main.top;

        string getBooksUrl = StringAsset.API.GetBookInfo;
        StartCoroutine(APIRequest.GetRequest(StringAsset.GetFullAPIUrl(getBooksUrl), UnityWebRequest.kHttpVerbGET, (string rawJson) => {
            if (string.IsNullOrEmpty(rawJson))
                return;

            var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
            var bookData = data.ToList();
            var count = bookData.Count;

            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(itemObject, transform);
                var itemImage = item.transform.GetChild(0).GetComponent<Image>();
                var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
                var itemButton = item.transform.GetChild(2).GetComponent<Button>();
                var bookInfo = bookData[i];

                itemTxt.text = bookInfo.name;
                //itemImage.sprite = bookInfo.picture; //TODO: pic trun tp sprit
            }
        }));
    }

    private void SwitchToUpButtons(bool isOpen)
    {
        upObject.SetActive(isOpen);
        downObject.SetActive(!isOpen);
    }
}
