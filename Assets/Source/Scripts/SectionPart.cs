namespace Ingame
{
    public interface ISectionPart
    {
        public void OnSectionAdded();
        public void OnPlayerSectionEnter();
        public void OnPlayerSectionExit();
        public void OnLevelOverviewManaged(bool isEntered);
    }
}