using Support.Extensions;

namespace Ingame
{
    public class EnemySectionPart : SectionPart
    {
        // private bool _isWorking = true;
        
        private void TurnOffChildren()
        {
            // for(var i = 0; i < transform.childCount; i++)
            //     transform.GetChild(i).SetGameObjectInactive();
            //
            // _isWorking = false;
        }

        private void TurnOnChildren()
        {
            // for(var i = 0; i < transform.childCount; i++)
            //     transform.GetChild(i).SetGameObjectActive();
            //
            // _isWorking = true;
        }  

        protected override void OnSectionEnter(int sectionId)
        {
            // if(boundedSectionId != sectionId)
            //     TurnOffChildren();
            // else
            //     TurnOnChildren();
        }

        protected override void OnLevelOverviewEnter()
        {
            // if(_isWorking)
            //     return;
            //
            // TurnOnChildren();
        }

        protected override void OnLevelOverviewExit(int currentSectionId)
        {
            // if(boundedSectionId != currentSectionId && _isWorking)
            //     TurnOffChildren();
        }
    }
}