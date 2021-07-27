using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    public class Modals : MonoBehaviour
    {
        public MainBarController mainBarController;

        private Modal[] modals;
        private List<Modal> lastModals = new List<Modal>();
        private List<Modal> rootModal = new List<Modal>();

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

        private void Start()
        {
            lastModals.Add(GetModel<MainModal>());

            rootModal.Add(GetModel<MainModal>());
            rootModal.Add(GetModel<FindBookModal>());
            rootModal.Add(GetModel<MineModal>());
        }

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

            if (FindSameModal(rootModal, targetModal))
            {
                lastModals.Clear();
                lastModals.Add(targetModal);
            }
            else if (currentModal != null)
            {
                if (!FindSameModal(lastModals, currentModal) && currentModal != GetModel<BookClassifyModal>())
                    lastModals.Add(currentModal);
            }

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

        public void LastModal()
        {
            if (lastModals.Count == 0) return;

            CloseAllModal();
            
            lastModals[lastModals.Count - 1].Show(true);
            currentModal = lastModals[lastModals.Count - 1];
            lastModals.RemoveAt(lastModals.Count - 1);
        }

        private bool FindSameModal(List<Modal> listModal, Modal _modal)
        {
            return listModal.Exists(m => m == _modal);
        }

        public void CloseBar(bool islose)
        {
            mainBarController.CloseBar(islose);
        }

        public void ChangePet(AllItemObj data, int currentInt)
        {
            var mine = GetModel<MineModal>();
            var main = GetModel<MainModal>();
            var petsItems = data.petsItems;
            var level = PlayerPrefs.GetInt("level");
            var mood = 1; //TODO:set mood

            data.currentPet = petsItems[currentInt];
            data.currentPet.image = data.petsLevelItem[currentInt].level[level].mood[mood];
            PlayerPrefs.SetInt("currentPet", currentInt);

            mine.LoadPet(data.currentPet);
            main.ChangePet(data.currentPet);
        }
    }
}

