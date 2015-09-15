using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AnSoft.DictionaryTrainer.Model
{
    public abstract class Storage<T>: IStorage<T> where T: class
    {
        public string Source { get; set; }
        
        public Storage(string source)
        {
            this.Source = source;
            this.Reopen();
        }

        protected IList<T> allList = new List<T>();
        public IList<T> AllList
        {
            get { return allList; }
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
                }
                FileInfo f = new FileInfo(this.Source);
                File.Copy(this.Source, String.Format("{0}\\{1}.dat", f.Directory, f.Name.Replace(".dat", "") + "_backup"), true);
            }
        }
    }
}
