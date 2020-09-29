using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Spooderfy
{
    internal static class SpooderfyContent
    {
        internal static SpooderfyCredentials Credentials = new SpooderfyCredentials();
        internal static Spooder Spooder = new Spooder();

        public static string TrackId
        {
            get
            {
                if (!Spooder.Opened)
                    return "";
                var Track = Spooder.API.GetPlayingTrack();
                if (Track != null && !Track.HasError())
                {
                    var Item = Track.Item;
                    if (Item != null)
                    {
                        return Item.Id;
                    }
                }
                return "";
            }
        }

        public static string Artist
        {
            get
            {
                if (!Spooder.Opened)
                    return "";
                var Track = Spooder.API.GetPlayingTrack();
                if (Track != null && !Track.HasError())
                {
                    var Item = Track.Item;
                    if (Item != null)
                    {
                        return Item.Artists.Select(x => x.Name).Aggregate((current, next) => next != string.Empty ? current + ", " + next : "");
                    }
                }
                return "";
            }
        }

        public static string Title
        {
            get
            {
                if (!Spooder.Opened)
                    return "";
                var Track = Spooder.API.GetPlayingTrack();
                if (Track != null && !Track.HasError())
                {
                    var Item = Track.Item;
                    if (Item != null)
                    {
                        return Item.Name;
                    }
                }
                return "";
            }
        }

        public static SpotifyAPI.Web.Models.PlaybackContext PlayingTrack
        {
            get
            {
                if (!Spooder.Opened)
                    return default;
                Spooder.Refresh();
                return Spooder.API.GetPlayingTrack();
            }
        }

        public static ImageSource Image
        {
            get
            {
                if (!Spooder.Opened)
                    return default(ImageSource);
                Spooder.Refresh();
                var Track = Spooder.API.GetPlayingTrack();
                if (Track != null && !Track.HasError())
                {
                    var Item = Track.Item;
                    if (Item != null)
                    {
                        var Images = Item.Album.Images;
                        if (Images.Count > 0)
                        {
                            return SpooderfyHelper.ImageFromUrl(Images[0].Url);
                        }
                        return default(ImageSource);
                    }
                }
                return default(ImageSource);
            }
        }

        public static string ImageUri
        {
            get
            {
                if (!Spooder.Opened)
                    return "";
                Spooder.Refresh();
                var Track = Spooder.API.GetPlayingTrack();
                if (Track != null && !Track.HasError())
                {
                    var Item = Track.Item;
                    if (Item != null)
                    {
                        var Images = Item.Album.Images;
                        if (Images.Count > 0)
                        {
                            return Images[0].Url;
                        }
                        return "";
                    }
                }
                return "";
            }
        }
    }
}
