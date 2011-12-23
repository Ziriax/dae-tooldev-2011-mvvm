using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DaeMvvmFramework;

namespace DogFight
{
    public class WorldContext : PropertyChangeSource
    {
        public MainContext Main { get; private set; }

        public ObservableCollection<FighterContext> Fighters { get; private set; }

        public WorldContext(MainContext parent)
        {
            Main = parent;
            Fighters = new ObservableCollection<FighterContext>();
        }

        public WorldContext(MainContext parent, World worldModel)
        {
            Main = parent;

            // Create fighter contexts from fighter models.
            // Use an anonymous class to associate 
            // the original model with the new context.
            var newFighterPairs =
                from model in worldModel.Fighters
                select new
                {
                    model,
                    context = new FighterContext(this)
                    {
                        Name = model.Name,
                        Position = new Point(model.PositionX, model.PositionY),
                        Rotation = model.Rotation
                    }
                };

            // For fast lookup, convert the list of model <-> context pairs to a dictionary.
            Dictionary<Fighter, FighterContext> newFighterMap =
                newFighterPairs.ToDictionary(pair => pair.model, pair => pair.context);

            // Set the Target properties (these are possibly cyclically connected)
            foreach (var pair in newFighterMap)
            {
                Fighter targetFighter = pair.Key.Target;
                if (targetFighter != null)
                {
                    FighterContext fighterContext = pair.Value;
                    fighterContext.Target = newFighterMap[targetFighter];
                }
            }

            Fighters = new ObservableCollection<FighterContext>(newFighterMap.Values);
        }

        public World CreateModel()
        {
            // Create fighter models from fighter contexts.

            // Use an anonymous class to associate 
            // the original context with the new model.
            var newFighterPairs =
                from context in Fighters
                select new
                {
                    context,
                    model = new Fighter
                    {
                        Name = context.Name,
                        PositionX = context.Position.X, 
                        PositionY = context.Position.Y,
                        Rotation = context.Rotation
                    }
                };

            // For fast lookup, convert the list of context <-> model pairs to a dictionary.
            Dictionary<FighterContext, Fighter> newFighterMap =
                newFighterPairs.ToDictionary(pair => pair.context, pair => pair.model);

            // Set the Target properties (these are possibly cyclically connected)
            foreach (var pair in newFighterMap)
            {
                FighterContext targetFighter = pair.Key.Target;
                if (targetFighter != null)
                {
                    Fighter fighterModel = pair.Value;
                    fighterModel.Target = newFighterMap[targetFighter];
                }
            }

            var world = new World();
            world.Fighters.AddRange(newFighterMap.Values);

            return world;
        }
    }
}