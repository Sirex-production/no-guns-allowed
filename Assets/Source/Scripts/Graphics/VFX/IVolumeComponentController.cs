using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ingame.Graphics.VFX
{
    public abstract class IVolumeComponentController<T> : MonoBehaviour where T : VolumeComponent
    {
        [SerializeField] protected Volume volumeReference;
        protected T effectToChange;

        protected void Awake()
        {
            volumeReference.profile.TryGet(out effectToChange);
        }

        protected abstract IEnumerator OnModificationRoutine();

        public abstract void DoReset();

        public void Modify()
        {
            StartCoroutine(OnModificationRoutine());
        }
    } 
}
