using lotte_Client.Models.Data;
using lotte_Client.Utils;
using lotte_Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.Databases
{
    public class DatabaseController
    {
        #region DatabaseController Instance
        private static DatabaseController _instance;

        public static DatabaseController Instance
        {
            get
            {
                return (_instance) ?? (_instance = new DatabaseController());
            }
        }
        #endregion

        public delegate void DbCompletedDelegate();
        public event DbCompletedDelegate DbCompleted;

        public List<Adver> AdverList;

        private DatabaseController()
        {
            AdverList = new List<Adver>();
        }

        public void SetData(bool useLocal)
        {
            if (useLocal)
            {
                SetDataFromLocal();
            }
            else
            {
                SetDataFromWebService();
            }
        }

        private void SetDataFromLocal()
        {
            if (DbCompleted != null) DbCompleted();
        }

        private void SetDataFromWebService()
        {
            App.MainViewModel.DownloadCompleted += MainViewModel_DownloadCompleted;
            App.MainViewModel.StartDownload();
        }

        void MainViewModel_DownloadCompleted()
        {
            AdverList = Converter.ToAdverList(App.MainViewModel.ContentViewModel.AdInfoList);
            RemoveNotUsedFiles();

            if (DbCompleted != null) DbCompleted();
        }

        private void RemoveNotUsedFiles()
        {
            try
            {
           var  localDirectory = new DirectoryInfo(MainViewModel.LOCAL_KO_AD_PATH);
                if (localDirectory.Exists)
                {
                    foreach (var directory in localDirectory.GetDirectories())
                    {
                        var exist = AdverList.Exists(o => o.ID == directory.Name);
                        if (exist == false)
                        {
                            directory.Delete(true);
                            Debug.WriteLine("Deleted directory - " + directory.Name);
                        }
                    }
                }
                                      }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Log.WriteLine(ex.Message);
            }
        }
    }
}
