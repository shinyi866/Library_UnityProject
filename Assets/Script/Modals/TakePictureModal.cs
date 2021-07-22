using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using View;
using System.IO;

public class TakePictureModal : Modal
{
    [SerializeField]
    private GameObject pictureObject;

    [SerializeField]
    private Image picture;

    [SerializeField]
    private Transform buttonTransform;

    [SerializeField]
    private Button backButton;

    private TypeFlag.CoverForm coverForm = new TypeFlag.CoverForm();
    private Button takePictureButton;
    private Button rightButton;
    private Texture2D currentImage;

    private void Awake()
    {        
        pictureObject.SetActive(false);

        takePictureButton = this.transform.GetChild(0).GetComponent<Button>();

        backButton.onClick.AddListener(() => {
            Modals.instance.OpenModal<BookInfoModal>();
            Modals.instance.CloseBar(true);
            picture.sprite = null;
        });

        takePictureButton.onClick.AddListener(() => {
            TakePicture();
            pictureObject.SetActive(true);
            rightButton.interactable = true;
        });

        var leftButton = ButtonGenerate.Instance.SetModalButton(StringAsset.TakePicture.again, TypeFlag.UIColorType.Green, buttonTransform);
        rightButton = ButtonGenerate.Instance.SetModalButton(StringAsset.TakePicture.upload, TypeFlag.UIColorType.Orange, buttonTransform);

        leftButton.onClick.AddListener(() => {
            pictureObject.SetActive(false);
        });

        rightButton.onClick.AddListener(() => {
            var view = Views.instance.OpenView<RemindView>();
            view.ShowOriginRemindView(StringAsset.TakePicture.receiveBook);
            view.button.onClick.AddListener(() => {

                UploadImage(view);

                bool levelup = false; // TODO: check level

                if (levelup)
                {
                    var view1 = Views.instance.OpenView<RemindView>();
                    var str = string.Format(StringAsset.TakePicture.updateMessage, "巧巧");//TODO: pet name
                    view1.ShowOriginRemindView(str);
                    view1.button.onClick.AddListener(view.DestoryView);                 
                }                
            });
        });
    }

    private void UploadImage(RemindView view)
    {
        coverForm.book_id = MainApp.Instance.currentBookData.book_id;
        coverForm.f = currentImage.EncodeToPNG();

        string url = StringAsset.GetFullAPIUrl(StringAsset.API.Cover);
        StartCoroutine(APIRequest.PostFormData(url, StringAsset.PostType.formData, coverForm, (bool result) =>
        {
            if (result)
            {
                picture.sprite = null;
                Destroy(currentImage);
                rightButton.interactable = false;
                view.DestoryView();
            }
            else
            {
                view.ShowOriginRemindView(StringAsset.BookRemind.sendFail);
                view.button.onClick.AddListener(view.DestoryView);
            }
        }));
    }

    private void TakePicture()
    {
        StartCoroutine(RenderScreenShot());
    }


    private IEnumerator RenderScreenShot()
    {
        Camera _camera = Camera.main;

        yield return new WaitForSeconds(0.1f);

        _camera.targetTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 1);  // (440, 250, 1); // (_camera.pixelWidth, _camera.pixelHeight, 1);

        RenderTexture renderTexture = _camera.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        _camera.Render();
        RenderTexture.active = renderTexture;
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

        renderResult.ReadPixels(rect, 0, 0);
        renderResult.Apply();

        Sprite screenShot = Sprite.Create(renderResult, rect, Vector2.zero);
        picture.sprite = screenShot;
        currentImage = renderResult;

        _camera.targetTexture = null;
    }

    private void SaveImage()
    {
        if (currentImage == null) return;

        // save in memory
        string filename = FileName();
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(currentImage, "Gallery", filename, (success, path) => Debug.Log("Media save result: " + success + " " + path));
    }

    private string FileName()
    {
        return string.Format("screen_{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
