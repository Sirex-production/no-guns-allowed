using System.Collections.Generic;
using System.Linq;
using Support;

namespace Ingame.AI
{
    public class ActorManager : MonoSingleton<ActorManager>
    {
        private Dictionary<ActorSide, List<ActorStats>> _actors = new Dictionary<ActorSide, List<ActorStats>>();

        public void AddActor(ActorStats actorStats)
        {
            if(actorStats == null)
                return;

            var actorSide = actorStats.ActorSide;
            if (!_actors.ContainsKey(actorSide) || _actors[actorSide] == null)
            {
                _actors.Add(actorSide, new List<ActorStats>(new[] {actorStats}));
                return;
            }
            
            if(!_actors[actorSide].Contains(actorStats))
                _actors[actorSide].Add(actorStats);
        }

        public ActorStats[] GetActorsOfTypes(params ActorSide[] actorSides)
        {
            if (actorSides == null)
                return null;

            var selectedActors = new List<ActorStats>();

            foreach (var actorSide in actorSides)
            {
                if(!_actors.ContainsKey(actorSide))
                    continue;
                
                selectedActors.AddRange(_actors[actorSide]);
            }

            return selectedActors.Where(actor => actor != null).ToArray();
        }

        public ActorStats[] GetOppositeActors(params ActorSide[] actorSides)
        {
            var selectedActors = new List<ActorStats>();
            var selectedLists = _actors
                .Where(pair => !actorSides.Contains(pair.Key))
                .Select(pair => pair.Value).ToArray();

            foreach (var actorList in selectedLists) 
                selectedActors.AddRange(actorList.Where(actor => actor != null));

            return selectedActors.ToArray();
        }
    }
}