using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class TakePictureModal : Modal
{
    [SerializeField]
    private GameObject pictureObject;

    [SerializeField]
    private Transform buttonTransform;

    private Button button; //TODO: change to picture button
    private Button rightButton;

    private void Awake()
    {
        button = this.transform.GetChild(0).GetComponent<Button>();

        pictureObject.SetActive(false);

        var leftButton = ButtonGenerate.Instance.SetModalButton(StringAsset.TakePicture.again, TypeFlag.UIColorType.Green, buttonTransform);
        rightButton = ButtonGenerate.Instance.SetModalButton(StringAsset.TakePicture.upload, TypeFlag.UIColorType.Orange, buttonTransform);

        leftButton.onClick.AddListener(() => {
            pictureObject.SetActive(false);
            //TODO: close bar
            //TODO: open camera
        });

        rightButton.onClick.AddListener(() => {
            var view = Views.instance.OpenView<RemindView>();
            view.ShowOriginRemindView(StringAsset.TakePicture.receiveBook);
            view.button.onClick.AddListener(() => {
                view.DestoryView();
                rightButton.interactable = false;

                bool levelup = false; // TODO: check level

                if (levelup)
                {
                    var view1 = Views.instance.OpenView<RemindView>();
                    var str = string.Format(StringAsset.TakePicture.updateMessage, "巧巧");//TODO: pet name
                    view1.ShowOriginRemindView(str);
                    view1.button.onClick.AddListener(view.DestoryView);

                    //TODO: upload image, change image
                }
            });
        });
    }

    private void Start()
    {
        ShowView(); // take picture and show
    }

    // take picture and show
    public void ShowView()
    {
        button.onClick.AddListener(() => {
            pictureObject.SetActive(true);
            rightButton.interactable = true;
        });
    }
}
