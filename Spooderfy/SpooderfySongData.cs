using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Spooderfy
{
    class SpooderfySongData
    {
        public string Artists;
        public string Title;
        public string Id;
        public string Image;

        public static SpooderfySongData GetData()
        {
            var Song = SpooderfyContent.PlayingTrack;
            if (!Song.HasError())
            {
                var Item = Song.Item;
                if (Item != null && !Item.HasError())
                {
                    var Images = Item.Album.Images;
                    return new SpooderfySongData()
                    {
                        Artists = Item.Artists.Select(x => x.Name).Aggregate((current, next) => next != string.Empty ? current + ", " + next : ""),
                        Title = Item.Name,
                        Id = Item.Id,
                        Image = (Images.Count > 0 ? Images[0].Url : default)
                    };
                }
                else
                {
                    SpooderfyHelper.HandleError($"[STAUTS{Item.Error.Status}] " + Item.Error.Message);
                }
            }
            else
            {
                SpooderfyHelper.HandleError($"[STAUTS{Song.Error.Status}] " + Song.Error.Message);
            }
            return new SpooderfySongData() { Artists = "null", Id = "null", Image = "", Title = "null" };
        }
    }
}
