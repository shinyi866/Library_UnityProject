using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using UnityEngine.UI;

public class MyGoalModal : Modal
{
    [SerializeField]
    private GameObject itemObject;

    [SerializeField]
    private GameObject imageObject;

    [SerializeField]
    private Transform itemTransform;

    [SerializeField]
    private GoalItemObj itemObj;

    [SerializeField]
    private Button backButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<MineModal>();
        });
    }

    private void Start()
    {
        CreateItem();
    }

    private void CreateItem()
    {
        var count = StringAsset.Goal.goalArray.Length;

        for (int i = 0; i < count; i++)
        {
            var item = Instantiate(itemObject, itemTransform);
            var itemTxt = item.transform.GetChild(0).GetComponent<Text>();
            var panelTransform = item.transform.GetChild(1);

            itemTxt.text = StringAsset.Goal.goalArray[i];

            for (int j = 0; j < 5; j++)
            {
                var imageItme = Instantiate(imageObject, panelTransform);
                var image = imageItme.GetComponent<Image>();
                // TODO: get image info
                var all = 50;
                var read = 30;
                var perIcon = all / 5;

                var persent = read / perIcon;

                if(i == 0)
                {
                    if(j < persent)
                        image.sprite = itemObj.guideItem.book;

                    if(j == persent)
                    {
                        var imageObject = image.transform.GetChild(0).gameObject;
                        var persentTxt = image.transform.GetChild(0).GetChild(0).GetComponent<Text>();

                        imageObject.SetActive(true);
                        persentTxt.text = string.Format("{0} / {1}", read, all);
                    }

                }
                else if (j < persent && i == 1)
                {
                    image.sprite = itemObj.guideItem.happy;
                }
                else if(j < persent && i == 2)
                {
                    image.sprite = itemObj.guideItem.scared;
                }
                else if(j < persent && i == 3)
                {
                    image.sprite = itemObj.guideItem.wow;
                }
                else if(j < persent && i == 4)
                {
                    image.sprite = itemObj.guideItem.angry;
                }
                else if (j < persent && i == 5)
                {
                    image.sprite = itemObj.guideItem.hate;
                }
                else if (j < persent && i == 6)
                {
                    image.sprite = itemObj.guideItem.sad;
                }
                else
                {
                    image.sprite = itemObj.guideItem.non;
                }
            }

        }
    }
}
