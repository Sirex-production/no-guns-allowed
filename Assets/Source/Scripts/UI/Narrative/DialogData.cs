using System;
using System.Collections.Generic;
using NaughtyAttributes;
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
        [ResizableTextArea]
        [AllowNesting]
        public string phrase;
        public float pauseAfterPhrase;
    }
}