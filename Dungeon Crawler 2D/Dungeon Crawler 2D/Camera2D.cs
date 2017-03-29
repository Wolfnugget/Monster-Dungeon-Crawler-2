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

        public Camera2D(Viewport view, List<string> levelList)
        {
            this.view = view;
            this.levelList = levelList;
        }

        public void SetPosition(Vector2 pos)
        {
            this.cameraPos = pos;

            for (int i = 0; i < levelList.Count; i++)
            {
                if (pos.X < 512)
                {
                    transform = Matrix.CreateTranslation(0, 0, 0);
                }
                else if (pos.X > levelList[i].Length * 64 - 576)
                {
                    transform = Matrix.CreateTranslation(-levelList[i].Length * 64 + 1088, 0, 0);
                }
                else
                {
                    transform = Matrix.CreateTranslation(-pos.X + view.Width / 2 - 32, 0, 0);
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