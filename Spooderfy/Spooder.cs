using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;


namespace Spooderfy
{
    internal class Spooder
    {
        public AuthorizationCodeAuth Authorization = new AuthorizationCodeAuth("xxx", "xxx");
        public Token Token = new Token();
        public SpotifyWebAPI API = new SpotifyWebAPI();
        private bool _opened;
        public bool Opened
        {
            get
            {
                return _opened;
            }
            set
            {
                _opened = value;
                OnOpenStateChanged(value);
            }
        }

        internal async void Open(SpooderfyCredentials Credentials)
        {
            Authorization = new AuthorizationCodeAuth(
                                Credentials.ClientId,
                                Credentials.ClientSecret,
                                Credentials.RedirectUrl,
                                Credentials.RedirectUrl,
                                Scope.Streaming |
                                Scope.AppRemoteControl |
                                Scope.UserReadCurrentlyPlaying |
                                Scope.UserReadPlaybackState |
                                Scope.UserReadPrivate);
            Authorization.AuthReceived += AuthRecieved;
            Authorization.Start();
            Authorization.OpenBrowser();
        }

        private async void AuthRecieved(object sender, AuthorizationCode payload)
        {
            Authorization.Stop();
            Token = await Authorization.ExchangeCode(payload.Code);
            API = new SpotifyWebAPI() { TokenType = Token.TokenType, AccessToken = Token.AccessToken };
            Opened = true;
            Save(this);
        }

        public async void Refresh()
        {
            if(Opened)
            {
                var RefreshedToken = await Authorization.RefreshToken(Token.RefreshToken);
                if(RefreshedToken.HasError())
                {
                    SpooderfyHelper.HandleError(RefreshedToken.Error);
                    return;
                }
                Opened = true;
                API.AccessToken = RefreshedToken.AccessToken;
                API.TokenType = RefreshedToken.TokenType;
                //Token = RefreshedToken;
                Save(this);
            }
        }

        public event OpenedChanged OpenStateChanged;

        public static Spooder Load()
        {
            if (File.Exists("DATA\\spooder.sfy"))
            {
                string Text = File.ReadAllText("DATA\\spooder.sfy");
                Spooder Spooder = new Spooder();
                JsonConvert.PopulateObject(Text, Spooder, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                return Spooder;
            }
            else
                return new Spooder();
        }

        public static Spooder Load(Action<bool> Callback)
        {
            if (File.Exists("DATA\\spooder.sfy"))
            {
                string Text = File.ReadAllText("DATA\\spooder.sfy");
                Spooder Spooder = new Spooder();
                Spooder.OpenStateChanged += new OpenedChanged(Callback);
                JsonConvert.PopulateObject(Text, Spooder, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
                return Spooder;
            }
            else
                return new Spooder();
        }

        public static void Save(Spooder Value)
        {
            if (!Directory.Exists("DATA"))
                Directory.CreateDirectory("DATA");
            if (!File.Exists("DATA\\spooder.sfy"))
                File.Create("DATA\\spooder.sfy").Close();
            string Text = JsonConvert.SerializeObject(Value);
            File.WriteAllText("DATA\\spooder.sfy", Text);
        }

        private void OnOpenStateChanged(bool State)
        {
            OpenStateChanged?.Invoke(State);
        }

        public delegate void OpenedChanged(bool State);
    }
}
