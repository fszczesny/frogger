using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    //Classe de contantes utilizadas no jogo
    static class Constants
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
        // Constantes de controle de loop do jogo junto a atualização de obstaculos
        public const int intervalBetwenNewObstacleLoop = 70;
        public const int intervalBetwenChangeStreat = 100;
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
        public const string valueFonts = "Score";
        // Constantes strings usadas como mensagens de interface
        public const string progressLevelMessage = "Press Enter to start the next level!";
        public const string deadAndNextLiveMessage = "Press Space to use the next live!";
        public const string winAllLevelsMessage = "You win all levels. Press Enter to restart!";
        public const string restartMessage = "Press Enter to restart!";
        public const string startMessage = "FROGGER - THE MEDIEVAL EDITION";
        public const string askForASpaceMessage = "Press Space to start";
        public const string livesMessage = "Lives: ";
        public const string timeMessage = "Time: ";
        public const string scoreMessage = "Score: ";
        // Constantes de controle do loop de ignorar o teclado
        public const int ignoreKyboardLoop = 200;
    }
}
