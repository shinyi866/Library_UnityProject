using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

public class BookInfoModal : Modal
{
    [SerializeField]
    private Button moreInfoButton;

    [SerializeField]
    private Transform buttonTransform;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private GameObject imageObject;
    [SerializeField]
    private GameObject noImageObject;

    private Button leftButton;
    private Button rightButton;

    private Button remindLeftButton;
    private Button remindRightButton;    

    private Image bookCoverImage;
    private Button noImageButton;

    private void Awake()
    {
        backButton.onClick.AddListener(() => { Modals.instance.OpenModal<FindBookResultModal>(); });

        moreInfoButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<MoreInfoView>();
            view.ShowView(StringAsset.Test.testString);
        });
    }

    public void BookInfo()
    {
        titleText.text = StringAsset.BookInfo.info;

        DestoryView();
        // TODO: check API have book cover or not
        HaveBookCover(false);
        CreateButtons(StringAsset.BookInfo.saveStudy, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<RemindView>();
            view.ShowChooseRemindView(StringAsset.BookRemind.successToStudy);

            remindLeftButton = view.leftButton;
            remindLeftButton.onClick.AddListener(() =>
            {
                Modals.instance.OpenModal<MyStudyModal>();
                view.DestoryView();
            });

            remindRightButton = view.rightButton;
            remindRightButton.onClick.AddListener(() =>
            {                
                view.DestoryView();
                NotReadBook();
            });
        });

        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    public void NotReadBook()
    {
        titleText.text = StringAsset.BookInfo.notRead;

        DestoryView();
        CreateButtons(StringAsset.BookInfo.finish, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<LongView>();
            view.ShowMoodView();
        });

        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    public void ReadBook()
    {
        titleText.text = StringAsset.BookInfo.read;

        DestoryView();
        CreateButtons(StringAsset.BookInfo.change, StringAsset.BookInfo.findBook);

        leftButton.onClick.AddListener(() =>
        {
            var view = Views.instance.OpenView<LongView>();
            view.ShowClassifyView();
        });

        rightButton.onClick.AddListener(() =>
        {
            var modal = Modals.instance.OpenModal<GuideModal>();
            modal.ShowView(TypeFlag.GuideType.ARfindBook);
            // AR ibeacon
        });
    }

    private void CreateButtons(string leftString, string rightString)
    {
        leftButton = ButtonGenerate.Instance.SetModalButton(leftString, TypeFlag.UIColorType.Green);
        leftButton.transform.SetParent(buttonTransform);

        rightButton = ButtonGenerate.Instance.SetModalButton(rightString, TypeFlag.UIColorType.Orange);
        rightButton.transform.SetParent(buttonTransform);
    }

    private void HaveBookCover(bool isOpen)
    {
        imageObject.SetActive(isOpen);
        noImageObject.SetActive(!isOpen);

        bookCoverImage = imageObject.GetComponent<Image>();
        noImageButton = noImageObject.GetComponent<Button>();

        noImageButton.onClick.AddListener(()=> {
            var view = Views.instance.OpenView<RemindView>();
            view.ShowOriginRemindView(StringAsset.TakePicture.takePicture);

            view.button.onClick.AddListener(() => {
                view.DestoryView();
                Modals.instance.OpenModal<TakePictureModal>();
                //TODO: close bar
                //TODO: open camera
            });
        });
    }

    private void DestoryView()
    {
        if(leftButton != null)
            Destroy(leftButton.gameObject);

        if (rightButton != null)
            Destroy(rightButton.gameObject);
    }
}
