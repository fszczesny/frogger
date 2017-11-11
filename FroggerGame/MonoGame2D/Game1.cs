using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        Texture2D background;
        bool gameStarted;
        bool gameOver;
        int score;
        int lives;
        SpriteFont stateFont;
        SpriteFont scoreFont;
        Texture2D startGameSplash;
        Texture2D gameOverTexture;
        Player frooger;


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
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler();
            frooger.Update(elapsedTime);
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

             // Desenha o sapo
            frooger.Draw(spriteBatch);

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
                spriteBatch.DrawString(scoreFont, "Score: ",new Vector2(screenWidth - 350, 50), Color.Black);
                spriteBatch.DrawString(scoreFont, score.ToString(),new Vector2(screenWidth - 175, 50), Color.Black);

                // Desenha a quantidade de vidas
                spriteBatch.DrawString(scoreFont, "Lives: ", new Vector2(screenWidth - 725, 50), Color.Black);
                spriteBatch.DrawString(scoreFont, lives.ToString(), new Vector2(screenWidth - 575, 50), Color.Black);

                // Desenha o timer
                spriteBatch.DrawString(scoreFont, "Timer: ", new Vector2(screenWidth - 1850, 50), Color.Black);

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
            
            // Controla teclas de direção
            if (state.IsKeyDown(Keys.Left))
            {

            }
            else if (state.IsKeyDown(Keys.Right))
            {

            }
            else if (state.IsKeyDown(Keys.Up))
            {

            }
            else if (state.IsKeyDown(Keys.Down))
            {

            }
        }

        // Metodo de inicio do jogo
        public void StartGame()
        {
            frooger.x = screenWidth / 2;
            frooger.y = screenHeight -(screenHeight / 8);
            // Inicializa as variaveis de jogo
            lives = 5;
            score = 0;
        }
    }
}
