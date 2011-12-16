using System.Diagnostics;
using System.IO;
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

            // Serialize to memory stream. 
            // Tests should not change the environment,
            // so we prefer not to use the file system.
            var stream = new MemoryStream();
            world.SaveTo(stream);
            Assert.IsTrue(stream.Length > 0 );

            // Deserialize and check result.
            stream.Position = 0;

            world = World.LoadFrom(stream);

            Assert.IsNotNull(world.Fighters);
            Assert.AreEqual(2, world.Fighters.Count);

            spitfire = world.Fighters.FirstOrDefault(f => f.Name == "Spitfire");
            Assert.IsNotNull(spitfire);
            Assert.AreEqual("Spitfire", spitfire.Name);
            Assert.AreEqual(100, spitfire.PositionX);
            Assert.AreEqual(200, spitfire.PositionY);
            Assert.AreEqual(45, spitfire.Rotation);
            
            stuka = world.Fighters.FirstOrDefault(f => f.Name == "Stuka");
            Assert.IsNotNull(stuka);
            Assert.AreEqual("Stuka", stuka.Name);
            Assert.AreEqual(300, stuka.PositionX);
            Assert.AreEqual(400, stuka.PositionY);
            Assert.AreEqual(90, stuka.Rotation);
            Assert.AreEqual(spitfire, stuka.Target);

            // Dump result to debug window for demo purposes
            // WARNING: The StreamReader will close the stream when it is closed!
            // And the using statement will call Dispose on the StreamReader,
            // which will close the stream. 
            // So we have to do this debug dump at the end of our test.
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                Debug.WriteLine(reader.ReadToEnd());
            }
        }
    }
}
