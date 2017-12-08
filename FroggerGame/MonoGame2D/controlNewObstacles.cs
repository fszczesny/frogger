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
        // Variaveis de controle
        private int loopNewObstaclesControl;
        private int loooNewObstaclesIncrease;
        private float scale;
        private float streeLeftLimit;
        private float streeRigthLimit;
        private List<int> lastInserts;
        private Random random;

        // Construtor
        public ControlNewObstacles()
        {
            random = new Random();
            lastInserts = new List<int>();
        }

        // Getters e setters
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
                loopNewObstaclesControl = Constants.zero;
                spawnNewObstacle(device, obstacles, validLines, aceleretionToRigth, acelerationToLeft);
            }
            loopNewObstaclesControl++;
        }

        // Metodo de inserção aleatoria de tipo e rua dos obstaculos
        public void spawnNewObstacle(GraphicsDevice device, List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft)
        {
            Obstacles cart;
            // Seleciona aleatoriamente um tipo de carroça
            int typeOfCart = random.Next(Constants.beginType, Constants.endType);
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
            int streeat = random.Next(Constants.beginStreat, Constants.endStreat);
            //Seleciona aleatoriamente uma rua para a coarroça
            // Cuida pra não por duas carroças seguidas na mesma rua
            if (lastInserts.Count != Constants.zero)
            {
                while (lastInserts[lastInserts.Count - Constants.one] == streeat)
                {
                    streeat = random.Next(Constants.beginStreat, Constants.endStreat);
                }
            }
            cart.setStreat(streeat);
            cart.setY(validLines[streeat - Constants.one]);
            if (streeat % Constants.two != Constants.zero)
            {
                cart.setX(streeLeftLimit);
                cart.setDx((float)(aceleretionToRigth * (Constants.acelerationFactor * (streeat + Constants.two))));
                cart.setAngle(Constants.angleObstacleToRigth);
            }
            else
            {
                cart.setX(streeRigthLimit);
                cart.setDx((float)(acelerationToLeft * (Constants.acelerationFactor * (streeat + Constants.two))));
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
            for (int i = Constants.zero; i < obstacles.Count; i++)
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
            for (int i = Constants.zero; i < obstacles.Count; i++)
            {
                obstacles[i].Update(elapsedTime);
            }
        }

        // Metodo de desenho de todos os obstaculos na tela
        public void DrawAllObstacles(List<Obstacles> obstacles, SpriteBatch sprite)
        {
            for (int i = Constants.zero; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(sprite);
            }
        }

        // Destrutor
        ~ControlNewObstacles()
        {

        }
    }
}
