using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livethoughts.Toolkit.Powershell.SocialShare.Data.Settings
{
    public class SocialShareSettings
    {
        public string UserId { get; set; }

        public bool IsDebugEnabled { get; set; }

        public static SocialShareSettings LoadFromFile(string settingsPath)
        {
            var result = new SocialShareSettings();

            using (StreamReader file = File.OpenText(settingsPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = (SocialShareSettings)serializer.Deserialize(file, typeof(SocialShareSettings));
            }

            return result;
        }
    }
}
