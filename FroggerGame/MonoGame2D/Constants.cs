using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    //Classe de contantes utilizadas no jogo
    public static class Constants
    {
        // Constante de diretorio ativo
        public const string directory = "Content";
        // Constantes de movimentação do Frogger
        public const int initialFroggerPass = 5;
        public const float decFroggerPass = (float)0.5;
        public const float angleFrogger0 = 0;
        public const float angleFrogger90 = 300;
        public const float angleFrogger180 = 600;
        public const float angleFrogger270 = 900;
        // Contantes de movimentação dos obstaculos
        public const float acelerationFactor = (float)0.25;
        public const float decAceleration = (float)0.2;
        public const float rigthAceleration = 1;
        public const float leftAceleration = -1;
        public const float angleObstacleToRigth = 0;
        public const float angleObstacleToLeft = (float)600.05;
        public const int obstacleAcelerationPar = 3;
        // Constantes de controle de loop do jogo junto a atualização de obstaculos
        public const int intervalBetwenNewObstacleLoop = 50;
        public const int intervalBetwenChangeStreat = 70;
        public const int decFrequencyObstacle = 5;
        public const int decFrequencyChangeStreat = 20;
        public const int lessNumberToChangeStreat = 5;
        // Constantes de valores default de vida,nivel, pontos e etc do jogo
        public const int initialLives = 5;
        public const int initialLevel = 0;
        public const int initialScore = 0;
        public const int pointsForWin = 2;
        public const int maxLevel = 8;
        // Constantes de nome de arquivos a serem caregados
        public const string froggerSprite = "Content/frooger.png";
        public const string greenCartSprites = "Content/green_cart.png";
        public const string redCartSprites = "Content/red_cart.png";
        public const string purpleCartSprites = "Content/purple_cart.png";
        public const string backgroundSprite = "background";
        public const string startSprite = "start-splash";
        public const string gameoverSprite = "game-over";
        public const string bloodSprite = "blood";
        public const string froogerSpriteToTexture = "frooger";
        public const string skullSprite = "skull";
        public const string winSprite = "win";
        public const string gameMessagesFont = "GameState";
        public const string menorFont = "menor";
        public const string valueFonts = "Score";
        // Constantes strings usadas como mensagens de interface
        public const string progressLevelMessage = "Press Enter to start the next level!";
        public const string deadAndNextLiveMessage = "Press Space to use the next live!";
        public const string winAllLevelsMessage = "You win all levels. Press Enter to restart!";
        public const string restartMessage = "Press Enter to restart without saving!";
        public const string restartMessageTwo = "Enter your name and press insert to save your score!";
        public const string startMessage = "FROGGER - THE MEDIEVAL EDITION";
        public const string bestMessage = "Bast Players Historic:";
        public const string pressEnterMessage = "Press Enter to restart!";
        public const string askForASpaceMessage = "Press Space to start";
        public const string livesMessage = "Lives: ";
        public const string timeMessage = "Level: ";
        public const string scoreMessage = "Score: ";
        // Constantes de controle do loop de ignorar o teclado
        public const int ignoreKyboardLoop = 200;
        // Volres contantes
        public const int zero = 0;
        public const int one = 1;
        public const int two = 2;
        public const int tree = 3;
        public const int four = 4;
        // Contantes magicas de posicionamento de elementos
            public const int verticalBeginPositionOfFrogger = 5;
            // Valores de limite dos randomicos de pista e tipo de carro;
            public const int beginStreat = 1;
            public const int endStreat = 7;
            public const int beginType = 1;
            public const int endType = 4;
            // Valores do box de colisão
            public const float HitBoxPlayer = 0.25f;
            public const float HitBoxObstacle = 0.75f;
            // Constantes de limites
            public const int maxLimit = 20;
            public const int minLimit = 8;
            public const int leftAndRigthLimit = 17;
            public const int beginPosition = 5;
            // Constantes de determinação de ruas validas
            public const float streat1 = (float)4.5;
            public const float streat2 = (float)3.05;
            public const float streat3 = (float)2.475;
            public const float streat4 = (float)1.975;
            public const float streat5 = (float)1.725;
            public const float streat6 = (float)1.465;
             // Constante de proporção escrita de mensagem
            public const float messageConst = (float)0.81;
            // Constantes de posicionamento feedback de pontos, vidas e tempo
            public const float livesNameWidth = (float)0.62;
            public const float livesNameHeigth = (float)0.046;
            public const float timeNameWidth = (float)0.036;
            public const float timeNameHeigth = (float)0.046;
            public const float scoreNameWidth = (float)0.82;
            public const float scoreNameHeigth = (float)0.046;
            public const float livesValueWidth = (float)0.7;
            public const float livesValueHeigth = (float)0.046;
            public const float timeValueWidth = (float)0.105;
            public const float timeValueHeigth = (float)0.046;
            public const float scoreValueWidth = (float)0.9;
            public const float scoreValueHeigth = (float)0.046;
            public const float spacingMenu = (float)0.075;
            public const float spacingEnter = (float)0.4;
            public const float spacingInset = (float)0.5;
            public const int bestNumbers = 10;
    }
}
