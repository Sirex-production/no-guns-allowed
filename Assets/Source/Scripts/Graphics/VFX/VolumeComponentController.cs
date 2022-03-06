using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ingame.Graphics.VFX
{
    public abstract class VolumeComponentController<T> : MonoBehaviour where T : VolumeComponent
    {
        [SerializeField] protected Volume volumeReference;
        protected T effectToChange;

        protected void Awake()
        {
            volumeReference.profile.TryGet(out effectToChange);
        }

        protected abstract IEnumerator OnModificationRoutine();

        public abstract void Reset();

        public void Modify()
        {
            StartCoroutine(OnModificationRoutine());
        }
    } 
}
