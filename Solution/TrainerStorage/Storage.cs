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
            this.allList = new List<T>();
            this.roAllList = new ReadOnlyCollection<T>(allList);

            this.Source = source;
            this.Reopen();
        }

        public string Source { get; set; }

        protected IList<T> allList;
        public IReadOnlyCollection<T> roAllList;
        public IReadOnlyCollection<T> AllList
        {
            get { return roAllList; }
        }

        public virtual T Add(T item)
        {
            this.allList.Add(item);
            return item;
        }

        public virtual T Update(T item)
        {
            return item;
        }

        public virtual bool Delete(T item)
        {
            this.allList.Remove(item);
            return true;
        }

        public virtual void Save()
        {
            var serializer = new BinaryFormatter();
            using (var fileStream = new FileStream(this.Source, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, this.allList);
            }
        }

        public virtual void Reopen()
        {
            if (File.Exists(this.Source))
            {
                var serializer = new BinaryFormatter();
                using (var fileStream = new FileStream(this.Source, FileMode.Open))
                {
                    this.allList = serializer.Deserialize(fileStream) as IList<T>;
                    this.roAllList = new ReadOnlyCollection<T>(this.allList);
                }
                FileInfo f = new FileInfo(this.Source);
                File.Copy(this.Source, String.Format("{0}\\{1}.dat", f.Directory, f.Name.Replace(".dat", "") + "_backup"), true);
            }
        }
    }
}
