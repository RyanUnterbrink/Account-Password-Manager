using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using IWshRuntimeLibrary;

namespace Account_Manager
{
    public class SaveLoad
    {
        // fields
        string accManPath;
        string managedFile = "/Managed.xd";
        string configFile = "/Configuration.xd";
        string managedDest;
        string configDest;

        public string ManagedDest
        {
            get
            {
                return managedDest;
            }
        }

        // constructor
        public SaveLoad()
        {
            // get account manager folder path
            accManPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Account Manager";

            // load new folder if it doesnt exist
            if (!Directory.Exists(accManPath))
            {
                Directory.CreateDirectory(accManPath);
            }

            SetupDestination(accManPath + managedFile, ref managedDest);
            SetupDestination(accManPath + configFile, ref configDest);
        }

        // methods
        public void SaveManaged(string data)
        {
            SaveFile(data, managedDest);
        }
        public void SaveConfiguration(Configuration config)
        {
            SaveFile(config, configDest);
        }
        public dynamic LoadManaged(string data)
        {
            return LoadFile(data, managedDest);
        }
        public dynamic LoadConfiguration(Configuration config)
        {
            return LoadFile(config, configDest);
        }

        void SaveFile(dynamic data, string destination)
        {
            // write to or create a file to save to
            FileStream file;
            if (System.IO.File.Exists(destination))
            {
                file = System.IO.File.OpenWrite(destination);
            }
            else
            {
                file = System.IO.File.Create(destination);
            }

            // deserialize data into file
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);

            // close file
            file.Close();
        }
        dynamic LoadFile(dynamic data, string destination)
        {
            // try opening a file to load from
            FileStream file;
            if (System.IO.File.Exists(destination))
            {
                file = System.IO.File.OpenRead(destination);
            }
            else
            {
                return data;
            }

            // deseralize data from file
            BinaryFormatter bf = new BinaryFormatter();
            data = (dynamic)bf.Deserialize(file);

            // close file
            file.Close();

            return data;
        }
        public void SetupDestination(string filePath, ref string destination)
        {
            // find file shortcut
            string shortcutFilePath = GetShortcutFile(filePath);
            if (shortcutFilePath != null && System.IO.File.Exists(shortcutFilePath))
            {
                destination = shortcutFilePath;
            }
            else
            {
                destination = filePath;
            }
        }
        string GetShortcutFile(string path)
        {
            string shortcutPath = path + ".lnk";
            if (!System.IO.File.Exists(shortcutPath))
            {
                return null;
            }

            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                if (shortcut.TargetPath != null)
                {
                    FileSecurity fileSecurity = System.IO.File.GetAccessControl(shortcut.TargetPath);
                    return shortcut.TargetPath;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
