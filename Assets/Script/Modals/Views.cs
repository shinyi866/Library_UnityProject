using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    public class Views : MonoBehaviour
    {
        private BoxView[] views;

        private static Views _instance;

        public static Views instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Views>();
                    _instance.SetUp();
                }

                return _instance;
            }
        }

        private BoxView currentModal;

        public void SetUp()
        {
            views = GetComponentsInChildren<BoxView>();
        }

        public T GetView<T>() where T : BoxView
        {
            return views.First(x => typeof(T) == x.GetType()) as T;
        }

        public T OpenView<T>() where T : BoxView
        {
            if (views == null) return null;

            BoxView targetModal = null;

            foreach (BoxView modal in views)
            {
                modal.Show(false);

                if (typeof(T) == modal.GetType())
                {
                    targetModal = modal;
                    targetModal.Show(true);
                }

            }

            currentModal = targetModal;

            return targetModal as T;
        }

        public void CloseView()
        {
            if (currentModal != null)
                currentModal.Show(false);
        }

        public void CloseAllView()
        {
            foreach (BoxView modal in views) { modal.Show(false); }
        }
    }
}

