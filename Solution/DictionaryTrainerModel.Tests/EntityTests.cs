using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    
    class EmptyEntity: Entity
    { }
    
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void Equals()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            
            var entity = new EmptyEntity() { ID = id1 };
            var entity2 = new EmptyEntity() { ID = id1 };
            var entity3 = new EmptyEntity() { ID = id2 };

            Assert.IsFalse(entity.Equals(null));
            Assert.IsTrue(entity.Equals(entity));
            Assert.IsTrue(entity.Equals(entity2));
            Assert.IsFalse(entity.Equals(entity3));
            Assert.IsFalse(entity.Equals(new Word()));
        }

        [TestMethod]
        public void EqualsWithObject()
        {
            var id1 = Guid.NewGuid();

            var entity = new EmptyEntity() { ID = id1 };
            var entity2 = new EmptyEntity() { ID = id1 };

            Assert.IsFalse(entity.Equals(null));
            Assert.IsTrue(entity.Equals(entity as Object));
            Assert.IsFalse(entity.Equals(new ScheduleItem()));
        }
    }
}
