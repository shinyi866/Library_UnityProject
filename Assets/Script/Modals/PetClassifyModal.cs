using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class PetClassifyModal : Modal
{
    [SerializeField]
    private GameObject petItem;

    [SerializeField]
    private Transform contentTransform;

    [SerializeField]
    private Button backButton;

    private Image image;
    private Button button;
    private List<Button> buttons = new List<Button>();
    private AllItemObj data;
    private AllItemObj.PetsItem[] petsItems;

    private void Awake()
    {
        backButton.onClick.AddListener(() =>
        {
            //Modals.instance.OpenModal<MineModal>();
            Modals.instance.LastModal();
        });
    }

    private void Start()
    {
        CreateItem();
    }

    private void CreateItem()
    {
        data = MainApp.Instance.itemData;
        petsItems = data.petsItems;

        var dataLenght = petsItems.Length;

        for (int i = 0; i < dataLenght; i++)
        {
            var itemObj = Instantiate(petItem, contentTransform);
            var button = itemObj.transform.GetChild(0).gameObject.GetComponent<Button>();
            var image = itemObj.transform.GetChild(0).gameObject.GetComponent<Image>();
            var txt = itemObj.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponentInChildren<Text>();

            image.sprite = petsItems[i].image;
            txt.text = petsItems[i].name;
            buttons.Add(button);
        }

        BookButtonClick();
    }

    private void BookButtonClick()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int closureIndex = i;

            buttons[closureIndex].onClick.AddListener(() =>
            {
                var view = Views.instance.OpenView<RemindView>();
                view.ShowImageRemindView_Pet(petsItems[closureIndex].info, petsItems[closureIndex].image);

                view.leftButton.onClick.AddListener(() =>
                {
                    view.DestoryView();
                });

                view.rightButton.onClick.AddListener(() =>
                {
                    view.DestoryView();

                    Modals.instance.ChangePet(data, closureIndex);
                    bgImage.sprite = petsItems[closureIndex].bgImage;
                });
            });
        }
    }
}
