using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.Storage
{
    public abstract class Storage<T>: IStorage<T> where T: class
    {        
        public Storage(string source)
        {
            this.Items = new List<T>();            

            this.Source = source;
            this.Reopen();
        }

        public string Source { get; set; }

        private IList<T> items;
        protected IList<T> Items
        {
            get { return items; }
            set 
            { 
                items = value;
                this.roAllList = new ReadOnlyCollection<T>(Items);
            }
        }

        private IReadOnlyCollection<T> roAllList;
        public IReadOnlyCollection<T> AllList
        {
            get { return roAllList; }
        }

        public virtual T Add(T item)
        {
            this.Items.Add(item);
            return item;
        }

        public virtual T Update(T item)
        {
            return item;
        }

        public virtual bool Delete(T item)
        {
            this.Items.Remove(item);
            return true;
        }

        public virtual void Save()
        {
            var serializer = new BinaryFormatter();
            using (var fileStream = new FileStream(this.Source, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, this.Items);
            }
        }

        public virtual void Reopen()
        {
            if (File.Exists(this.Source))
            {
                var serializer = new BinaryFormatter();
                using (var fileStream = new FileStream(this.Source, FileMode.Open))
                {
                    this.Items = serializer.Deserialize(fileStream) as IList<T>;
                }
                FileInfo f = new FileInfo(this.Source);
                File.Copy(this.Source, String.Format("{0}\\{1}.dat", f.Directory, f.Name.Replace(".dat", "") + "_backup"), true);
            }
        }
    }
}
