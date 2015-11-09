using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid ID { get; set; }

        public bool Equals(Entity other)
        {
            if (other == null)
                return false;
            else if (Object.ReferenceEquals(this, other))
                return true;
            else if (other.GetType() != this.GetType())
                return false;
            else
                return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}
