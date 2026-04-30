using System.ComponentModel;
using System.Windows;
using NationalInstruments.SystemConfiguration;

namespace NationalInstruments.Examples.GetAndSetImage
{
    class GetAndSetImageWorker : INotifyPropertyChanged
    {
        private bool canBeginGetSetImage;
        private SoftwareComponentCollection installedSoftware;

        public GetAndSetImageWorker()
        {
            ImageMetadata = new ImageMetadata();
            CanBeginGetSetImage = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanBeginGetSetImage
        {
            get
            {
                return canBeginGetSetImage;
            }
            private set
            {
                if (canBeginGetSetImage != value)
                {
                    canBeginGetSetImage = value;
                    NotifyPropertyChanged("CanBeginGetSetImage");
                }
            }
        }

        public string Target
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Directory
        {
            get;
            set;
        }

        public ImageMetadata ImageMetadata
        {
            get;
            set;
        }

        public NetworkInterfaceSettings NetworkInterfaceSettings
        {
            get;
            set;
        }

        public SoftwareComponentCollection InstalledSoftware
        {
            get { return installedSoftware; }
            private set
            {
                if (installedSoftware != value)
                {
                    installedSoftware = value;
                    NotifyPropertyChanged("InstalledSoftware");
                }
            }
        }

        public void StartGetImage(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginGetSetImage = false;
                    try
                    {
                        InstalledSoftware = null;
                        var session = new SystemConfiguration.SystemConfiguration(Target, Username, password);
                        session.CreateSystemImageAsFolder(Directory, true, ImageMetadata);
                        InstalledSoftware = session.GetInstalledSoftwareComponents();
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Create System Image threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        CanBeginGetSetImage = true;
                    }
                }
            );
            worker.RunWorkerAsync();
        }

        public void StartSetImage(string password)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args)
                {
                    CanBeginGetSetImage = false;
                    try
                    {
                        InstalledSoftware = null;
                        var session = new SystemConfiguration.SystemConfiguration(Target, Username, password);
                        session.SetSystemImageFromFolder(Directory, NetworkInterfaceSettings);
                        InstalledSoftware = session.GetInstalledSoftwareComponents();
                    }
                    catch (SystemConfigurationException ex)
                    {
                        string errorMessage = string.Format("Set System Image threw a System Configuration Exception.\n\nError Code: {0:X}\n{1}", ex.ErrorCode, ex.Message);
                        MessageBox.Show(errorMessage, "System Configuration Exception");
                    }
                    finally
                    {
                        CanBeginGetSetImage = true;
                    }
                }
            );
            worker.RunWorkerAsync();
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
