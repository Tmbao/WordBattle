using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordBattle.InvisibleGameEntities
{
    class Camera : InvisibleGameEntity
    {
        private Matrix world, view, projection;

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }

        public Matrix WVP
        {
            get { return World * View * Projection; }
        }

        public Matrix InvertWVP
        {
            get { return Matrix.Invert(WVP); }
        }

        public Camera()
        {
            world = Matrix.Identity;
            view = Matrix.Identity;
            projection = Matrix.Identity;
        }
    }
}
