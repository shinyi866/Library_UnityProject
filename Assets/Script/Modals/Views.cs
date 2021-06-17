using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    public class Views : MonoBehaviour
    {
        private Modal[] modals;

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

        private Modal currentModal;

        public void SetUp()
        {
            modals = GetComponentsInChildren<Modal>();
        }

        public T GetView<T>() where T : Modal
        {
            return modals.First(x => typeof(T) == x.GetType()) as T;
        }

        public T OpenView<T>() where T : Modal
        {
            if (modals == null) return null;

            Modal targetModal = null;

            foreach (Modal modal in modals)
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
            foreach (Modal modal in modals) { modal.Show(false); }
        }
    }
}

