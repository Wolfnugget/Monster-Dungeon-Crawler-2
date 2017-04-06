using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dungeon_Crawler_2D
{
    class Camera2D
    {
        public Matrix transform;
        public Vector2 cameraPos;
        private Viewport view;
        private List<string> levelList;
        public float zoom;

        public Camera2D(Viewport view, List<string> levelList, float zoom)
        {
            this.zoom = zoom;
            this.view = view;
            this.levelList = levelList;
        }

        public void SetPosition(Vector2 pos)
        {
            cameraPos = pos;

            for (int i = 0; i < levelList.Count; i++)
            {
                if (pos.X < 0)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                }
                else if (pos.X > levelList[i].Length * 16 - 16)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                }
                else if (pos.Y < 0)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                }
                else if (pos.Y > levelList.Count * 16 - 16)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                }
                else
                {
                    transform = Matrix.CreateTranslation(-pos.X + view.Width / 2, -pos.Y + view.Height / 2, 0) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
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