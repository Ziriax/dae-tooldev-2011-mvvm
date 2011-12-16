using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DogFight
{
    [DataContract]
    public class World
    {
        [DataMember]
        public readonly List<Fighter> Fighters = new List<Fighter>();
    }
}
