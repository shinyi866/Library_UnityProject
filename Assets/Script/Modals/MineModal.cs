using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class MineModal : Modal
{
    [SerializeField]
    private Text petName;

    [SerializeField]
    private Image petImage;

    [SerializeField]
    private Button changeButton;

    [SerializeField]
    private Transform buttonTransform;

    private Button leftButton;
    private Button rightButton;

    private void Awake()
    {
        changeButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<PetClassifyModal>();
        });
    }

    private void Start()
    {
        //data = MainApp.Instance.itemData;
        LoadPet(MainApp.Instance.itemData.currentPet);
        CreateButtons(StringAsset.Mine.achievement, StringAsset.Mine.study);

        rightButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<MyStudyModal>();
        });

        leftButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<MyGoalModal>();
        });
    }

    public void LoadPet(AllItemObj.PetsItem data)
    {
        Debug.Log("current " + data.name);
        petName.text = data.name;
        petImage.sprite = data.image;
    }

    private void CreateButtons(string leftString, string rightString)
    {
        leftButton = ButtonGenerate.Instance.SetModalButton(leftString, TypeFlag.UIColorType.Green);
        leftButton.transform.SetParent(buttonTransform);
        var leftButtonRect = leftButton.GetComponent<RectTransform>();
        leftButtonRect.localScale = new Vector3(1, 1, 1);

        rightButton = ButtonGenerate.Instance.SetModalButton(rightString, TypeFlag.UIColorType.Orange);
        rightButton.transform.SetParent(buttonTransform);
        var rightButtonRect = rightButton.GetComponent<RectTransform>();
        rightButtonRect.localScale = new Vector3(1, 1, 1);
    }
}
