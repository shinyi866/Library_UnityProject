using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class Modal : MonoBehaviour
    {
        [SerializeField]
        protected CanvasGroup canvasGroup;
        [SerializeField]
        protected Image bgImage;

        public virtual void Show(bool isShow)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.interactable = isShow;
                canvasGroup.blocksRaycasts = isShow;

                bgImage.sprite = MainApp.Instance.itemData.currentPet.bgImage;
            }
        }
    }
}

