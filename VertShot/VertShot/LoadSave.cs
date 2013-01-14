using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

namespace VertShot
{
    static public class LoadSave
    {
        static public Files.Config LoadConfig()
        {
            if (!File.Exists("config.cfg"))
                SaveConfig(new Files.Config());
            Files.Config config;
            BinaryFormatter bf = new BinaryFormatter();
            GZipStream gzs = new GZipStream(File.OpenRead("config.cfg"), CompressionMode.Decompress);
            config = (Files.Config)bf.Deserialize(gzs);
            gzs.Close();
            gzs.Dispose();
            if (config.fileVersion != Files.Config.FileVersion)
            {
                if(!config.Upgrade())
                    config = new Files.Config();
                SaveConfig(config);
            }

            return config;
        }

        static public bool SaveConfig(Files.Config config)
        {
            BinaryFormatter bf = new BinaryFormatter();
            GZipStream gzs = new GZipStream(File.Create("config.cfg"), CompressionMode.Compress);
            if (gzs == null)
                return false;
            bf.Serialize(gzs, config);
            gzs.Close();
            gzs.Dispose();


            return true;
        }
    }
}
