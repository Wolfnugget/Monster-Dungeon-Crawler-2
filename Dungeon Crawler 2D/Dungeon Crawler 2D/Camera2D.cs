using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon_Crawler_2D
{
    class Camera2D
    {
        public Matrix transform;
        public Vector2 cameraPos;
        public Vector3 zoomVector;
        private Viewport view;
        private World.Room room;
        public float zoom;

        int windowWidth;
        int windowHeight;

        public Camera2D(Viewport view, int windowWidth, int windowHeight, World.Room room, float zoom)
        {
            this.zoom = zoom;
            this.view = view;
            this.room = room;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            zoomVector = new Vector3(zoom, zoom, 1);
        }

        public void SetPosition(Vector2 pos)
        {
            cameraPos = pos;

            for (int i = 0; i < room.roomBluePrint.Count; i++)
            {
                if ((-pos.X - 8) + (windowWidth / (2 * zoom)) > 0)
                {
                    transform = Matrix.CreateTranslation(0, (-pos.Y - 8) + (windowHeight / (2 * zoom)), 0) * Matrix.CreateScale(zoomVector);
                }
                if (pos.X > room.roomBluePrint[i].Length * 16 - 16)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(zoomVector);
                }
                if ((-pos.Y - 8) + (windowHeight / (2 * zoom)) > 0)
                {
                    transform = Matrix.CreateTranslation((-pos.X - 8) + (windowWidth / (2 * zoom)), 0, 0) * Matrix.CreateScale(zoomVector);
                }
                if (pos.Y > room.roomBluePrint.Count * 16 - 16)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(zoomVector);
                }
                else
                {
                    //LÅT STÅ
                    //transform = Matrix.CreateTranslation((-pos.X - 8) + (windowWidth / (2 * zoom)), (-pos.Y - 8) + (windowHeight / (2 * zoom)), 0) * Matrix.CreateScale(zoomVector);
                }
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