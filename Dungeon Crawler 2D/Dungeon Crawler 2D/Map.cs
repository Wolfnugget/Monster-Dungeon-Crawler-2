using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    class Map
    {
        List<Room> rooms;
        int currentRoom;

        public Map(Room room, int maxRooms, Texture2D exTex, Texture2D playerTex, PlayerCharacter player)
        {
            rooms = new List<Room>(maxRooms);

            rooms.Add(new Room(exTex, playerTex, player, "Maps/" + "StartRoom/" + "E1" + ".txt"));

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = 0; j < rooms[i].tileList.Count; j++)
                {
                    if (rooms[i].tileList[j][0] == 'N')
                    {
                        rooms.Add(new Room(exTex, playerTex, player, "Maps/" + "South/" + "something" + ".txt"));
                    }
                    if (rooms[i].tileList[j][rooms[i].tileList[0].Length] == 'N')
                    {
                        rooms.Add(new Room(exTex, playerTex, player, "Maps/" + "North/" + "something" + ".txt"));
                    }
                }
                for (int j = 0; j < rooms[i].tileList[0].Length; j++)
                {
                    if (rooms[i].tileList[0][j] == 'N')
                    {
                        rooms.Add(new Room(exTex, playerTex, player, "Maps/" + "East/" + "something" + ".txt"));
                    }
                    if (rooms[i].tileList[rooms[i].tileList.Count][j] == 'N')
                    {
                        rooms.Add(new Room(exTex, playerTex, player, "Maps/" + "West/" + "something" + ".txt"));
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            rooms[currentRoom].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rooms[currentRoom].Draw(spriteBatch);
        }
    }
}
