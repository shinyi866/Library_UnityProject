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

    private List<Transform> transformList = new List<Transform>(); 

    private void Awake()
    {
        backButton.onClick.AddListener(()=>
        {
            ClearList();
            Modals.instance.BackToLastModal();
        });
    }

    public void CreateItem(int index, string text)
    {
        var classifyData = MainApp.Instance.itemData.booksItems[index];
        var nameArray = classifyData.name;
        var imageArray = classifyData.image;

        titleText.text = text;

        for (int i = 0; i < nameArray.Length; i++)
        {
            var itemObj = Instantiate(classifyItem, container);
            //var button = itemObj.gameObject.GetComponent<Button>();
            var image = itemObj.gameObject.GetComponent<Image>();
            var txt = itemObj.gameObject.GetComponentInChildren<Text>();

            image.sprite = imageArray[i];
            txt.text = nameArray[i];
            transformList.Add(itemObj);
        }

    }

    private void ClearList()
    {
        if(transformList.Count != 0)
        {
            foreach(var t in transformList) { Destroy(t.gameObject); }
            foreach (var t in transformList) { Destroy(t.GetChild(0).gameObject); }

            transformList.Clear();
        }
    }
}
