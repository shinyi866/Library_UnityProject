using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using View;

public class FindBookResultModal : Modal
{
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private GameObject nullObject;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private GameObject itemObject;

    [SerializeField]
    private Transform contentTransform;

    private List<GameObject> list = new List<GameObject>();

    public void ClassifyResult(string url)
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookClassifyModal>(); });
        //titleText.text = text;

        FindBooks(url);
    }

    public void MoodAndNameSearchResult(string url)
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookModal>(); });
        //titleText.text = text;

        FindBooks(url);
    }

    public void FindBooks(string url)
    {
        CleanList();

        StartCoroutine(APIRequest.GetRequest(url, UnityWebRequest.kHttpVerbGET, (string rawJson) => {

            try
            {
                Debug.Log("success url" + url);

                var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
                var bookData = data.ToList();
                var count = bookData.Count;

                for (int i = count / 2; i < count; i++)
                {
                    CreateItem(i, bookData);
                    Debug.Log("1 i " + i);
                }

                for (int i = 0; i < count / 2; i++)
                {
                    CreateItem(i, bookData);
                    Debug.Log("2 i " + i);
                }
            }
            catch
            {
                var item = Instantiate(nullObject, contentTransform);
                list.Add(item);
            }

        }));
    }

    private void CreateItem(int i, List<TypeFlag.BookDatabaseType> bookData)
    {
        var item = Instantiate(itemObject, contentTransform);
        var itemImage = item.transform.GetChild(0).GetComponent<Image>();
        var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
        var itemButton = item.transform.GetChild(2).GetComponent<Button>();
        var closureIndex = i;
        var bookInfo = bookData[closureIndex];

        itemTxt.text = bookInfo.name;

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
}
