namespace Client
{
    using System.Collections.Generic;
    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<UIScreen> screens;
        [SerializeField] private List<UIPopup> popups;

        private UIScreen currentScreen;

        public void DisplayScreen<T>(params object[] parameters) where T : UIScreen
        {
            currentScreen?.Close();
            currentScreen = GetScreen<T>();
            currentScreen?.Display(parameters);
        }

        private void OnEnable()
        {
            foreach (UIScreen screen in screens)
            {
                screen.gameObject.SetActive(false);
            }
        }

        private UIScreen GetScreen<T>() where T : UIScreen
        {
            foreach (UIScreen screen in screens)
            {
                if (screen is T s)
                    return s;
            }

            Debug.LogError($"Couldn't found UI Screen of type {typeof(T)}");
            return null;
        }

        public T GetPopup<T>() where T : UIPopup
        {
            foreach (UIPopup popup in popups)
            {
                if (popup is T s)
                    return s;
            }

            Debug.LogError($"Couldn't found UI Screen of type {typeof(T)}");
            return null;
        }
    }
}