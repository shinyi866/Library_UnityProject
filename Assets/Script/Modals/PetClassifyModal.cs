﻿using System.Collections;
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
    private AllItemObj.PetsItem[] data;

    private void Awake()
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<MineModal>(); });
    }

    private void Start()
    {
        CreateItem();
    }

    private void CreateItem()
    {
        data = MainApp.Instance.itemData.petsItems;
        var dataLenght = data.Length;

        for (int i = 0; i < dataLenght; i++)
        {
            var itemObj = Instantiate(petItem, contentTransform);
            var button = itemObj.transform.GetChild(0).gameObject.GetComponent<Button>();
            var image = itemObj.transform.GetChild(0).gameObject.GetComponent<Image>();
            var txt = itemObj.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponentInChildren<Text>();

            image.sprite = data[i].image;
            txt.text = data[i].name;
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
                view.ShowImageRemindView_Pet(data[closureIndex].info, data[closureIndex].image);

                view.leftButton.onClick.AddListener(() =>
                {
                    view.DestoryView();
                });

                view.rightButton.onClick.AddListener(() =>
                {
                    view.DestoryView();

                    // TODO: change pet
                });
            });
        }
    }
}
