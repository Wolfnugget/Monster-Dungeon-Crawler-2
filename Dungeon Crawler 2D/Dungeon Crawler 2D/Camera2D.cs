using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    class Camera2D
    {
        public float zoom;
        public Matrix transform;
        public Vector2 position;
        protected float rotation;

        public Camera2D(float zoom, float rotation, Vector2 position)
        {
            this.zoom = zoom;
            this.rotation = rotation;
            this.position = position;
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0))
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateScale(new Vector3(zoom, zoom, 1))
                * Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2, 0));

            return transform;
        }
    }
}
