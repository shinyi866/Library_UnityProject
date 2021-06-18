using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class BookClassifyModal : Modal
{
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Transform leftTransform;

    [SerializeField]
    private Transform rightTransform;

    [SerializeField]
    private Transform item_V;

    [SerializeField]
    private Transform item_H;

    private AllItemObj.BookTitleItem[] titleItems;
    private AllItemObj.BookItem classifyData;
    private List<Button> bookButtons = new List<Button>();
    private List<GameObject> classifyList = new List<GameObject>();

    private void Awake()
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<BookInfoModal>(); });
    }

    private void Start()
    {
        CreateHorizontalItem();
        DefaultView();
    }

    private void CreateHorizontalItem()
    {
        titleItems = MainApp.Instance.itemData.booksTitleItems;
        var bookLength = titleItems.Length;

        for (int i = 0; i < bookLength; i++)
        {
            var itemObj = Instantiate(item_H, leftTransform);
            var button = itemObj.gameObject.GetComponent<Button>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            txt.text = titleItems[i].name;
            bookButtons.Add(button);
        }

        BookButtonClick();
    }

    private void CreateVerticalItem(int index)
    {
        ClearList();

        classifyData = MainApp.Instance.itemData.booksItems[index];
        var nameArray = classifyData.name;
        var imageArray = classifyData.image;

        for (int i = 0; i < nameArray.Length; i++)
        {
            var itemObj = Instantiate(item_V, rightTransform);
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            image.sprite = imageArray[i];
            txt.text = nameArray[i];
            classifyList.Add(itemObj.gameObject);
        }

        ClassifyButtonClick(index);        
    }

    private void ClassifyButtonClick(int titleIndex)
    {
        for (int i = 0; i < classifyList.Count; i++)
        {
            int closureIndex = i;

            classifyList[closureIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                var view = Views.instance.OpenView<RemindView>();
                var str = string.Format(StringAsset.BookRemind.classifySuggest, titleItems[titleIndex].name, classifyData.name[closureIndex]);

                view.ShowImageRemindView(str, classifyData.image[closureIndex]);
                var leftButton = view.leftButton;
                var rightButton = view.rightButton;

                leftButton.onClick.AddListener(view.DestoryView);

                rightButton.onClick.AddListener(() => {
                    view.DestoryView();

                    view = Views.instance.OpenView<RemindView>();
                    view.ShowOriginRemindView(StringAsset.BookRemind.receiveSuggest);

                    var button = view.button;
                    button.onClick.AddListener(()=>
                    {
                        // TODO: button send API suggest
                        view.DestoryView();
                    });
                    
                });
            });
        }
    }

    private void BookButtonClick()
    {
        for (int i = 0; i < bookButtons.Count; i++)
        {
            int closureIndex = i;

            bookButtons[closureIndex].onClick.AddListener(() =>
            {
                CreateVerticalItem(closureIndex);
            });
        }
    }

    private void DefaultView()
    {
        bookButtons[0].Select();
        CreateVerticalItem(0);
    }

    private void ClearList()
    {
        if (classifyList.Count != 0)
        {
            foreach (var t in classifyList) { Destroy(t); }

            classifyList.Clear();
        }
    }
}
