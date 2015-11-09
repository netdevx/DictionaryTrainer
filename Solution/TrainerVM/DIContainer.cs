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
        protected DIContainer()
            : base()
        {
            this.RegisterDependencies();
        }

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
        
        protected void RegisterDependencies()
        {
            string pathToStorageFiles = ConfigurationManager.AppSettings["pathToStorageFiles"];
            if (pathToStorageFiles != null && !pathToStorageFiles.EndsWith("\\"))
                pathToStorageFiles = pathToStorageFiles + "\\";
            string wordStorageFileName = String.Format("{0}{1}", pathToStorageFiles, Constants.wordStorageFileName);
            string wordResultStorageFileName = String.Format("{0}{1}", pathToStorageFiles, Constants.wordResultStorageFileName);
            
            this.Bind<IWordStorage>().ToConstant<WordStorage>(new WordStorage(wordStorageFileName));
            this.Bind<IStorage<WordResult>>().ToConstant<WordResultStorage>(new WordResultStorage(wordResultStorageFileName));

            int penaltyRepetitionCount = 2;
            Int32.TryParse(ConfigurationManager.AppSettings["penaltyRepetittionCount"], out penaltyRepetitionCount);
            this.Bind<Trainer>().To<Trainer>().WithConstructorArgument("language", Language.En).WithConstructorArgument("penaltyRepetitionCount", penaltyRepetitionCount);

            int sessionWordCount = 10;
            byte customWordPercent = 50;
            Int32.TryParse(ConfigurationManager.AppSettings["sessionWordCount"], out sessionWordCount);
            Byte.TryParse(ConfigurationManager.AppSettings["customWordPercent"], out customWordPercent);

            this.Bind<IWordSessionProvider>().To<WordSessionProvider>()
                .WithConstructorArgument("sessionWordCount", sessionWordCount)
                .WithConstructorArgument("customWordPercent", customWordPercent);            
            this.Bind<IScheduleBuilder>().To<ScheduleBuilder>();
            
            this.Bind<TrainerVM>().To<TrainerVM>();
            this.Bind<MainVM>().To<MainVM>().InSingletonScope();
        }

        public T Get<T>()
        {
            return ResolutionExtensions.Get<T>(this);
        }
    }
}
