using UnityEngine;

namespace Client
{
    public abstract class UIPopup : MonoBehaviour
    {

    }

    public abstract class UIPopup<T> : UIPopup where T : PopupContext
    {
        public void Display(T context)
        {
            gameObject.SetActive(true);
            OnDisplay(context);
        }

        public void Close()
        {
            OnClose();
            gameObject.SetActive(false);
        }

        protected abstract void OnDisplay(T context);
        protected abstract void OnClose();
    }

    public abstract class PopupContext
    {

    }
}