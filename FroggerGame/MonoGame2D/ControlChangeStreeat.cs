using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    public class ControlChangeStreeat
    {
        public ControlChangeStreeat()
        {

        }

        // Verifica se tá na hora de trocar a pista do obstaculos
        public void verifyChangeStreatObstacles(List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft, Random random, float loopChangeStreatControl, float loooChangeStreatIncrease)
        {
            if (loopChangeStreatControl == loooChangeStreatIncrease)
            {
                loopChangeStreatControl = 0;
                this.changeStreet(obstacles, validLines, aceleretionToRigth, acelerationToLeft, random);
            }
            loopChangeStreatControl++;
        }

        // Metodo que realiza a troca de pista de um determinado obstaculo de forma aleatoria
        public void changeStreet(List<Obstacles> obstacles, List<float> validLines, float aceleretionToRigth, float acelerationToLeft, Random random)
        {
            int limit = obstacles.Count;
            if (limit > Constants.lessNumberToChangeStreat)
            {
                int position = random.Next(1, limit);
                int streeat = random.Next(1, 7);
                Obstacles obstacle = obstacles[position];
                int originalstreat = obstacle.getStreat();
                float originalY = obstacle.getY();
                float originalDx = obstacle.getDx();
                float originalAngle = obstacle.getAngle();
                while (obstacle.getStreat() == streeat)
                {
                    streeat = random.Next(1, 7);
                }
                obstacle.setStreat(streeat);
                obstacle.setY(validLines[streeat - 1]);
                if (streeat % 2 != 0)
                {
                    obstacle.setX((float)(aceleretionToRigth * (Constants.acelerationFactor * (streeat + 2))));
                    obstacle.setAngle(Constants.angleObstacleToRigth);
                }
                else
                {
                    obstacle.setDx((float)(acelerationToLeft * (Constants.acelerationFactor * (streeat + 2))));
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
    }
}
