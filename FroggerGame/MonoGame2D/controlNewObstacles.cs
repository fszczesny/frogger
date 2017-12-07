using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    public class ControlNewObstacles
    {
        private int loopNewObstaclesControl;
        private int loooNewObstaclesIncrease;
        private float scale;
        private float streeLeftLimit;
        private float streeRigthLimit;
        private List<int> lastInserts;
        private Random random;

        public ControlNewObstacles()
        {
            random = new Random();
            lastInserts = new List<int>();
        }

        public float getScale()
        {
            return this.scale;
        }

        public float getStreeLeftLimit()
        {
            return this.streeLeftLimit;
        }

        public float getStreeRigthLimit()
        {
            return this.streeRigthLimit;
        }

        public int getLoopControl()
        {
            return this.loopNewObstaclesControl;
        }

        public int getLoopIncrease()
        {
            return this.loooNewObstaclesIncrease;
        }

        public void setStreeLeftLimit(float toSet)
        {
            this.streeLeftLimit = toSet;
        }

        public void setStreeRigthLimit(float toSet)
        {
            this.streeRigthLimit = toSet;
        }

        public void setScale(float scaleToSet)
        {
            this.scale = scaleToSet;
        }

        public void setLoopControl(int control)
        {
            this.loopNewObstaclesControl = control;
        }

        public void setLoopIncrease(int increase)
        {
            this.loooNewObstaclesIncrease = increase;
        }

        // Verifica se tá na hora de por mais obstaculos
        public void verifyIfNeedMoreObstacles(GraphicsDevice device, List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft)
        {
            if (loopNewObstaclesControl == loooNewObstaclesIncrease)
            {
                loopNewObstaclesControl = 0;
                spawnNewObstacle(device, obstacles, validLines, aceleretionToRigth, acelerationToLeft);
            }
            loopNewObstaclesControl++;
        }

        // Metodo de inserção aleatoria de tipo e rua dos obstaculos
        public void spawnNewObstacle(GraphicsDevice device, List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft)
        {
            Obstacles cart;
            // Seleciona aleatoriamente um tipo de carroça
            int typeOfCart = random.Next(1, 4);
            switch (typeOfCart)
            {
                case 1:
                    cart = new Obstacles(device, Constants.redCartSprites, scale);
                    break;
                case 2:
                    cart = new Obstacles(device, Constants.greenCartSprites, scale);
                    break;
                default:
                    cart = new Obstacles(device, Constants.purpleCartSprites, scale);
                    break;
            }
            int streeat = random.Next(1, 7);
            //Seleciona aleatoriamente uma rua para a coarroça
            // Cuida pra não por duas carroças seguidas na mesma rua
            if (lastInserts.Count != 0)
            {
                while (lastInserts[lastInserts.Count - 1] == streeat)
                {
                    streeat = random.Next(1, 7);
                }
            }
            cart.setStreat(streeat);
            cart.setY(validLines[streeat - 1]);
            if (streeat % 2 != 0)
            {
                cart.setX(streeLeftLimit);
                cart.setDx((float)(aceleretionToRigth * (Constants.acelerationFactor * (streeat + 2))));
                cart.setAngle(Constants.angleObstacleToRigth);
            }
            else
            {
                cart.setX(streeRigthLimit);
                cart.setDx((float)(acelerationToLeft * (Constants.acelerationFactor * (streeat + 2))));
                cart.setAngle(Constants.angleObstacleToLeft);
            }
            if (validLines.Contains(cart.getY()))
            {
                lastInserts.Add(streeat);
                obstacles.Add(cart);
            }
        }

        // Verifica se o obstaculo já está em uma coodernada externa a tela utilizada
        public void VerifyIfObstaclesIsOutOfScreen(List<Obstacles> obstacles)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                if ((obstacles[i].getX() < streeLeftLimit) || (obstacles[i].getX() > streeRigthLimit))
                {
                    obstacles.RemoveAt(i);
                }
            }
        }

        // Metodo de atualização da posição de todos os obstaculos
        public void UpdateAllObstacles(List<Obstacles> obstacles, float elapsedTime)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Update(elapsedTime);
            }
        }

        // Metodo de desenho de todos os obstaculos na tela
        public void DrawAllObstacles(List<Obstacles> obstacles, SpriteBatch sprite)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(sprite);
            }
        }
    }
}
