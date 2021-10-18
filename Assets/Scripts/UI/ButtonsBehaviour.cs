using UnityEngine;

namespace Ingame
{
    public class ButtonsBehaviour : MonoBehaviour
    {
        [Header("OpenUrl parameters")]
        [SerializeField] private string urlToOpen = "https://www.google.com/";
        
        public void OpenUrl()
        {
            Application.OpenURL(urlToOpen);
        }
    }
}