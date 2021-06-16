using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    public class Modals : MonoBehaviour
    {
        private Modal[] modals;

        private static Modals _instance;

        public static Modals instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Modals>();
                    _instance.SetUp();
                }

                return _instance;
            }
        }

        private Modal currentModal;
        private Modal lastModal;

        public void SetUp()
        {
            modals = GetComponentsInChildren<Modal>();
        }

        public T GetModel<T>() where T : Modal
        {
            return modals.First(x => typeof(T) == x.GetType()) as T;
        }

        public T OpenModal<T>() where T : Modal
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

            lastModal = currentModal;
            currentModal = targetModal;

            return targetModal as T;
        }

        public void CloseModal()
        {
            if (currentModal != null)
                currentModal.Show(false);
        }

        public void CloseAllModal()
        {
            foreach (Modal modal in modals) { modal.Show(false); }
        }

        public void BackToLastModal()
        {
            CloseAllModal();

            lastModal.Show(true);

            var current = currentModal;
            currentModal = lastModal;
            lastModal = current;
        }
    }
}

