using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;
using UnityEngine.Networking;
using System.Linq;

public class MyStudyModal : Modal
{
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Button readButton;
    [SerializeField]
    private Button notReadButton;
    [SerializeField]
    private GameObject itemObject;
    [SerializeField]
    private Transform itemTransform;
    [SerializeField]
    private Text persentText;
    [SerializeField]
    private Slider slider;

    private RectTransform containerRect;
    private Text readText;
    private Text notReadText;

    private void Awake()
    {
        containerRect = itemTransform.GetComponent<RectTransform>();
        backButton.onClick.AddListener(() => { Modals.instance.LastModal(); });
        readText = readButton.transform.GetChild(0).GetComponent<Text>();
        notReadText = notReadButton.transform.GetChild(0).GetComponent<Text>();
    }


    private void Start()
    {
        ReadBooks(false);

        readButton.onClick.AddListener(()=> ReadBooks(true));
        notReadButton.onClick.AddListener(() => ReadBooks(false));
    }

    private void ReadBooks(bool isRead)
    {
        var changColor = isRead ? readText : notReadText;
        var notChangColor = !isRead ? readText : notReadText;
        changColor.color = Color.white;
        notChangColor.color = Color.black;

        string url = StringAsset.GetFullAPIUrl(StringAsset.API.MostView);

        StartCoroutine(APIRequest.GetRequest(url, UnityWebRequest.kHttpVerbGET, (string rawJson) => {
            if (string.IsNullOrEmpty(rawJson))
                return;

            var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
            var bookData = data.ToList();
            var count = bookData.Count;
            float persent = ((float)count / 30);

            CaculateItemContent(count);

            persentText.text = string.Format("{0} / 30", count);
            slider.value = persent;
            Debug.Log("persent " + persent);

            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(itemObject, itemTransform);
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

                itemButton.onClick.AddListener(() => {
                    Modals.instance.OpenModal<BookInfoModal>().BookInfoLoad(bookInfo);
                });
            }
        }));
    }

    private void CaculateItemContent(int number)
    {
        if (number <= 4) return;

        var n = (number - 4) / 2;
        var addHeight = n * 475;

        containerRect.sizeDelta = new Vector2(770, addHeight + 1200);
    }
}
