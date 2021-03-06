﻿using System;
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
        // Variaveis de estado e controle do jogo
        private float aceleretionToRigth;
        private float acelerationToLeft;
        private bool gameStarted;
        private bool gameOver;
        private bool gameGetName;
        private bool dead;
        private bool win;
        private int score;
        private int lives;
        private int level;
        private int pointsForWin;
        private float froggerPass;
        private int beginPause;
        List<float> validLines = new List<float>();
        private bool showResults;

        // Construtor
        public ControlGameParameters()
        {
            aceleretionToRigth = Constants.rigthAceleration;
            acelerationToLeft = Constants.leftAceleration;
            pointsForWin = Constants.pointsForWin;
        }

        // Getter e setters
        public List<float> getValidLines()
        {
            return this.validLines;
        }

        public bool getGameGetName()
        {
            return this.gameGetName;
        }

        public bool getShowResults()
        {
            return this.showResults;
        }

        public void setShowResults(bool s)
        {
            this.showResults = s;
        }

        public void setGameName(bool name)
        {
            this.gameGetName = name;
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

        public void setValidLines( List<float> validLines)
        {
            this.validLines = validLines;
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

        // Inicia parametros de acordo para o proximo nivel
        public void startNextLevelParameters(int levelAux)
        {
            froggerPass = (float)(froggerPass - Constants.decFroggerPass);
            score = score + (pointsForWin * lives * (levelAux - Constants.one)); // Ainda não leva em consideração o tempo
            acelerationToLeft = (float)(acelerationToLeft - Constants.decAceleration);
            aceleretionToRigth = (float)(aceleretionToRigth + Constants.decAceleration);
        }

        // Inicia parametros do nivel inicial
        public void startBeginParameters()
        {
            froggerPass = Constants.initialFroggerPass;
            score = Constants.initialScore;
            acelerationToLeft = Constants.leftAceleration;
            aceleretionToRigth = Constants.rigthAceleration;
        }

        // Atualização dos parametros conforme parametros passados
        public void startParametrs(bool isGameOver, bool isWin, bool isDead, bool theGameStart, int nunberOfLives, int initialScore, int initialLevel, bool name , bool show)
        {
            showResults = show;
            gameGetName = name;
            gameOver = isGameOver;
            win = isWin;
            dead = isDead;
            gameStarted = theGameStart;
            lives = nunberOfLives;
            score = initialScore;
            level = initialLevel;
        }

        // Atualização dos parametros por solicitação do teclado
        public void startParametersWhithKeyboard(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead, bool name, bool show)
        {
            showResults = show;
            gameGetName = name;
            gameStarted = theGameStarted;
            gameOver = theGameOver;
            win = thePlayerWined;
            dead = thePlayerDead;
        }

        // Verifica se o jogador ganhou o jogo
        public bool thePalyerWin(Player frooger, float screenHeight)
        {
            if (gameStarted && frooger.getY() <= screenHeight / Constants.verticalBeginPositionOfFrogger)
            {
                gameGetName = true;
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
            if (beginPause > Constants.zero)
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
                        if (lives == Constants.zero)
                        {
                            gameOver = true;
                        }
                        dead = true;
                        gameGetName = true;
                    }
                    win = thePalyerWin(frooger, screenHeight);
                }
            }
        }

        // Cria lista de ruas validas
        public void setValidLines(float screenHeight)
        {
            validLines.Clear();
            validLines.Add((float)(screenHeight - screenHeight / Constants.streat1));
            validLines.Add((float)(screenHeight - screenHeight / Constants.streat2));
            validLines.Add((float)(screenHeight - screenHeight / Constants.streat3));
            validLines.Add((float)(screenHeight - screenHeight / Constants.streat4));
            validLines.Add((float)(screenHeight - screenHeight / Constants.streat5));
            validLines.Add((float)(screenHeight - screenHeight / Constants.streat6));
        }

        // Destrutor
        ~ControlGameParameters()
        {

        }
    }
}
