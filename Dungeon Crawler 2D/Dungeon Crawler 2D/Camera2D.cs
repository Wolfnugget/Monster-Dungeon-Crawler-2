using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon_Crawler_2D
{
    class Camera2D
    {
        private HUDManager bars;
        public Matrix transform;
        public Vector2 cameraPos;
        private Vector2 screenCenter;
        private Vector2 roomSize;
        private Vector3 zoomVector;
        private Viewport view;
        private World.Map map;
        public float zoom;

        private int windowWidth;
        private int windowHeight;

        public Camera2D(HUDManager hud, Viewport view, int windowWidth, int windowHeight, World.Map map, float zoom)
        {
            this.bars = hud;
            this.zoom = zoom;
            this.view = view;
            this.map = map;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            screenCenter = new Vector2(windowWidth / (2 * zoom), windowHeight / (2 * zoom));
            roomSize = new Vector2(map.rooms[map.currentLocation].tiles.GetLength(1) * 16, map.rooms[map.currentLocation].tiles.GetLength(0) * 16);
            zoomVector = new Vector3(zoom, zoom, 0);
        }

        public void SetPosition(Vector2 pos)
        {
            roomSize = new Vector2(map.rooms[map.currentLocation].tiles.GetLength(1) * 16, map.rooms[map.currentLocation].tiles.GetLength(0) * 16);
            cameraPos = pos;

            //Is the room bigger than the screen? (both X, and Y)
            if (roomSize.X >= (screenCenter.X * 2) - ((bars.sideBarWidth * 2) / zoom) && roomSize.Y >= screenCenter.Y * 2)
            {
                transform = Matrix.CreateTranslation(-cameraPos.X + screenCenter.X, -cameraPos.Y + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);

                //Has it reached the left edge?
                if (-cameraPos.X + screenCenter.X > bars.sideBarWidth / zoom)
                {
                    transform = Matrix.CreateTranslation(bars.sideBarWidth / zoom, -cameraPos.Y + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the top edge?
                if (-cameraPos.Y + screenCenter.Y > 0)
                {
                    transform = Matrix.CreateTranslation(-cameraPos.X + screenCenter.X, 0, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the right edge?
                if (cameraPos.X + screenCenter.X > roomSize.X + (bars.sideBarWidth / zoom))
                {
                    transform = Matrix.CreateTranslation(-roomSize.X + (screenCenter.X * 2) - (bars.sideBarWidth / zoom), -cameraPos.Y + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the bottom edge?
                if (cameraPos.Y + screenCenter.Y > roomSize.Y)
                {
                    transform = Matrix.CreateTranslation(-cameraPos.X + screenCenter.X, -roomSize.Y + screenCenter.Y * 2, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the left edge and the top edge?
                if (-cameraPos.X + screenCenter.X > bars.sideBarWidth / zoom && -cameraPos.Y + screenCenter.Y > 0)
                {
                    transform = Matrix.CreateTranslation(bars.sideBarWidth / zoom, 0, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the top edge and the right edge?
                if (-cameraPos.Y + screenCenter.Y > 0 && cameraPos.X + screenCenter.X > roomSize.X + (bars.sideBarWidth / zoom))
                {
                    transform = Matrix.CreateTranslation(-roomSize.X + (screenCenter.X * 2) - (bars.sideBarWidth / zoom), 0, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the right edge and the bottom edge?
                if (cameraPos.X + screenCenter.X > roomSize.X + (bars.sideBarWidth / zoom) && cameraPos.Y + screenCenter.Y > roomSize.Y)
                {
                    transform = Matrix.CreateTranslation(-roomSize.X + (screenCenter.X * 2) - (bars.sideBarWidth / zoom), -roomSize.Y + screenCenter.Y * 2, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the bottom edge and the left edge?
                if (cameraPos.Y + screenCenter.Y > roomSize.Y && -cameraPos.X + screenCenter.X > bars.sideBarWidth / zoom)
                {
                    transform = Matrix.CreateTranslation(bars.sideBarWidth / zoom, -roomSize.Y + screenCenter.Y * 2, 0) * Matrix.CreateScale(zoomVector);
                }
            }

            //Is the room bigger than the screen? (X, but not Y)
            if (roomSize.X >= (screenCenter.X * 2) - ((bars.sideBarWidth * 2)  / zoom) && roomSize.Y < screenCenter.Y * 2)
            {
                transform = Matrix.CreateTranslation(-cameraPos.X + screenCenter.X, (-roomSize.Y / 2) + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
                //Has it reached the left edge?
                if (-cameraPos.X + screenCenter.X > bars.sideBarWidth / zoom)
                {
                    transform = Matrix.CreateTranslation(bars.sideBarWidth / zoom, (-roomSize.Y / 2) + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the right edge?
                if (cameraPos.X + screenCenter.X > roomSize.X + (bars.sideBarWidth / zoom))
                {
                    transform = Matrix.CreateTranslation(-roomSize.X + (screenCenter.X * 2) - (bars.sideBarWidth / zoom), (-roomSize.Y / 2) + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
                }
            }

            //Is the room bigger than the screen? (Y, but not X)
            if (roomSize.X < screenCenter.X * 2 && roomSize.Y >= screenCenter.Y * 2)
            {
                transform = Matrix.CreateTranslation((-roomSize.X / 2) + screenCenter.X, -cameraPos.Y + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
                //Has it reached the top edge?
                if (-cameraPos.Y + screenCenter.Y > 0)
                {
                    transform = Matrix.CreateTranslation((-roomSize.X / 2) + screenCenter.X, 0, 0) * Matrix.CreateScale(zoomVector);
                }

                //Has it reached the bottom edge?
                if (cameraPos.Y + screenCenter.Y > roomSize.Y)
                {
                    transform = Matrix.CreateTranslation((-roomSize.X / 2) + screenCenter.X, -roomSize.Y + screenCenter.Y * 2, 0) * Matrix.CreateScale(zoomVector);
                }
            }

            //Is the room smaller than the screen ? (both X, and Y)
            if (roomSize.X < (screenCenter.X * 2) - ((bars.sideBarWidth * 2) / zoom) && roomSize.Y < screenCenter.Y * 2)
            {
                transform = Matrix.CreateTranslation((-roomSize.X / 2) + screenCenter.X, (-roomSize.Y / 2) + screenCenter.Y, 0) * Matrix.CreateScale(zoomVector);
            }
        }

        public Vector2 GetPosition()
        {
            return cameraPos;
        }

        public Matrix GetTransform()
        {
            return transform;
        }
    }
}