using DaeMvvmFramework;
using DogFight;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DogFightTests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void Convert()
        {
            // Arrange: create a world model.
            var world = new World();

            var spitfire = new Fighter { Name = "Spitfire", PositionX = 100, PositionY = 200, Rotation = 45 };
            var stuka = new Fighter { Name = "Stuka", PositionX = 300, PositionY = 400, Rotation = 90, Target = spitfire };

            world.Fighters.Add(spitfire);
            world.Fighters.Add(stuka);

            // Act: convert to view model.
        	var mainContext = new MainContext(null);
			var worldContext = new WorldContext(mainContext, world);

            // Assert: verify view model
            Assert.IsNotNull(worldContext.Fighters);
            Assert.AreEqual(2, worldContext.Fighters.Count);

            FighterContext spitfireContext = worldContext.Fighters.FirstOrDefault(f => f.Name == "Spitfire");
            Assert.IsNotNull(spitfireContext);
            Assert.AreEqual("Spitfire", spitfireContext.Name);
            Assert.AreEqual(100.0, spitfireContext.Position.X);
            Assert.AreEqual(200.0, spitfireContext.Position.Y);
            Assert.AreEqual(45.0, spitfireContext.Rotation);
            Assert.AreEqual(null, spitfireContext.Target);

            FighterContext stukaContext = worldContext.Fighters.FirstOrDefault(f => f.Name == "Stuka");
            Assert.IsNotNull(stukaContext);
            Assert.AreEqual("Stuka", stukaContext.Name);
            Assert.AreEqual(300.0, stukaContext.Position.X);
            Assert.AreEqual(400.0, stukaContext.Position.Y);
            Assert.AreEqual(90.0, stukaContext.Rotation);
            Assert.AreEqual(spitfireContext, stukaContext.Target);

            // Act: convert back to model.
            world = worldContext.CreateModel();

            // Assert: correctness of created model.
            Assert.IsNotNull(world.Fighters);
            Assert.AreEqual(2, world.Fighters.Count);

            spitfire = world.Fighters.FirstOrDefault(f => f.Name == "Spitfire");
            Assert.IsNotNull(spitfire);
            Assert.AreEqual("Spitfire", spitfire.Name);
            Assert.AreEqual(100, spitfire.PositionX);
            Assert.AreEqual(200, spitfire.PositionY);
            Assert.AreEqual(45, spitfire.Rotation);
            Assert.AreEqual(null, spitfire.Target);

            stuka = world.Fighters.FirstOrDefault(f => f.Name == "Stuka");
            Assert.IsNotNull(stuka);
            Assert.AreEqual("Stuka", stuka.Name);
            Assert.AreEqual(300, stuka.PositionX);
            Assert.AreEqual(400, stuka.PositionY);
            Assert.AreEqual(90, stuka.Rotation);
            Assert.AreEqual(spitfire, stuka.Target);
        }

        [TestMethod]
        public void New()
        {
            // Arrange: create a main context.
            var main = new MainContext(null);

            // Assert: verify it has a valid and empty world.
            Assert.IsNotNull(main.World);
            Assert.IsNotNull(main.World.Fighters);
            Assert.AreEqual(main.World.Fighters.Count, 0);

            // Arrange: Add a fighter context.
            main.World.Fighters.Add(new FighterContext(main.World));

            // Assert: we must be able to call the new command.
            Assert.IsTrue(main.NewCommand.CanExecute(null));

            // Act: call New command.
            main.NewCommand.Execute(null);

            // Assert: verify we have an empty world again.
            Assert.IsNotNull(main.World);
            Assert.IsNotNull(main.World.Fighters);
            Assert.AreEqual(main.World.Fighters.Count, 0);
        }

        [TestMethod]
        public void AddFighter()
        {
            // Arrange: create a main context.
            var main = new MainContext(null);

			// Assert: no fighters should be added yet
			Assert.AreEqual(main.World.Fighters.Count, 0);

			// Act: create new fighter context
        	var newFighterContext = new NewFighterContext(main);

			// Assert: context must be invalid (name not yet given)
			Assert.IsFalse(newFighterContext.IsValid);

			// Act: set name
        	newFighterContext.Name = "newFighter";

			// Assert: context must be valid now
			Assert.IsTrue(newFighterContext.IsValid);

			// Assert: we must be able to add the fighter now.
            Assert.IsTrue(newFighterContext.AddFighterCommand.CanExecute(null));

            // Act: Add a new fighter 
            newFighterContext.AddFighterCommand.Execute(null);

            // Assert: the new fighter must have been added and be the selected one.
            Assert.AreEqual(main.World.Fighters.Count, 1);
            Assert.AreEqual(main.SelectedFighter, main.World.Fighters[0]);

			// Assert: the fighter should have the correct name
			Assert.AreEqual(main.SelectedFighter.Name, newFighterContext.Name);
        }

        [TestMethod]
        public void RemoveFighter()
        {
            // Arrange: create a main context.
            var main = new MainContext(null);

            // Assert: we should not be able to remove a fighter.
            Assert.IsFalse(main.RemoveFighterCommand.CanExecute(null));

            // Arrange: add a 3 of fighters, select the second
            var fighter1 = new FighterContext(main.World);
            var fighter2 = new FighterContext(main.World);
            var fighter3 = new FighterContext(main.World);
            main.World.Fighters.Add(fighter1);
            main.World.Fighters.Add(fighter2);
            main.World.Fighters.Add(fighter3);

            // Assert: we should not yet be able to remove a fighter.
            Assert.IsFalse(main.RemoveFighterCommand.CanExecute(null));

            // Select the second fighter.
            main.SelectedFighter = fighter2;

            // Assert: we should now be able to remove the fighter.
            Assert.IsTrue(main.RemoveFighterCommand.CanExecute(null));

            // Act: remove fighter2
            main.RemoveFighterCommand.Execute(null);

            // Assert: fighter2 should be removed.
            Assert.IsTrue(main.World.Fighters.SequenceEqual(new[] {fighter1, fighter3}));

            // Assert: fighter3 should now be selected.
            Assert.AreEqual(main.SelectedFighter, fighter3);

            // Assert: we should now be able to remove fighter3.
            Assert.IsTrue(main.RemoveFighterCommand.CanExecute(null));

            // Act: remove fighter3
            main.RemoveFighterCommand.Execute(null);

            // Assert: fighter3 should be removed.
            Assert.IsTrue(main.World.Fighters.SequenceEqual(new[] { fighter1 }));

            // Assert: fighter1 should now be selected.
            Assert.AreEqual(main.SelectedFighter, fighter1);

            // Assert: we should now be able to remove fighter1.
            Assert.IsTrue(main.RemoveFighterCommand.CanExecute(null));

            // Act: remove fighter1
            main.RemoveFighterCommand.Execute(null);

            // Assert: nothing should be selected.
            Assert.IsNull(main.SelectedFighter);

            // Assert: world should be empty now.
            Assert.AreEqual(main.World.Fighters.Count, 0);
        }
    }
}