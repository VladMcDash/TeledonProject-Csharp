using System;
namespace ProiectMPP.TeledonProject.Domain;
[Serializable]
public abstract class Entity<TId>
    {
        public TId Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Entity<TId> entity)
            {
                return Equals(Id, entity.Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return $"Entity{{id={Id}}}";
        }
    }
