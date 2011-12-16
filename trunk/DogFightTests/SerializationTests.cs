using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DogFight;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogFightTests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestSerialization()
        {
            var world = new World();

            var spitfire = new Fighter {Name = "Spitfire", PositionX = 100, PositionY = 200, Rotation = 45};
            var stuka = new Fighter{Name = "Stuka", PositionX = 300, PositionY = 400, Rotation = 90, Target = spitfire};

            world.Fighters.Add(spitfire);
            world.Fighters.Add(stuka);

            var stream = new MemoryStream();
            world.SaveTo(stream);
            Assert.IsTrue(stream.Length > 0 );

            // Deserialize and check result.
            stream.Position = 0;

            world = World.LoadFrom(stream);

            Assert.IsNotNull(world.Fighters);
            Assert.AreEqual(world.Fighters.Count, 2);

            spitfire = world.Fighters.FirstOrDefault(f => f.Name == "Spitfire");
            Assert.IsNotNull(spitfire);
            Assert.AreEqual(spitfire.Name, "Spitfire");
            Assert.AreEqual(spitfire.PositionX, 100);
            Assert.AreEqual(spitfire.PositionY, 200);
            Assert.AreEqual(spitfire.Rotation, 45);
            
            stuka = world.Fighters.FirstOrDefault(f => f.Name == "Stuka");
            Assert.IsNotNull(stuka);
            Assert.AreEqual(stuka.Name, "Stuka");
            Assert.AreEqual(stuka.PositionX, 300);
            Assert.AreEqual(stuka.PositionY, 400);
            Assert.AreEqual(stuka.Rotation, 90);
            Assert.AreEqual(stuka.Target, spitfire);
        }
    }
}
