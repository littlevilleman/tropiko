namespace Client
{
    using UnityEngine;

    public abstract class UIScreen : MonoBehaviour
    {
        public void Display(params object[] parameters)
        {
            gameObject.SetActive(true);
            OnDisplay(parameters);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose();
        }

        protected abstract void OnDisplay(params object[] parameters);
        protected abstract void OnClose();
    }
}