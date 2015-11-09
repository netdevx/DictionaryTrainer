using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public abstract class SavePointEntity<TEntity> : Entity, ISavePointer, ICloneable where TEntity : SavePointEntity<TEntity>
    {
        [NonSerialized]
        private TEntity savePoint;

        public abstract void CopyTo(TEntity copy);

        public TEntity Clone()
        {
            var clone = System.Activator.CreateInstance(typeof(TEntity)) as TEntity;
            this.CopyTo(clone);

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public void MakeSavePoint()
        {
            if (savePoint == null)
                this.savePoint = this.Clone();
            else
                throw new DictionaryTrainerException("SavePoint already has been made for this object!");
        }

        public void DeleteSavePoint()
        {
            this.savePoint = null;
        }

        public void RollbackToSavePoint()
        {
            if (this.savePoint != null)
            {
                this.savePoint.CopyTo(this as TEntity);
                this.savePoint = null;
            }
        }

        public bool IsSavePoint
        {
            get { return this.savePoint != null; }
        }
    }
}
