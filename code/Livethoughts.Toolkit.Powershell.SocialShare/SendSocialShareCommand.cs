using System.Management.Automation;

namespace Livethoughts.Toolkit.Powershell.SocialShare
{
    [Cmdlet(VerbsData.Publish, "SocialShare")]
    public class SendSocialShareCommand : Cmdlet
    {
        public string Name { get; set; }


        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }
    }
}
