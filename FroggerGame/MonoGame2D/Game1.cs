using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace MonoGame2D
{
    // Medieval Frooger Version 1.0 - Versão desiginada a etapas 1 e 2 de definição do trabalho

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
            public const int intervalBetwenChangeStreat = 300;
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

    public class Game1 : Game
    {
        // Declaração de variaveis globias dentre a classe
        // Variaveis de ambiente grafico
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;
            SpriteFont stateFont;
            SpriteFont scoreFont;
            Texture2D startGameSplash;
            Texture2D gameOverTexture;
            Texture2D winTexture;
            Texture2D background;
            Texture2D bloodTexture;
            Texture2D froggerTexture;
            Texture2D skullTexture;
            float scale;
        // Variaveis de posicionamento e limitação do ruas
            float screenWidth;
            float screenHeight;
            float streeLeftLimit;
            float streeRigthLimit;
            List<float> validLines = new List<float>();
        // Variaveis posicionamento angular e aceleração todas já iniializadas aqui
            float angleToRight = Constants.angleObstacleToRigth;
            float angleToLeft = Constants.angleObstacleToLeft;
            float aceleretionToRigth = Constants.rigthAceleration;
            float acelerationToLeft = Constants.leftAceleration;
        // Variaveis de controle de estado de jogo
            bool gameStarted;
            bool gameOver;
            bool dead;
            bool win;
            int score;
            int lives;
            int level;
            int loopNewObstaclesControl;
            int loopChangeStreatControl;
            int loooNewObstaclesIncrease;
            int loooChangeStreatIncrease;
            int pointsForWin = Constants.pointsForWin;
            float froggerPass;
            int beginPause;
        // Variavel para geração ramdomica
            Random random;
        // Declaração do objeto Player que representa o frogger
            Player frooger;
        // Declaração da lista de obstaculos e sua lista auxiliar de frequencia
            List<Obstacles> obstacles = new List<Obstacles>();
            List<int> lastInserts = new List<int>();
        // Fim da declaração de globais da classe

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Constants.directory;
        }

        // Metodo de inicialização
        protected override void Initialize()
        {
            base.Initialize();
            startParametrs(false, false, false, false, Constants.initialLives, Constants.initialScore, Constants.initialLevel, Constants.intervalBetwenNewObstacleLoop, Constants.intervalBetwenChangeStreat, Constants.initialFroggerPass);
            random = new Random();
            startScreenConfigs();
        }

        // Metodo de carga de elementos externos
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadTextureAndFontStyles();
            // Carrega sprite do player
            frooger = new Player(GraphicsDevice, Constants.froggerSprite, ScaleToHighDPI(0.3f));
            scale = ScaleToHighDPI(1.3f);
        }

        // Metodo de descarga de elementos externos
        protected override void UnloadContent()
        {
        }

        // Metodo de atualização do estatus dos elementos
        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler();
            verifyIfNeedMoreObstacles();
            verifyChangeStreatObstacles();
            colisionsControl(elapsedTime);
            UpdateAllObstacles(elapsedTime);
            VerifyIfObstaclesIsOutOfScreen();         
            base.Update(gameTime);
        }

        // Metodo de desenho dos elementos graficos
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // Desenha objetos
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);
            if (beginPause <= 0)
            {
                frooger.Draw(spriteBatch);
            }
            DrawAllObstacles(spriteBatch);
            // Se o jogo ainda não começou fica em tela de inicio
            if (!gameStarted)
            {
                showBeforeStartScreen(spriteBatch);
            }
            else
            {
                drawInterfaceOfPontuation();
                // Se ganhou
                if (win)
                {
                    // Verifica se atingiu nivel maximo
                    if (level == Constants.maxLevel)
                    {                       
                        drawStateScreen(Constants.winAllLevelsMessage, winTexture);                       
                    }
                    else
                    {
                        drawStateScreen(Constants.progressLevelMessage, winTexture);
                    }
                }
                // Se game over
                if (gameOver)
                {                   
                    drawStateScreen(Constants.restartMessage, gameOverTexture);
                }
                // Se morreu, mas tem vida ainda
                else if (dead)
                {
                    drawStateScreen(Constants.deadAndNextLiveMessage, skullTexture);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }



        // Metodo de inicio do jogo
        public void StartGame()
        {
            beginPause = Constants.ignoreKyboardLoop;
            level++;
            frooger.setTexture(froggerTexture);
            frooger.setX(screenWidth / 2);
            frooger.setY(screenHeight - (screenHeight / 8));
            frooger.setAngle(Constants.angleFrogger0);
            verifyLevel(level);
            obstacles.Clear();
            loopNewObstaclesControl = 0;
            loopChangeStreatControl = 0;
        }

        // Seta parametros para uso da proxima vida
        public void StartNextLive(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead)
        {
            frooger.setAngle(Constants.angleFrogger0);
            frooger.setTexture(froggerTexture);
            frooger.setX(screenWidth / 2);
            frooger.setY(screenHeight - (screenHeight / 8));
            gameStarted = theGameStarted;
            gameOver = theGameOver;
            win = thePlayerWined;
            dead = thePlayerDead;
        }

        // Conjtrola colisões e seus ecos nas ações do jogo
        public void colisionsControl(float elapsedTime)
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
                    win = thePalyerWin();
                }
            }
        }      

        // Metodo de leitura do teclado
        void KeyboardHandler()
        {
            KeyboardState state = Keyboard.GetState();
            // Encerra o jogo se a tecla esc for precionada
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            // Inicia o jogo de a tecla espaço for precionada.
            if (!gameStarted)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    startParametersWhithKeyboard(true, false, false, false);
                }
                return;
            }
            // Reinicia se for precionado enter após game over
            if (gameOver && state.IsKeyDown(Keys.Enter))
            {
                startParametrs(this.gameOver, this.win, this.dead, this.gameStarted, Constants.initialLives, Constants.initialScore, Constants.initialLevel, Constants.intervalBetwenNewObstacleLoop, Constants.intervalBetwenChangeStreat, Constants.initialFroggerPass);
                startParametersWhithKeyboard(true, false, false, false);
            }
            if (dead && state.IsKeyDown(Keys.Space) && !gameOver)
            {
                StartNextLive(true, false, false, false);
            }
            if (win && state.IsKeyDown(Keys.Enter))
            {
                if (level == Constants.maxLevel)
                {
                    level = 0;
                }
                startParametersWhithKeyboard(true, false, false, false);
            }
            if (beginPause <= 0 && !win && !gameOver && !dead)
            {

                // Controla teclas de direção com controle de area da tela a ser usada
                if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                {
                    frooger.setAngle(Constants.angleFrogger0);
                    if (frooger.getY() > (screenHeight / 5))
                    {
                        frooger.setY(frooger.getY() - froggerPass);
                    }
                }
                else if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                {
                    frooger.setAngle(Constants.angleFrogger180);
                    if (frooger.getY() < (screenHeight - (screenHeight / 8)))
                    {
                        frooger.setY(frooger.getY() + froggerPass);
                    }
                }
                else if (state.IsKeyDown(Keys.Left) || (state.IsKeyDown(Keys.A)))
                {
                    frooger.setAngle(Constants.angleFrogger90);
                    if (frooger.getX() > screenWidth / 20)
                    {
                        frooger.setX(frooger.getX() - froggerPass);
                    }
                }
                else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                {
                    frooger.setAngle(Constants.angleFrogger270);
                    if (frooger.getX() < (screenWidth - (screenWidth / 20)))
                    {
                        frooger.setX(frooger.getX() + froggerPass);
                    }
                }
            }
        }

        // Metodo de identificação e escalonamento conforme dpis da tela utilizada
        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }

        // Metodo de atualização da posição de todos os obstaculos
        public void UpdateAllObstacles(float elapsedTime)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Update(elapsedTime);
            }
        }

        // Metodo de desenho de todos os obstaculos na tela
        public void DrawAllObstacles(SpriteBatch sprite)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(sprite);
            }
        }

        // Verifica se o obstaculo já está em uma coodernada externa a tela utilizada
        public void VerifyIfObstaclesIsOutOfScreen()
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                if ((obstacles[i].getX() < streeLeftLimit) || (obstacles[i].getX() > streeRigthLimit))
                {
                    obstacles.RemoveAt(i);
                }
            }
        }

        // Iniciaza parametros de jogo
        public void startParametrs(bool isGameOver, bool isWin, bool isDead, bool theGameStart, int nunberOfLives, int initialScore, int initialLevel, int initialObstacleFrequency, int changeStreatFrequency, int initialFroggerPass)
        {
            gameOver = isGameOver;
            win = isWin;
            dead = isDead;
            gameStarted = theGameStart;
            lives = nunberOfLives;
            score = initialScore;
            level = initialLevel;
            loooNewObstaclesIncrease = initialObstacleFrequency;
            loooChangeStreatIncrease = changeStreatFrequency;
            froggerPass = initialFroggerPass;
        }

        public void startParametersWhithKeyboard(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead)
        {
            StartGame();
            gameStarted = theGameStarted;
            gameOver = theGameOver;
            win = thePlayerWined;
            dead = thePlayerDead;
        }

        // Carrega textura de background do jogo 
        // Carrega estilo de fontes 
        public void loadTextureAndFontStyles()
        {
            background = Content.Load<Texture2D>(Constants.backgroundSprite);
            startGameSplash = Content.Load<Texture2D>(Constants.startSprite);
            gameOverTexture = Content.Load<Texture2D>(Constants.gameoverSprite);
            skullTexture = Content.Load<Texture2D>(Constants.skullSprite);
            winTexture = Content.Load<Texture2D>(Constants.winSprite);
            bloodTexture = Content.Load<Texture2D>(Constants.bloodSprite);
            froggerTexture = Content.Load<Texture2D>(Constants.froogerSpriteToTexture);
            stateFont = Content.Load<SpriteFont>(Constants.gameMessagesFont);
            scoreFont = Content.Load<SpriteFont>(Constants.valueFonts);
        }

        // Inicializa escala de frames da tela utilizada
        // Inicializa em tela cheia
        // Inicializa com ponteiro do mouse oculto
        public void startScreenConfigs()
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);
            this.IsMouseVisible = false;
            streeLeftLimit = -screenWidth / 17;
            streeRigthLimit = screenWidth + screenWidth / 17;
            validLines.Clear();
            validLines.Add((float)(screenHeight - screenHeight / 4.5));
            validLines.Add((float)(screenHeight - screenHeight / 3.05));
            validLines.Add((float)(screenHeight - screenHeight / 2.475));
            validLines.Add((float)(screenHeight - screenHeight / 1.975));
            validLines.Add((float)(screenHeight - screenHeight / 1.725));
            validLines.Add((float)(screenHeight - screenHeight / 1.465));
        }

        // Verifica se tá na hora de por mais obstaculos
        public void verifyIfNeedMoreObstacles()
        {
            if (loopNewObstaclesControl == loooNewObstaclesIncrease)
            {
                loopNewObstaclesControl = 0;
                spawnNewObstacle();
            }
            loopNewObstaclesControl++;
        }

        // Verifica se tá na hora de trocar a pista do obstaculos
        public void verifyChangeStreatObstacles()
        {
            if (loopChangeStreatControl == loooChangeStreatIncrease)
            {
                loopChangeStreatControl = 0;
                changeStreet();
            }
            loopChangeStreatControl++;
        }

        // Verifica se o jogador ganhou o jogo
        public bool thePalyerWin()
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

        // Carrega tela preta no inicio de jogo aguardando um espaço para iniciar
        // Insere temanho de fonte para escrita
        // Escreve centralizado
        public void showBeforeStartScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(startGameSplash, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);
            String title = Constants.startMessage;
            String pressSpace = Constants.askForASpaceMessage;
            Vector2 titleSize = stateFont.MeasureString(title);
            Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);
            spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), Color.ForestGreen);
            spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), Color.White);
        }

        // Desenha a pontuação
        // Desenha a quantidade de vidas
        // Desenha o timer
        public void drawInterfaceOfPontuation()
        {
            spriteBatch.DrawString(scoreFont, Constants.scoreMessage, new Vector2((float)(screenWidth * 0.82), (float)(screenHeight * 0.046)), Color.Black);
            spriteBatch.DrawString(scoreFont, score.ToString(), new Vector2((float)(screenWidth * 0.9), (float)(screenHeight * 0.046)), Color.Black);
            spriteBatch.DrawString(scoreFont, Constants.livesMessage, new Vector2((float)(screenWidth * 0.62), (float)(screenHeight * 0.046)), Color.Black);
            spriteBatch.DrawString(scoreFont, lives.ToString(), new Vector2((float)(screenWidth * 0.7), (float)(screenHeight * 0.046)), Color.Black);
            spriteBatch.DrawString(scoreFont, Constants.timeMessage, new Vector2((float)(screenWidth * 0.036), (float)(screenHeight * 0.046)), Color.Black);
        }

        // Desenha tela e escrita centrais na tela de win e game over
        public void drawStateScreen(string pressEnter, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Vector2(screenWidth / 2 - texture.Width / 2, screenHeight / 4 - texture.Width / 2), Color.White);
            Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);
            spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, (float)(screenHeight * 0.81)), Color.White);
        }

        // Metodo de controle do avanço de nivel
        public void verifyLevel(int levelAux)
        {
            if (levelAux > 1 && levelAux < (Constants.maxLevel+1))
            {
                loooNewObstaclesIncrease = loooNewObstaclesIncrease - Constants.decFrequencyObstacle;
                loooChangeStreatIncrease = loooChangeStreatIncrease - Constants.decFrequencyChangeStreat;
                froggerPass = (float)(froggerPass - Constants.decFroggerPass);
                score = score + (pointsForWin* lives * (levelAux - 1));
                acelerationToLeft = (float)(acelerationToLeft - Constants.decAceleration);
                aceleretionToRigth = (float)(aceleretionToRigth + Constants.decAceleration);
            }
            else
            {
                loooNewObstaclesIncrease = Constants.intervalBetwenNewObstacleLoop;
                loooChangeStreatIncrease = Constants.intervalBetwenChangeStreat;
                froggerPass = Constants.initialFroggerPass;
                score = Constants.initialScore;
                acelerationToLeft = Constants.leftAceleration;
                aceleretionToRigth = Constants.rigthAceleration;
            }
        }

        // Metodo de inserção aleatoria de tipo e rua dos obstaculos
        public void spawnNewObstacle()
        {
            Obstacles cart;
            // Seleciona aleatoriamente um tipo de carroça
            int typeOfCart = random.Next(1, 4);
            switch (typeOfCart)
            {
                case 1:
                    cart = new Obstacles(GraphicsDevice, Constants.redCartSprites, scale);
                    break;
                case 2:
                    cart = new Obstacles(GraphicsDevice, Constants.greenCartSprites, scale);
                    break;
                default:
                    cart = new Obstacles(GraphicsDevice, Constants.purpleCartSprites, scale);
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
                cart.setAngle(angleToRight);
            }
            else
            {
                cart.setX(streeRigthLimit);
                cart.setDx((float)(acelerationToLeft * (Constants.acelerationFactor * (streeat + 2))));
                cart.setAngle(angleToLeft);
            }
            if (validLines.Contains(cart.getY()))
            {
                lastInserts.Add(streeat);
                obstacles.Add(cart);
            }
        }

        // Metodo que realiza a troca de pista de um determinado obstaculo de forma aleatoria
        public void changeStreet()
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
                    obstacle.setAngle(angleToRight);
                }
                else
                {
                    obstacle.setDx((float)(acelerationToLeft * (Constants.acelerationFactor * (streeat + 2))));
                    obstacle.setAngle(angleToLeft);
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
