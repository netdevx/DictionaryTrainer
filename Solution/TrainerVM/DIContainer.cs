using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Ninject;

using AnSoft.DictionaryTrainer.Model;
using AnSoft.DictionaryTrainer.Storage;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class DIContainer: StandardKernel
    {
        private static DIContainer instance;
        public static DIContainer Instance
        {
            get
            {
                if (instance == null)
                    instance = new DIContainer();
                
                return instance;
            }
        }
        
        protected DIContainer(): base()
        {
            this.RegisterDependencies();
        }

        protected void RegisterDependencies()
        {
            this.Bind<Trainer>().To<Trainer>().WithConstructorArgument("language", Language.En);
            this.Bind<IWordStorage>().To<WordStorage>().WithConstructorArgument<string>("c:\\tmp\\dictionary.dat");
            this.Bind<IStorage<WordResult>>().To<WordResultStorage>().WithConstructorArgument<string>("results.dat");

            int sessionWordCount = 10;
            byte customWordPercent = 50;
            Int32.TryParse(ConfigurationManager.AppSettings["sessionWordCount"], out sessionWordCount);
            Byte.TryParse(ConfigurationManager.AppSettings["customWordPercent"], out customWordPercent);

            this.Bind<IWordSessionProvider>().To<WordSessionProvider>()
                .WithConstructorArgument("sessionWordCount", sessionWordCount)
                .WithConstructorArgument("customWordPercent", customWordPercent);
            
            this.Bind<IScheduleBuilder>().To<ScheduleBuilder>();
        }

        public T Get<T>()
        {
            return ResolutionExtensions.Get<T>(this);
        }
    }
}
