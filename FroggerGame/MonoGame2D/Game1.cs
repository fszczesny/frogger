﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace MonoGame2D
{
    // Medieval Frooger Version 1.0 - Versão desiginada a etapas 1 e 2 de definição do trabalho
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
        // Variaveis de posicionamento e limitação do ruas
        float screenWidth;
        float screenHeight;
        List<float> validLines = new List<float>();
        // Declaração do objeto Player que representa o frogger
        Player frooger;
        // Declaração da lista de obstaculos e sua lista auxiliar de frequencia
        List<Obstacles> obstacles = new List<Obstacles>();      
        // Declara objeto de controle de troca de rua
        ControlChangeStreeat controlChangeStreeat = new ControlChangeStreeat();
        ControlNewObstacles controlNewObstacles = new ControlNewObstacles();
        ControlGameParameters controlParameters = new ControlGameParameters();
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
            startScreenConfigs();
        }

        // Metodo de carga de elementos externos
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadTextureAndFontStyles();
            // Carrega sprite do player
            frooger = new Player(GraphicsDevice, Constants.froggerSprite, ScaleToHighDPI(0.3f));
            controlNewObstacles.setScale(ScaleToHighDPI(1.3f));
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
            controlNewObstacles.verifyIfNeedMoreObstacles(GraphicsDevice, obstacles, validLines, controlParameters.getAcelerationToR(), controlParameters.getAcelerationToL());
            controlChangeStreeat.verifyChangeStreatObstacles(obstacles, validLines, controlParameters.getAcelerationToR(), controlParameters.getAcelerationToL());
            controlParameters.colisionsControl(elapsedTime, frooger, obstacles, bloodTexture, screenHeight);
            controlNewObstacles.UpdateAllObstacles(obstacles, elapsedTime);
            controlNewObstacles.VerifyIfObstaclesIsOutOfScreen(obstacles);
            base.Update(gameTime);
        }

        // Metodo de desenho dos elementos graficos
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // Desenha objetos
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), Color.White);
            if (controlParameters.getPause() <= 0)
            {
                frooger.Draw(spriteBatch);
            }
            controlNewObstacles.DrawAllObstacles(obstacles, spriteBatch);
            // Se o jogo ainda não começou fica em tela de inicio
            if (!controlParameters.getGameStarted())
            {
                showBeforeStartScreen(spriteBatch);
            }
            else
            {
                drawInterfaceOfPontuation();
                // Se ganhou
                if (controlParameters.getWin())
                {
                    // Verifica se atingiu nivel maximo
                    if (controlParameters.getLevel() == Constants.maxLevel)
                    {
                        drawStateScreen(Constants.winAllLevelsMessage, winTexture);
                    }
                    else
                    {
                        drawStateScreen(Constants.progressLevelMessage, winTexture);
                    }
                }
                // Se game over
                if (controlParameters.getGameOver())
                {
                    drawStateScreen(Constants.restartMessage, gameOverTexture);
                }
                // Se morreu, mas tem vida ainda
                else if (controlParameters.getDead())
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
            controlParameters.setPause(Constants.ignoreKyboardLoop);
            controlParameters.setLevel(controlParameters.getLevel() + 1);
            frooger.setTexture(froggerTexture);
            frooger.setX(screenWidth / 2);
            frooger.setY(screenHeight - (screenHeight / 8));
            frooger.setAngle(Constants.angleFrogger0);
            verifyLevel(controlParameters.getLevel());
            obstacles.Clear();
            controlNewObstacles.setLoopControl(0);
            controlChangeStreeat.setLoopControl(0);
        }

        // Seta parametros para uso da proxima vida
        public void StartNextLive(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead)
        {
            frooger.setAngle(Constants.angleFrogger0);
            frooger.setTexture(froggerTexture);
            frooger.setX(screenWidth / 2);
            frooger.setY(screenHeight - (screenHeight / 8));

            controlParameters.setGameStarted(theGameStarted);
            controlParameters.setGameOver(theGameOver);
            controlParameters.setWin(thePlayerWined);
            controlParameters.setDead(thePlayerDead);
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
            if (!controlParameters.getGameStarted())
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    startParametersWhithKeyboard(true, false, false, false);
                }
                return;
            }
            // Reinicia se for precionado enter após game over
            if (controlParameters.getGameOver() && state.IsKeyDown(Keys.Enter))
            {
                startParametrs(controlParameters.getGameOver(), controlParameters.getWin(), controlParameters.getDead(), controlParameters.getGameStarted(), Constants.initialLives, Constants.initialScore, Constants.initialLevel, Constants.intervalBetwenNewObstacleLoop, Constants.intervalBetwenChangeStreat, Constants.initialFroggerPass);
                startParametersWhithKeyboard(true, false, false, false);
            }
            if (controlParameters.getDead() && state.IsKeyDown(Keys.Space) && !controlParameters.getGameOver())
            {
                StartNextLive(true, false, false, false);
            }
            if (controlParameters.getWin() && state.IsKeyDown(Keys.Enter))
            {
                if (controlParameters.getLevel() == Constants.maxLevel)
                {
                    controlParameters.setLevel(0);
                }
                startParametersWhithKeyboard(true, false, false, false);
            }
            if (controlParameters.getPause() <= 0 && !controlParameters.getWin() && !controlParameters.getGameOver() && !controlParameters.getDead())
            {

                // Controla teclas de direção com controle de area da tela a ser usada
                if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                {
                    frooger.setAngle(Constants.angleFrogger0);
                    if (frooger.getY() > (screenHeight / 5))
                    {
                        frooger.setY(frooger.getY() - controlParameters.getPass());
                    }
                }
                else if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                {
                    frooger.setAngle(Constants.angleFrogger180);
                    if (frooger.getY() < (screenHeight - (screenHeight / 8)))
                    {
                        frooger.setY(frooger.getY() + controlParameters.getPass());
                    }
                }
                else if (state.IsKeyDown(Keys.Left) || (state.IsKeyDown(Keys.A)))
                {
                    frooger.setAngle(Constants.angleFrogger90);
                    if (frooger.getX() > screenWidth / 20)
                    {
                        frooger.setX(frooger.getX() - controlParameters.getPass());
                    }
                }
                else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                {
                    frooger.setAngle(Constants.angleFrogger270);
                    if (frooger.getX() < (screenWidth - (screenWidth / 20)))
                    {
                        frooger.setX(frooger.getX() + controlParameters.getPass());
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

        // Iniciaza parametros de jogo
        public void startParametrs(bool isGameOver, bool isWin, bool isDead, bool theGameStart, int nunberOfLives, int initialScore, int initialLevel, int initialObstacleFrequency, int changeStreatFrequency, int initialFroggerPass)
        {
            controlParameters.startParametrs(isGameOver, isWin, isDead, theGameStart, nunberOfLives, initialScore, initialLevel);
            controlNewObstacles.setLoopIncrease(initialObstacleFrequency);
            controlChangeStreeat.setLoopIncrease(changeStreatFrequency);
            controlParameters.setPass(initialFroggerPass);
        }

        public void startParametersWhithKeyboard(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead)
        {
            StartGame();
            controlParameters.startParametersWhithKeyboard(theGameStarted, theGameOver, thePlayerWined, thePlayerDead);
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
            controlNewObstacles.setStreeLeftLimit(-screenWidth / 17);
            controlNewObstacles.setStreeRigthLimit(screenWidth + screenWidth / 17);
            validLines.Clear();
            validLines.Add((float)(screenHeight - screenHeight / 4.5));
            validLines.Add((float)(screenHeight - screenHeight / 3.05));
            validLines.Add((float)(screenHeight - screenHeight / 2.475));
            validLines.Add((float)(screenHeight - screenHeight / 1.975));
            validLines.Add((float)(screenHeight - screenHeight / 1.725));
            validLines.Add((float)(screenHeight - screenHeight / 1.465));
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
            spriteBatch.DrawString(scoreFont, controlParameters.getscore().ToString(), new Vector2((float)(screenWidth * 0.9), (float)(screenHeight * 0.046)), Color.Black);
            spriteBatch.DrawString(scoreFont, Constants.livesMessage, new Vector2((float)(screenWidth * 0.62), (float)(screenHeight * 0.046)), Color.Black);
            spriteBatch.DrawString(scoreFont, controlParameters.getLives().ToString(), new Vector2((float)(screenWidth * 0.7), (float)(screenHeight * 0.046)), Color.Black);
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
            if (levelAux > 1 && levelAux < (Constants.maxLevel + 1))
            {
                controlNewObstacles.setLoopControl(controlNewObstacles.getLoopIncrease() - Constants.decFrequencyObstacle);
                controlChangeStreeat.setLoopIncrease(controlChangeStreeat.getLoopIncrease() - Constants.decFrequencyChangeStreat);
                controlParameters.startNextLevelParameters(levelAux);
            }
            else
            {
                controlNewObstacles.setLoopIncrease(Constants.intervalBetwenNewObstacleLoop);
                controlChangeStreeat.setLoopIncrease(Constants.intervalBetwenChangeStreat);
                controlParameters.startBeginParameters();
            }
        }
    }
}
