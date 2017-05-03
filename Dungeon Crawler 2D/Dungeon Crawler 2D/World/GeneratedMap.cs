﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Dungeon_Crawler_2D.World
{
    public class GeneratedMap: Map
    {
        Random rand;

        public GeneratedMap(TextureManager textures, ContentManager content, int minNumberOfRooms, int maxNumberOfRoomsOffset = 0)
            : base(textures, content)
        {
            rand = new Random();
            GenerateMap(rand.Next(minNumberOfRooms, minNumberOfRooms + maxNumberOfRoomsOffset), textures, content);
        }

        /// <summary>
        /// Genererar rum.
        /// </summary>
        /// <param name="numberOfRooms"></param>
        /// <param name="textures"></param>
        void GenerateMap(int numberOfRooms, TextureManager textures, ContentManager content)
        {
            rooms.Add(new PreMadeArea(GetRandomRoomPath("Maps/StartRoom"), new Point(0, 0), textures, content));

            HashSet<int> excludeRoom = new HashSet<int>(); //Rum vars dörrar har genererats.
            excludeRoom.Clear();
            HashSet<int> excludeExit = new HashSet<int>(); //dörrar som är klara(leder till ett rum).

            HashSet<int> cantChange = new HashSet<int>();

            int addingExitsTo = 0; //rum som arbetas med.

            while (base.rooms.Count < numberOfRooms)
            {
                excludeExit.Clear();

                //kollar vilka dörrar som finns i rummet.
                for (int i = 0; i < rooms[addingExitsTo].doors.Length; i++)
                {
                    if (rooms[addingExitsTo].doors[i])
                    {
                        switch (i)
                        {
                            case 0:
                                if (CheckIfRoomExists(rooms[addingExitsTo].areaCoords, new Point(0, -1)))
                                {
                                    excludeExit.Add(i);
                                }
                                    break;
                            case 1:
                                if (CheckIfRoomExists(rooms[addingExitsTo].areaCoords, new Point(1, 0)))
                                {
                                    excludeExit.Add(i);
                                }
                                    break;
                            case 2:
                                if (CheckIfRoomExists(rooms[addingExitsTo].areaCoords, new Point(0, 1)))
                                {
                                    excludeExit.Add(i);
                                }
                                    break;
                            case 3:
                                if (CheckIfRoomExists(rooms[addingExitsTo].areaCoords, new Point(-1, 0)))
                                {
                                    excludeExit.Add(i);
                                }
                                    break;
                        }
                    }
                }
                int exitsToAdd = rooms[addingExitsTo].doors.Length - excludeExit.Count;

                while (exitsToAdd > 0 && base.rooms.Count < numberOfRooms) //genererar rum för varje dör, eller tills det inte ska genereras fler rum.
                {
                    int r = GeneratorUtility.GetRandomNumberExcluding(rand, excludeExit, 0, 3);
                    if (r == 0)
                    {
                        base.rooms.Add(new PreMadeArea(GetRandomRoomPath("Maps/South"),
                            base.rooms[addingExitsTo].areaCoords + new Point(0, -1), textures, content));
                        excludeExit.Add(0);
                        exitsToAdd--;
                    }
                    else if (r == 1)
                    {
                        base.rooms.Add(new PreMadeArea(GetRandomRoomPath("Maps/West"),
                            base.rooms[addingExitsTo].areaCoords + new Point(1, 0), textures, content));
                        excludeExit.Add(1);
                        exitsToAdd--;
                    }
                    else if (r == 2)
                    {
                        base.rooms.Add(new PreMadeArea(GetRandomRoomPath("Maps/North"),
                            base.rooms[addingExitsTo].areaCoords + new Point(0, 1), textures, content));
                        excludeExit.Add(2);
                        exitsToAdd--;
                    }
                    else if (r == 3)
                    {
                        base.rooms.Add(new PreMadeArea(GetRandomRoomPath("Maps/East"),
                            base.rooms[addingExitsTo].areaCoords + new Point(-1, 0), textures, content));
                        excludeExit.Add(3);
                        exitsToAdd--;
                    }
                }
                excludeRoom.Add(addingExitsTo); //lägger till rummet som har genererats så att den inte försöker generera till det rummet igen.

                addingExitsTo = GeneratorUtility.GetRandomNumberExcluding(rand, excludeRoom, 0, base.rooms.Count - 1); //tar ett nytt random rum som det sedan ska genereras nya rum till baserat på antal dörrar.

            }
            for (int i = 0; i < rooms.Count; i++)
            {
                Console.WriteLine(rooms[i].areaCoords);
            }
            RemoveOneWayDoors();
        }

        /// <summary>
        /// Tar bort dörrar som inte leder någonstans
        /// </summary>
        private void RemoveOneWayDoors()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].doors[0])
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].areaCoords ==
                            (rooms[i].areaCoords + new Point(0, -1)))
                        {
                            foundEntance = true;
                            if (!rooms[y].doors[2])
                            {
                                rooms[i].RemoveExit(TileType.NorthExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.NorthExit);
                    }
                }
                if (rooms[i].doors[1])
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].areaCoords ==
                            (rooms[i].areaCoords + new Point(1, 0)))
                        {
                            foundEntance = true;
                            if (!rooms[y].doors[3])
                            {
                                rooms[i].RemoveExit(TileType.EastExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.EastExit);
                    }
                }
                if (rooms[i].doors[2])
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].areaCoords ==
                            (rooms[i].areaCoords + new Point(0, 1)))
                        {
                            foundEntance = true;
                            if (!rooms[y].doors[0])
                            {
                                rooms[i].RemoveExit(TileType.SouthExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.SouthExit);
                    }
                }
                if (rooms[i].doors[3])
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].areaCoords ==
                            (rooms[i].areaCoords + new Point(-1, 0)))
                        {
                            foundEntance = true;
                            if (!rooms[y].doors[1])
                            {
                                rooms[i].RemoveExit(TileType.WestExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.WestExit);
                    }
                }
            }
        }

        /// <summary>
        /// Kolla om ett rum finns i riktning från ett annat rum.
        /// </summary>
        /// <param name="checkingFrom"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool CheckIfRoomExists(Point checkingFrom, Point direction)
        {
            Point roomToCheck = checkingFrom + direction;

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].areaCoords == roomToCheck)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// väljer en random fil i en folder från spelets bas folder.
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        private string GetRandomRoomPath(string FolderPath)
        {
            List<string> roomPaths = new List<string>();
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + FolderPath).Select(Path.GetFileName))
            {
                roomPaths.Add(FolderPath + "/" + file);
            }
            rand.Next(0, roomPaths.Count);

            return roomPaths[rand.Next(0, roomPaths.Count - 1)];
        }

        private int[] GetDoorArray(int roomNumber)
        {
            int[] roomsDoors = new int[4];

            for (int i = 0; i < rooms[roomNumber].doors.Length; i++)
            {

            }

            return roomsDoors;
        }

        protected override void ChangeRoom(Point RoomDirection, TileType entrance)
        {
            Point newRoomCoords = rooms[currentRoom].areaCoords + RoomDirection;

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].areaCoords == newRoomCoords)
                {
                    Console.WriteLine("Entering" + newRoomCoords);
                    currentRoom = i;
                    MapEventArgs args = new MapEventArgs(MapEventType.ChangeRoom);
                    args.Position = rooms[currentRoom].GetTileCenterOfType(entrance);
                    OnEvent(args);
                    CheckMovement(args.Position, RoomDirection);
                    break;
                }
            }
        }

    }
}
