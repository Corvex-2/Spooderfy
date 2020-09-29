using System.IO;
using Newtonsoft.Json;

namespace Spooderfy
{
    internal class SpooderfyCredentials
    {
        private string clientId;
        private string clientSecret;
        private string redirectUrl;

        public string ClientId
        {
            get
            {
                return clientId;
            }
            set
            {
                clientId = value;
                Save(this);
            }
        }

        public string ClientSecret
        {
            get
            {
                return clientSecret;
            }
            set
            {
                clientSecret = value;
                Save(this);
            }
        }
        public string RedirectUrl
        {
            get
            {
                return redirectUrl;
            }
            set
            {
                redirectUrl = value;
                Save(this);
            }
        }

        public static SpooderfyCredentials Load()
        {
            if (File.Exists("DATA\\credentials.sfy"))
            {
                string Text = File.ReadAllText("DATA\\credentials.sfy");
                if (string.IsNullOrEmpty(Text))
                    return new SpooderfyCredentials();
                SpooderfyCredentials Credentials = new SpooderfyCredentials();
                JsonConvert.PopulateObject(Text, Credentials, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                return Credentials;
            }
            else
                return new SpooderfyCredentials();
        }

        public static void Save(SpooderfyCredentials Value)
        {
            if (!Directory.Exists("DATA"))
                Directory.CreateDirectory("DATA");
            if (!File.Exists("DATA\\credentials.sfy"))
                File.Create("DATA\\credentials.sfy").Close();
            string Text = JsonConvert.SerializeObject(Value);
            File.WriteAllText("DATA\\credentials.sfy", Text);
        }
    }
}
