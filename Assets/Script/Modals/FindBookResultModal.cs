using DanielLochner.Assets.SimpleScrollSnap;
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
    private GameObject bookItemObject;

    [SerializeField]
    private GameObject catItemObject;

    [SerializeField]
    private Transform contentTransform;

    [SerializeField]
    private Transform catContentTransform;

    [SerializeField]
    private SimpleScrollSnap scrollSnap;

    public bool isCurrentPage;
    private AllItemObj allItemObj;
    private List<GameObject> list = new List<GameObject>();
    private List<GameObject> items = new List<GameObject>();
    private List<TypeFlag.BookDatabaseType> bookDatabases = new List<TypeFlag.BookDatabaseType>();

    private void Awake()
    {
        allItemObj = MainApp.Instance.itemData;

        backButton.onClick.AddListener(() =>
        {
            Modals.instance.LastModal();
            isCurrentPage = false;
        });
    }

    public void ClassifyResult(string url)
    {
        FindBooks(url);
        isCurrentPage = true;
    }

    public void MoodAndNameSearchResult(string url)
    {
        FindBooks(url);
        isCurrentPage = true;
    }

    public void FindBooks(string url)
    {
        CleanList();

        StartCoroutine(APIRequest.GetRequest(url, UnityWebRequest.kHttpVerbGET, (string rawJson) => {

            try
            {
                var data = JsonSerialization.FromJson<TypeFlag.BookDatabaseType>(rawJson);
                bookDatabases = data.ToList();
                var count = bookDatabases.Count;
                var half = count / 2;

                for (int i = count - 1; i >= half; i--)
                {
                    var item = scrollSnap.AddToFront(bookItemObject);
                    CreateItem(item, i);
                }

                for (int i = 0; i < half; i++)
                {
                    var item = scrollSnap.AddToBack(bookItemObject);
                    CreateItem(item, i);
                }

                CreateCat(half);

                scrollSnap.startingPanel = half;
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

    private void CreateItem(GameObject item, int i)
    {
        if (bookDatabases == null) return;
        //var item = Instantiate(itemObject, contentTransform);
        var itemImage = item.transform.GetChild(0).GetComponent<Image>();
        var itemTxt = item.transform.GetChild(1).GetComponent<Text>();
        var itemButton = item.GetComponent<Button>();
        var closureIndex = i;
        var bookInfo = bookDatabases[closureIndex];

        itemTxt.text = bookInfo.name;

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

    public void CreateCat(int inputIndex)
    {
        if (!isCurrentPage) return;

        CleanItems();
        
        var index = IndexSwitch(inputIndex);
        var bookInfo = bookDatabases[index];
        var count = bookInfo.classify.Count;
        
        for (int i = 0; i <= count; i++)
        {
            var item = Instantiate(catItemObject, catContentTransform);
            var itemTxt = item.transform.GetChild(0).GetComponent<Text>();
            var itemImage = item.GetComponent<Image>();

            item.GetComponent<Button>().enabled = false;
            itemTxt.color = Color.white;
            items.Add(item);

            if (i == count)
            {
                var moodObj = allItemObj.moodItems[bookInfo.mood];

                itemImage.sprite = moodObj.image;
                itemTxt.text = moodObj.name;
            }
            else
            {
                var classifyString = bookInfo.classify[i];
                var classify = bookInfo.GetClassify(classifyString);

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

    private void CleanList()
    {
        scrollSnap.enabled = false;

        for (int i = 0; i < list.Count / 2; i++)
        {
            scrollSnap.RemoveFromFront();
            scrollSnap.RemoveFromBack();
        }

        if (list != null)
            list.Clear();

        CleanItems();
    }

    private void CleanItems()
    {
        if (items != null)
        {
            foreach (var i in items) { Destroy(i.gameObject); }
            items.Clear();
        }
    }

    private int IndexSwitch(int index)
    {
        if (index >= 10)
            return index - 10;
        else
            return index + 10;
    }
}
