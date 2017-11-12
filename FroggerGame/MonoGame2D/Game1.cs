using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace MonoGame2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float screenWidth;
        float screenHeight;
        float streetOneH;
        float streetTwoH;
        float streetTreeH;
        float streetFourH;
        float streetFiveH;
        float streetSixH;
        float streeLeftLimit;
        float streeRigthLimit;
        float angleToRight = 0;
        float angleToLeft = (float)600.05;
        float aceleretionToRigth = 1;
        float acelerationToLeft = -1;
        float scale;
        Texture2D background;
        bool gameStarted;
        bool gameOver;
        int score;
        int lives;
        SpriteFont stateFont;
        Random random;
        SpriteFont scoreFont;
        Texture2D startGameSplash;
        Texture2D gameOverTexture;
        Player frooger;
        List<float> screen = new List<float>();
        List<int> lines = new List<int>();
        List<Obstacles> obstacles = new List<Obstacles>();
        int loopControl;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Metodo de inicialização
        protected override void Initialize()
        {
            base.Initialize();

            // Iniciaza parametros de jogo
            gameOver = false;
            gameStarted = false;
            lives = 0;
            score = 0;

            random = new Random();

            // Inicializa escala de frames da tela utilizada
            // Inicializa em tela cheia
            // Inicializa com ponteiro do mouse oculto
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);
            this.IsMouseVisible = false;

        }

        // Metodo de carga de elementos externos
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carrega textura de background do jogo
            background = Content.Load<Texture2D>("background");
            startGameSplash = Content.Load<Texture2D>("start-splash");
            gameOverTexture = Content.Load<Texture2D>("game-over");

            // Carrega sprites do jogo
            frooger = new Player(GraphicsDevice, "Content/frooger.png", ScaleToHighDPI(0.3f));
            scale = ScaleToHighDPI(1.3f);

            // Carrega estilo de fontes
            stateFont = Content.Load<SpriteFont>("GameState");
            scoreFont = Content.Load<SpriteFont>("Score");
        }

        // Metodo de descarga de elementos externos
        protected override void UnloadContent()
        {
        }

        // Metodo de atualização do estatus dos elementos
        protected override void Update(GameTime gameTime)
        {
            if (loopControl == 50)
            {
                loopControl = 0;
            }
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler();
            if (loopControl == 0)
            {
                spawnNewObstacle();
            }
            frooger.Update(elapsedTime);
            UpdateAllObstacles(elapsedTime);
            VerifyObstacles();
            loopControl++;
            base.Update(gameTime);
        }

        // Metodo de desenho dos elementos graficos
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Inicializa o ambente de operações de desenho na tela
            spriteBatch.Begin();

            // Desenha o background carregado
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);

            // Desenha o sapo e os obstaculos
            
            frooger.Draw(spriteBatch);
            DrawAllObstacles(spriteBatch);

            // Se o jogo ainda não começou fica em tela de inicio
            if (!gameStarted)
            {
                // Carrega tela preta no inicio de jogo aguardando um espaço para iniciar
                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0,
                (int)screenWidth, (int)screenHeight), Color.White);
                String title = "FROGGER - THE MEDIEVAL EDITION";
                String pressSpace = "Press Space to start";
                // Insere temanho de fonte para escrita
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);
                // Escreve centralizado
                spriteBatch.DrawString(stateFont, title,new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3),
                Color.ForestGreen);
                spriteBatch.DrawString(stateFont, pressSpace,new Vector2(screenWidth / 2 - pressSpaceSize.X / 2,
                screenHeight / 2), Color.White);
            }
            else
            {
                // Desenha a pontuação
                spriteBatch.DrawString(scoreFont, "Score: ",new Vector2((float)(screenWidth * 0.82), (float)(screenHeight * 0.046)), Color.Black);
                spriteBatch.DrawString(scoreFont, score.ToString(),new Vector2((float)(screenWidth * 0.9), (float)(screenHeight * 0.046)), Color.Black);

                // Desenha a quantidade de vidas
                spriteBatch.DrawString(scoreFont, "Lives: ", new Vector2((float)(screenWidth * 0.62), (float)(screenHeight * 0.046)), Color.Black);
                spriteBatch.DrawString(scoreFont, lives.ToString(), new Vector2((float)(screenWidth * 0.7), (float)(screenHeight * 0.046)), Color.Black);

                // Desenha o timer
                spriteBatch.DrawString(scoreFont, "Timer: ", new Vector2((float)(screenWidth * 0.036), (float)(screenHeight * 0.046)), Color.Black);

                // Se game over
                if (gameOver)
                {
                    // Desenha gameover na tela de forma centralizada
                    spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, screenHeight / 4 - gameOverTexture.Width / 2), Color.White);
                    String pressEnter = "Press Enter to restart!";
                    // Determina tamanho de fonte para reinicio
                    Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);
                    // Desenha centralizado horizontalmente a escrita
                    spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White);
                }
            }

            spriteBatch.End();
            // Encerra o ambente de operações de desenho na tela

            base.Draw(gameTime);
        }

        // Metodo de identificação e escalonamento conforme dpis da tela utilizada
        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
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
                    StartGame();
                    gameStarted = true;
                    gameOver = false;
                }
                return;
            }
            // Reinicia se for precionado enter após game over
            if (gameOver && state.IsKeyDown(Keys.Enter))
            {
                StartGame();
                gameStarted = true;
                gameOver = false;
            }

            // Controla teclas de direção com controle de area da tela a ser usada
            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                frooger.angle = 0;
                if (frooger.y > (screenHeight / 5))
                {
                    frooger.y = frooger.y - 6;
                }
            }
            else if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                frooger.angle = 600;
                if (frooger.y < (screenHeight - (screenHeight / 8)))
                {
                    frooger.y = frooger.y + 6;
                }
            }
            else if (state.IsKeyDown(Keys.Left) || (state.IsKeyDown(Keys.A)))
            {
                frooger.angle = 300;
                if (frooger.x > screenWidth/20)
                {
                    frooger.x = frooger.x - 6;
                }
            }
            else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                frooger.angle = 900;
                if (frooger.x < (screenWidth - (screenWidth / 20)))
                {
                    frooger.x = frooger.x + 6;
                }
            }
        }

        // Metodo de inicio do jogo
        public void StartGame()
        {
            streetOneH = (float)(screenHeight - screenHeight / 4.5);
            streetTwoH = (float)(screenHeight - screenHeight / 3.05);
            streetTreeH = (float)(screenHeight - screenHeight / 2.475);
            streetFourH = (float)(screenHeight - screenHeight / 1.975);
            streetFiveH = (float)(screenHeight - screenHeight / 1.725);
            streetSixH = (float)(screenHeight - screenHeight / 1.465);
            streeLeftLimit = -screenWidth / 17;
            streeRigthLimit = screenWidth + screenWidth / 17;
            screen.Add(streetOneH);
            screen.Add(streetTwoH);
            screen.Add(streetTreeH);
            screen.Add(streetFourH);
            screen.Add(streetFiveH);
            screen.Add(streetSixH);
            frooger.x = screenWidth / 2;
            frooger.y = screenHeight -(screenHeight / 8);
            // Inicializa as variaveis de jogo
            lives = 5;
            score = 0;
            loopControl = 0;
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
                    cart = new Obstacles(GraphicsDevice, "Content/red_cart.png", scale);
                    break;
                case 2:
                    cart = new Obstacles(GraphicsDevice, "Content/green_cart.png", scale);
                    break;
                default:
                    cart = new Obstacles(GraphicsDevice, "Content/purple_cart.png", scale);
                    break;
            }
            int streeat = random.Next(1, 7);

            //Seleciona aleatoriamente uma rua para a coarroça
            // Cuida pra não por duas carroças seguidas na mesma rua
            if (lines.Count != 0)
            {
                while (lines[lines.Count -1] == streeat)
                {
                    streeat = random.Next(1, 7);
                }
            }

            switch (streeat)
            {
                case 1:
                    cart.x = streeLeftLimit;
                    cart.y = streetOneH;
                    cart.dX = (float)(aceleretionToRigth * 0.75);
                    cart.angle = angleToRight;
                    break;
                case 2:
                    cart.x = streeRigthLimit;
                    cart.y = streetTwoH;
                    cart.dX = acelerationToLeft * 1;
                    cart.angle = angleToLeft;
                    break;
                case 3:
                    cart.x = streeLeftLimit;
                    cart.y = streetTreeH;
                    cart.dX = (float)(aceleretionToRigth * 1.25);
                    cart.angle = angleToRight;
                    break;
                case 4:
                    cart.x = streeRigthLimit;
                    cart.y = streetFourH;
                    cart.dX = (float)(acelerationToLeft * 1.5);
                    cart.angle = angleToLeft;
                    break;
                case 5:
                    cart.x = streeLeftLimit;
                    cart.y = streetFiveH;
                    cart.dX = (float)(aceleretionToRigth * 1.75);
                    cart.angle = angleToRight;
                    break;
                default:
                    cart.x = streeRigthLimit;
                    cart.y = streetSixH;
                    cart.dX = acelerationToLeft * 2;
                    cart.angle = angleToLeft;
                    break;
            }
            if (screen.Contains(cart.y))
            {
                lines.Add(streeat);
                obstacles.Add(cart);
            }
        }

        public void UpdateAllObstacles(float elapsedTime)
        {
            for (int i = 0 ; i < obstacles.Count ; i++)
            {
                obstacles[i].Update(elapsedTime);
            }
        }

        public void DrawAllObstacles(SpriteBatch sprite)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                obstacles[i].Draw(sprite);
            }
        }

        public void VerifyObstacles()
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                if ((obstacles[i].x < streeLeftLimit) || (obstacles[i].x > streeRigthLimit))
                {
                    obstacles.RemoveAt(i);
                }
            }
        }
    }
}
