using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class FindBookClassifyModal : Modal
{
    [SerializeField]
    private Transform container;
    [SerializeField]
    private Transform classifyItem;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Button backButton;

    private List<GameObject> classifyList = new List<GameObject>();
    private AllItemObj.BookItem classifyData;

    private void Awake()
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookModal>(); });
    }

    public void CreateItem(int index, string text)
    {
        ClearList();

        classifyData = MainApp.Instance.itemData.booksItems[index];
        var nameArray = classifyData.name;
        var imageArray = classifyData.image;

        titleText.text = text;

        for (int i = 0; i < nameArray.Length; i++)
        {
            var itemObj = Instantiate(classifyItem, container);
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            image.sprite = imageArray[i];
            txt.text = nameArray[i];
            classifyList.Add(itemObj.gameObject);
        }

        BookButtonClick();
    }

    private void BookButtonClick()
    {
        for (int i = 0; i < classifyList.Count; i++)
        {
            int closureIndex = i;

            classifyList[closureIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Search(closureIndex);
            });
        }
    }

    private void Search(int index)
    {
        var txt = classifyData.name[index];

        string getBooksUrl = StringAsset.GetSearchAPIUrl(txt);
        var modal = Modals.instance.OpenModal<FindBookResultModal>();
        modal.ClassifyResult(getBooksUrl);
    }

    private void ClearList()
    {
        if(classifyList.Count != 0)
        {
            foreach(var t in classifyList) { Destroy(t); }

            classifyList.Clear();
        }
    }
}
