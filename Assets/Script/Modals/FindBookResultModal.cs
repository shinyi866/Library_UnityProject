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
    private Button bookButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private GameObject itemObject;

    [SerializeField]
    private Transform contentTransform;

    private List<GameObject> list = new List<GameObject>();

    public void BookResult(int index, string text)
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookClassifyModal>(); });
        titleText.text = text;

        bookButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<BookInfoModal>();
            // TODO: read ro not?
            modal.BookInfo();
        });
    }

    public void MoodResult(int index, string text)
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookModal>(); });
        titleText.text = text;

        bookButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<BookInfoModal>();
            // TODO: read ro not?
            modal.BookInfo();
        });
    }

    public void FindBooks(string url)
    {
        CleanList();

        StartCoroutine(APIRequest.GetRequest(url, UnityWebRequest.kHttpVerbGET, (string rawJson) => {
            if (string.IsNullOrEmpty(rawJson))
                return;

            Debug.Log("success");

            var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
            var bookData = data.ToList();
            var count = bookData.Count;

            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(itemObject, contentTransform);
                var itemImage = item.transform.GetChild(0).GetComponent<Image>();
                var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
                var itemButton = item.transform.GetChild(2).GetComponent<Button>();
                var closureIndex = i;
                var bookInfo = bookData[closureIndex];

                itemTxt.text = bookInfo.name;

                list.Add(item);
                itemButton.onClick.AddListener(() => {
                    Modals.instance.OpenModal<BookInfoModal>().BookInfoLoad(bookInfo);
                });
            }
        }));
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
