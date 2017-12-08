using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    public class ControlChangeStreeat
    {
        // Variaveis de controle
        private int loopChangeStreatControl;
        private int loooChangeStreatIncrease;
        private Random random;

        // Construtor
        public ControlChangeStreeat()
        {
            random = new Random();
        }

        // Getters e setters
        public int getLoopControl()
        {
            return this.loopChangeStreatControl;
        }

        public int getLoopIncrease()
        {
            return this.loooChangeStreatIncrease;
        }

        public void setLoopControl(int control)
        {
            this.loopChangeStreatControl = control;
        }

        public void setLoopIncrease(int increase)
        {
            this.loooChangeStreatIncrease = increase;
        }

        // Verifica se tá na hora de trocar a pista do obstaculos
        public void verifyChangeStreatObstacles(List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft)
        {
            if (loopChangeStreatControl == loooChangeStreatIncrease)
            {
                loopChangeStreatControl = Constants.zero;
                changeStreet(obstacles, validLines, aceleretionToRigth, acelerationToLeft);
            }
            loopChangeStreatControl++;
        }

        // Metodo que realiza a troca de pista de um determinado obstaculo de forma aleatoria
        private void changeStreet(List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft)
        {
            int limit = obstacles.Count;
            if (limit > Constants.lessNumberToChangeStreat)
            {
                int position = random.Next(Constants.beginStreat, limit);
                int streeat = random.Next(Constants.beginStreat, Constants.endStreat);
                Obstacles obstacle = obstacles[position];
                int originalstreat = obstacle.getStreat();
                float originalY = obstacle.getY();
                float originalDx = obstacle.getDx();
                float originalAngle = obstacle.getAngle();
                while (obstacle.getStreat() == streeat)
                {
                    streeat = random.Next(Constants.beginStreat, Constants.endStreat);
                }
                obstacle.setStreat(streeat);
                obstacle.setY(validLines[streeat - Constants.one]);
                if (streeat % 2 != 0)
                {
                    obstacle.setDx((float)(aceleretionToRigth * (Constants.acelerationFactor * (streeat + Constants.two))));
                    obstacle.setAngle(Constants.angleObstacleToRigth);
                }
                else
                {
                    obstacle.setDx((float)(acelerationToLeft * (Constants.acelerationFactor * (streeat + Constants.two))));
                    obstacle.setAngle(Constants.angleObstacleToLeft);
                }
                if (obstacle.verifyColisionObsttacleWithObstacles(obstacles, position))
                {
                    obstacle.setStreat(originalstreat);
                    obstacle.setY(originalY);
                    obstacle.setDx(originalDx);
                    obstacle.setAngle(originalAngle);
                }
            }
        }

        // Destrutor
        ~ControlChangeStreeat()
        {

        }
    }
}
