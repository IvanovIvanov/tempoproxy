using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TempoProxy
{
    public class FileWorker: IFileWorker
    {
        private String pathToFile;

        /// <summary>
        /// set pathToFile null to load default UserList file
        /// </summary>
        /// <param name="pathToFile"></param>
        public FileWorker(String pathToFile)
        {
            if (String.IsNullOrEmpty(pathToFile))
            {
                this.pathToFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["pathToUserListFile"]);
                return;
            }

            this.pathToFile = pathToFile;
        }

        public String ReadFile()
        {
            String dataFromFile = "";
            dataFromFile = System.IO.File.ReadAllText(pathToFile);
            return dataFromFile;
        }

        public void UpdateFile(String newData)
        {
            try {
                RemoveFile();
                System.IO.File.WriteAllText(pathToFile, newData);
            }
            catch { }
        }
        public void RemoveFile()
        {
            try
            {
                if (System.IO.File.Exists(pathToFile))
                    System.IO.File.Delete(pathToFile);
            }
            catch { }
        }
    }
}