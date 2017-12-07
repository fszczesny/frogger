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
    public class ControlGameParameters
    {
        private float aceleretionToRigth;
        private float acelerationToLeft;
        private bool gameStarted;
        private bool gameOver;
        private bool dead;
        private bool win;
        private int score;
        private int lives;
        private int level;
        private int pointsForWin;
        private float froggerPass;
        private int beginPause;

        public ControlGameParameters()
        {
            aceleretionToRigth = Constants.rigthAceleration;
            acelerationToLeft = Constants.leftAceleration;
            pointsForWin = Constants.pointsForWin;
        }

        public float getAcelerationToR()
        {
            return this.aceleretionToRigth;
        }

        public float getAcelerationToL()
        {
            return this.acelerationToLeft;
        }

        public bool getGameOver()
        {
            return this.gameOver;
        }

        public bool getGameStarted()
        {
            return this.gameStarted;
        }

        public bool getDead()
        {
            return this.dead;
        }

        public bool getWin()
        {
            return this.win;
        }

        public int getscore()
        {
            return this.score;
        }

        public int getLives()
        {
            return this.lives;
        }

        public int getLevel()
        {
            return this.level;
        }

        public int getPoints()
        {
            return this.pointsForWin;
        }

        public int getPause()
        {
            return this.beginPause;
        }

        public float getPass()
        {
            return this.froggerPass;
        }

        public void setAcelerationToR(float a)
        {
            this.aceleretionToRigth = a;
        }

        public void setAcelerationToL(float a)
        {
            this.acelerationToLeft = a;
        }

        public void setGameOver(bool b)
        {
            this.gameOver = b;
        }

        public void setGameStarted(bool b)
        {
            this.gameStarted = b;
        }

        public void setDead(bool b)
        {
            this.dead = b;
        }

        public void setWin(bool b)
        {
            this.win = b;
        }

        public void setscore(int i)
        {
            this.score = i;
        }

        public void setLives(int i)
        {
            this.lives = i;
        }

        public void setLevel(int i)
        {
            this.level = i;
        }

        public void setPoints(int i)
        {
            this.pointsForWin = i;
        }

        public void setPause(int i)
        {
            this.beginPause = i;
        }

        public void setPass(float a)
        {
            this.froggerPass = a;
        }

        public void startNextLevelParameters(int levelAux)
        {
            froggerPass = (float)(froggerPass - Constants.decFroggerPass);
            score = score + (pointsForWin * lives * (levelAux - 1));
            acelerationToLeft = (float)(acelerationToLeft - Constants.decAceleration);
            aceleretionToRigth = (float)(aceleretionToRigth + Constants.decAceleration);
        }

        public void startBeginParameters()
        {
            froggerPass = Constants.initialFroggerPass;
            score = Constants.initialScore;
            acelerationToLeft = Constants.leftAceleration;
            aceleretionToRigth = Constants.rigthAceleration;
        }

        public void startParametrs(bool isGameOver, bool isWin, bool isDead, bool theGameStart, int nunberOfLives, int initialScore, int initialLevel)
        {
            gameOver = isGameOver;
            win = isWin;
            dead = isDead;
            gameStarted = theGameStart;
            lives = nunberOfLives;
            score = initialScore;
            level = initialLevel;
        }

        public void startParametersWhithKeyboard(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead)
        {
            gameStarted = theGameStarted;
            gameOver = theGameOver;
            win = thePlayerWined;
            dead = thePlayerDead;
        }

        // Verifica se o jogador ganhou o jogo
        public bool thePalyerWin(Player frooger, float screenHeight)
        {
            if (gameStarted && frooger.getY() <= screenHeight / 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Controla colisões e seus ecos nas ações do jogo
        public void colisionsControl(float elapsedTime, Player frooger, List<Obstacles> obstacles, Texture2D bloodTexture, float screenHeight)
        {
            if (beginPause > 0)
            {
                beginPause--;
            }
            else
            {
                if (!win && !gameOver && !dead)
                {
                    frooger.Update(elapsedTime);
                    if (frooger.verifyColisionWithObstacles(obstacles))
                    {
                        lives--;
                        frooger.setTexture(bloodTexture);
                        if (lives == 0)
                        {
                            gameOver = true;
                        }
                        dead = true;
                    }
                    win = thePalyerWin(frooger, screenHeight);
                }
            }
        }
    }
}
