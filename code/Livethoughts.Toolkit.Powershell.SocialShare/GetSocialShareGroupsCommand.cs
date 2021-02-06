using Livethoughts.Toolkit.Powershell.SocialShare.Data.Facebook;
using Livethoughts.Toolkit.Powershell.SocialShare.Data.Settings;
using Livethoughts.Toolkit.Powershell.SocialShare.Data.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Net;

namespace Livethoughts.Toolkit.Powershell.SocialShare
{
    [Cmdlet(VerbsCommon.Get, "SocialShareGroups")]
    public class GetSocialShareGroupsCommand : Cmdlet
    {
        [Parameter(ParameterSetName = "ByParameter")]
        public string UserId { get; set; }

        [Parameter(ParameterSetName = "ByFile")]
        public string SettingsPath { get; set; }

        [Parameter]
        public bool IsDebug { get; set; }

        public SocialShareSettings Settings { get; set; }

        public List<SocialShareGroup> SocialShareGroups { get; set; }

        protected override void ProcessRecord()
        {

            if(!string.IsNullOrEmpty(SettingsPath))
            {
                Settings = SocialShareSettings.LoadFromFile(SettingsPath);

                UserId = Settings.UserId;
                IsDebug = Settings.IsDebugEnabled;
            }

            SocialShareGroups = new List<SocialShareGroup>();

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");


                ListGroupsFacebookResponse jsonresponse;
                using (Stream data = client.OpenRead($"https://graph.facebook.com/{UserId}/groups"))
                {

                    using (StreamReader reader = new StreamReader(data))
                    {
                        string response = reader.ReadToEnd();

                        if(IsDebug)
                            Console.WriteLine(response);

                        jsonresponse = JsonConvert.DeserializeObject<ListGroupsFacebookResponse>(response);

                        data.Close();
                        reader.Close();
                    }
                }

                ParseResponse(jsonresponse);

                if (jsonresponse.paging != null && !string.IsNullOrEmpty(jsonresponse.paging.next))
                {
                    ProcessNextPage(jsonresponse);
                }
            }

            WriteObject(SocialShareGroups, true);
        }

        private void ProcessNextPage(ListGroupsFacebookResponse jsonresponse)
        {
            if (jsonresponse == null || jsonresponse.paging == null || string.IsNullOrEmpty(jsonresponse.paging.next))
                return;

            ListGroupsFacebookResponse jsonPageResponse;

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                using (Stream data = client.OpenRead(jsonresponse.paging.next))
                {

                    using (StreamReader reader = new StreamReader(data))
                    {
                        string response = reader.ReadToEnd();

                        if (IsDebug)
                            Console.WriteLine(response);

                        jsonPageResponse = JsonConvert.DeserializeObject<ListGroupsFacebookResponse>(response);

                        data.Close();
                        reader.Close();
                    }
                }
            }
            ParseResponse(jsonPageResponse);

            if (jsonPageResponse.paging != null && !string.IsNullOrEmpty(jsonPageResponse.paging.next))
            {
                ProcessNextPage(jsonPageResponse);
            }
        }

        private void ParseResponse(ListGroupsFacebookResponse jsonresponse)
        {
            foreach (var group in jsonresponse.data)
            {
                SocialShareGroups.Add(ParseGroupData(group));
            }
        }

        private SocialShareGroup ParseGroupData(ListGroupsFacebookResponseGroup group)
        {
            SocialShareGroup result = new SocialShareGroup();

            result.Id = group.id;
            result.Name = group.name;

            return result;
        }
    }
}
