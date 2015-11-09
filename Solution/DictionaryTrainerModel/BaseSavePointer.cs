using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public abstract class BaseSavePointer<TEntity> : ISavePointer where TEntity : Entity
    {
        public BaseSavePointer(TEntity entity)
        {
            this.Entity = entity;
        }

        protected TEntity savePoint;

        public TEntity Entity { get; protected set; }

        public abstract void CopyTo(TEntity source, TEntity copy);

        protected TEntity Clone(TEntity source)
        {
            var clone = System.Activator.CreateInstance(typeof(TEntity)) as TEntity;
            this.CopyTo(source, clone);

            return clone;
        }

        protected TEntity Clone()
        {
            return this.Clone(this.Entity);
        }

        public void MakeSavePoint()
        {
            if (savePoint == null)
                this.savePoint = this.Clone();
            else
                throw new ApplicationException("SavePoint already has been made for this object!");
        }

        public void DeleteSavePoint()
        {
            this.savePoint = null;
        }

        public void RollbackToSavePoint()
        {
            if (this.savePoint != null)
            {
                this.CopyTo(this.savePoint, this.Entity);
                this.savePoint = null;
            }
        }

        public bool IsSavePoint
        {
            get { return this.savePoint != null; }
        }
    }
}
