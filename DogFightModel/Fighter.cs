using System.Runtime.Serialization;

namespace DogFight
{
    [DataContract]
    public class Fighter
    {
        [DataMember]
        public string Name;

        [DataMember]
        public double PositionX;

        [DataMember]
        public double PositionY;

        [DataMember]
        public double Rotation;

        [DataMember]
        public Fighter Target;
    }
}
