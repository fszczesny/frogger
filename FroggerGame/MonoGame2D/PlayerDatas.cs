using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;

namespace MonoGame2D
{
    public class PlayerDatas
    {
        private string name;
        private int points;

        public PlayerDatas()
        {

        }

        public string getName()
        {
            return this.name;
        }

        public int getPoints()
        {
            return this.points;
        }

        public void setName(string newName)
        {
            this.name = newName;
        }

        public void setPoints(int newPoints)
        {
            this.points = newPoints;
        }

        ~PlayerDatas()
        {

        }
    }
}
