//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System.IO;

//namespace Dungeon_Crawler_2D.MapSystem
//{
//    class Map
//    {
//        public List<Room> rooms;
//        public int roomNr;
//        public int currentRoom;
//        protected int fileCountNorth;
//        protected int fileCountSouth;
//        protected int fileCountEast; 
//        protected int fileCountWest;
//        Random rand = new Random();

//        public Map(int maxRooms, Texture2D exTex, Texture2D playerTex)
//        {
//            rooms = new List<Room>(maxRooms);
//            roomNr = 0;
//            currentRoom = 0;
//            rooms.Add(new Room(exTex, playerTex, "Maps/" + "StartRoom/" + "1" + ".txt", roomNr));

//            fileCountEast = Directory.GetFiles("Maps/East").Length;
//            fileCountNorth = Directory.GetFiles("Maps/North").Length;
//            fileCountSouth = Directory.GetFiles("Maps/South").Length;
//            fileCountWest = Directory.GetFiles("Maps/West").Length;

//            for (int i = 0; i < rooms.Count; i++)
//            {
//                for (int j = 0; j < rooms[i].tileList.Count; j++)
//                {
//                    if (rooms[i].tileList[j][0] == 'N')
//                    {
//                        roomNr++;
//                        int roomFile = rand.Next(1, fileCountSouth + 1);
//                        rooms.Add(new Room(exTex, playerTex, "Maps/" + "South/" + roomFile + ".txt", roomNr));
//                    }
//                    if (rooms[i].tileList[j][rooms[i].tileList[0].Length] == 'N')
//                    {
//                        roomNr++;
//                        int roomFile = rand.Next(1, fileCountNorth + 1);
//                        rooms.Add(new Room(exTex, playerTex, "Maps/" + "North/" + roomFile + ".txt", roomNr));
//                    }
//                }
//                for (int j = 0; j < rooms[i].tileList[0].Length; j++)
//                {
//                    if (rooms[i].tileList[0][j] == 'N')
//                    {
//                        roomNr++;
//                        int roomFile = rand.Next(1, fileCountEast + 1);
//                        rooms.Add(new Room(exTex, playerTex, "Maps/" + "East/" + roomFile + ".txt", roomNr));
//                    }
//                    if (rooms[i].tileList[rooms[i].tileList.Count][j] == 'N')
//                    {
//                        roomNr++;
//                        int roomFile = rand.Next(1, fileCountWest + 1);
//                        rooms.Add(new Room(exTex, playerTex, "Maps/" + "West/" + roomFile + ".txt", roomNr));
//                    }
//                }
//            }
//        }

//        public void Update(GameTime gameTime)
//        {
//            rooms[currentRoom].Update(gameTime);
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            rooms[currentRoom].Draw(spriteBatch);
//        }

//        public void EnterRoom(int nextRoom)
//        {
//            this.currentRoom = nextRoom;
//            rooms[currentRoom].LoadRoom();
//        }
//    }
//}
