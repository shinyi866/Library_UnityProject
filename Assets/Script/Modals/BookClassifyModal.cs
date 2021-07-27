using UnityEngine.Networking;
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

    
    private TypeFlag.CatForm catForm = new TypeFlag.CatForm();
    private AllItemObj.BookTitleItem[] titleItems;
    private AllItemObj.BookItem classifyData;
    private List<Button> bookButtons = new List<Button>();
    private List<GameObject> classifyList = new List<GameObject>();
    private bool isColoChange;
    private int leftButtonIndex;

    private void Awake()
    {
        backButton.onClick.AddListener(() => { Modals.instance.LastModal(); });
    }

    private void Start()
    {
        CreateHorizontalItem();
    }

    private void CreateHorizontalItem()
    {
        leftButtonIndex = 0;
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
        DefaultView(leftButtonIndex);
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

                DefaultView(leftButtonIndex);

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
                        catForm.book_id = MainApp.Instance.currentBookData.book_id;
                        catForm.cat = classifyData.name[closureIndex];
                        string jsonString = JsonUtility.ToJson(catForm);
                        
                        string url = StringAsset.GetFullAPIUrl(StringAsset.API.Cat);
                        StartCoroutine(APIRequest.PostJson(url, UnityWebRequest.kHttpVerbPOST, StringAsset.PostType.json, jsonString, (bool result)=>
                        {
                            if(result)
                            {
                                view.DestoryView();
                            }
                            else
                            {
                                view.ShowOriginRemindView(StringAsset.BookRemind.sendFail);
                                view.button.onClick.AddListener(view.DestoryView);
                            }
                        }));
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
            var txt = bookButtons[closureIndex].transform.GetChild(0).GetComponent<Text>();

            bookButtons[closureIndex].onClick.AddListener(() =>
            {
                for (int j = 0; j < bookButtons.Count; j++)
                    bookButtons[j].transform.GetChild(0).GetComponent<Text>().color = Color.black;

                if (!isColoChange)
                {
                    bookButtons[leftButtonIndex].image.color = Color.white;
                    isColoChange = true;
                }

                leftButtonIndex = closureIndex;
                txt.color = Color.white;
                CreateVerticalItem(closureIndex);                
            });
        }   
    }

    private void DefaultView(int index)
    {
        bookButtons[index].image.color = MainApp.Instance.uiColorData.GetUIColor(TypeFlag.UIColorType.Lias).color;        
        bookButtons[index].transform.GetChild(0).GetComponent<Text>().color = Color.white;
        CreateVerticalItem(index);

        isColoChange = false;
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
