using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.UI
{
    [CreateAssetMenu(menuName = "Data/Ingame/Narrative/DialogData", fileName = "NewDialogData")]
    public class DialogData : ScriptableObject
    {
        [SerializeField] private List<DialogPhrasePart> dialog;
        
        public List<DialogPhrasePart> Dialog => new List<DialogPhrasePart>(dialog);
    }

    [Serializable]
    public class DialogPhrasePart
    {
        public string phrase;
        public float pauseAfterPhrase;
    }
}