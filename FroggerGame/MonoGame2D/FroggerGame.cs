﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace MonoGame2D
{
    // Medieval Frooger Version 1.2 - Versão desiginada a etapas 1 e 2 e 3 de definição do trabalho
    public class FroggerGame : Game
    {
        // Declaração de variaveis globias dentre a classe
        // Variaveis de ambiente grafico
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont stateFont;
        SpriteFont menorFont;
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
        // Declaração do objeto Player que representa o frogger
        Player frooger;
        // Declaração da lista de obstaculos e sua lista auxiliar de frequencia
        List<Obstacles> obstacles = new List<Obstacles>();      
        // Declara objeto de controle de troca de rua, de controle de novos obstaculos, controle de parametros e historico
        ControlChangeStreeat controlChangeStreeat = new ControlChangeStreeat();
        ControlNewObstacles controlNewObstacles = new ControlNewObstacles();
        ControlGameParameters controlParameters = new ControlGameParameters();
        HistoricDocControl controlHistoric = new HistoricDocControl();
        PlayerDatas playerDatas = new PlayerDatas();
        List<PlayerDatas> players = new List<PlayerDatas>();

        string name;
        // Fim da declaração de globais da classe

        public FroggerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Constants.directory;
        }

        // Metodo de inicialização
        protected override void Initialize()
        {
            base.Initialize();
            startParametrs(false, false, false, false, Constants.initialLives, Constants.initialScore, Constants.initialLevel, Constants.intervalBetwenNewObstacleLoop, Constants.intervalBetwenChangeStreat, Constants.initialFroggerPass, false, false);
            startScreenConfigs();
            name = string.Empty;
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
            controlNewObstacles.verifyIfNeedMoreObstacles(GraphicsDevice, obstacles, controlParameters.getValidLines(), controlParameters.getAcelerationToR(), controlParameters.getAcelerationToL());
            controlChangeStreeat.verifyChangeStreatObstacles(obstacles, controlParameters.getValidLines(), controlParameters.getAcelerationToR(), controlParameters.getAcelerationToL());
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
            spriteBatch.Draw(background, new Rectangle(Constants.zero, Constants.zero, (int)screenWidth, (int)screenHeight), Color.White);
            if (controlParameters.getPause() <= Constants.zero)
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
                    if (controlParameters.getGameGetName() && controlParameters.getLevel() == Constants.maxLevel)
                    {
                        if (controlParameters.getShowResults())
                        {
                            drawBests(players);
                        }
                        else
                        {
                            drawMessage(Constants.winAllLevelsMessage, Constants.spacingEnter);
                            drawMessage(Constants.restartMessageTwo, Constants.spacingInset);
                            drawStateScreen(name, winTexture);
                        }
                    }
                    else
                    {
                        drawStateScreen(Constants.progressLevelMessage, winTexture);
                    }
                }
                // Se game over
                if (controlParameters.getGameOver())
                {
                    if (controlParameters.getGameGetName())
                    {
                        if (controlParameters.getShowResults())
                        {
                            drawBests(players);
                        }
                        else
                        {
                            drawMessage(Constants.restartMessage, Constants.spacingEnter);
                            drawMessage(Constants.restartMessageTwo, Constants.spacingInset);
                            drawStateScreen(name, gameOverTexture);
                        }
                    }
                    else
                    {
                        drawStateScreen(Constants.restartMessage, gameOverTexture);
                    }
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
            controlParameters.setLevel(controlParameters.getLevel() + Constants.one);
            frooger.setTexture(froggerTexture);
            frooger.setX(screenWidth / Constants.two);
            frooger.setY(screenHeight - (screenHeight / Constants.minLimit));
            frooger.setAngle(Constants.angleFrogger0);
            verifyLevel(controlParameters.getLevel());
            obstacles.Clear();
            controlNewObstacles.setLoopControl(Constants.zero);
            controlChangeStreeat.setLoopControl(Constants.zero);
            name = string.Empty;
        }

        // Seta parametros para uso da proxima vida
        public void StartNextLive(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead)
        {
            frooger.setAngle(Constants.angleFrogger0);
            frooger.setTexture(froggerTexture);
            frooger.setX(screenWidth / Constants.two);
            frooger.setY(screenHeight - (screenHeight / Constants.minLimit));

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
                    startParametersWhithKeyboard(true, false, false, false, false, false);
                }
                return;
            }
            // Reinicia se for precionado enter após game over
            if (controlParameters.getGameOver())
            {
                if (state.IsKeyDown(Keys.Enter))
                {
                    controlParameters.setGameName(false);
                    startParametrs(controlParameters.getGameOver(), controlParameters.getWin(), controlParameters.getDead(), controlParameters.getGameStarted(), Constants.initialLives, Constants.initialScore, Constants.initialLevel, Constants.intervalBetwenNewObstacleLoop, Constants.intervalBetwenChangeStreat, Constants.initialFroggerPass, controlParameters.getGameGetName(), controlParameters.getShowResults());
                    startParametersWhithKeyboard(true, false, false, false, false, false);
                }
                else if (state.IsKeyDown(Keys.Insert))
                {
                    playerDatas.setName(name);
                    playerDatas.setPoints(controlParameters.getscore());
                    // Salva no historico
                    players = controlHistoric.getTopXPlayers(Constants.bestNumbers, playerDatas);
                    // Imprime o historico
                    controlParameters.setShowResults(true);
                }
                else
                {
                    makeName(state);
                }
            }
            if (controlParameters.getDead() && state.IsKeyDown(Keys.Space) && !controlParameters.getGameOver())
            {
                StartNextLive(true, false, false, false);
            }
            if (controlParameters.getWin())
            {
                if (controlParameters.getLevel() == Constants.maxLevel)
                {
                    if (state.IsKeyDown(Keys.Enter))
                    {
                        controlParameters.setGameName(false);
                        startParametrs(controlParameters.getGameOver(), controlParameters.getWin(), controlParameters.getDead(), controlParameters.getGameStarted(), Constants.initialLives, Constants.initialScore, Constants.initialLevel, Constants.intervalBetwenNewObstacleLoop, Constants.intervalBetwenChangeStreat, Constants.initialFroggerPass, controlParameters.getGameGetName(), controlParameters.getShowResults());
                        startParametersWhithKeyboard(true, false, false, false, false, false);
                        controlParameters.setLevel(Constants.zero);
                    }
                    else if (state.IsKeyDown(Keys.Insert))
                    {
                        playerDatas.setName(name);
                        playerDatas.setPoints(controlParameters.getscore());
                        // Salva no historico
                        players = controlHistoric.getTopXPlayers(Constants.bestNumbers, playerDatas);
                        // Imprime o historico   
                        controlParameters.setShowResults(true);
                    }
                    else
                    {
                        makeName(state);
                    }
                }
                else
                {
                    if (state.IsKeyDown(Keys.Enter))
                    {
                        startParametersWhithKeyboard(true, false, false, false, false, false);
                    }
                }
            }
            if (controlParameters.getPause() <= Constants.zero && !controlParameters.getWin() && !controlParameters.getGameOver() && !controlParameters.getDead())
            {

                // Controla teclas de direção com controle de area da tela a ser usada
                if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                {
                    frooger.setAngle(Constants.angleFrogger0);
                    if (frooger.getY() > (screenHeight / Constants.beginPosition))
                    {
                        frooger.setY(frooger.getY() - controlParameters.getPass());
                    }
                }
                else if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                {
                    frooger.setAngle(Constants.angleFrogger180);
                    if (frooger.getY() < (screenHeight - (screenHeight / Constants.minLimit)))
                    {
                        frooger.setY(frooger.getY() + controlParameters.getPass());
                    }
                }
                else if (state.IsKeyDown(Keys.Left) || (state.IsKeyDown(Keys.A)))
                {
                    frooger.setAngle(Constants.angleFrogger90);
                    if (frooger.getX() > screenWidth / Constants.maxLimit)
                    {
                        frooger.setX(frooger.getX() - controlParameters.getPass());
                    }
                }
                else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                {
                    frooger.setAngle(Constants.angleFrogger270);
                    if (frooger.getX() < (screenWidth - (screenWidth / Constants.maxLimit)))
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
        public void startParametrs(bool isGameOver, bool isWin, bool isDead, bool theGameStart, int nunberOfLives, int initialScore, int initialLevel, int initialObstacleFrequency, int changeStreatFrequency, int initialFroggerPass, bool game, bool show)
        {
            controlParameters.startParametrs(isGameOver, isWin, isDead, theGameStart, nunberOfLives, initialScore, initialLevel, game , show);
            controlNewObstacles.setLoopIncrease(initialObstacleFrequency);
            controlChangeStreeat.setLoopIncrease(changeStreatFrequency);
            controlParameters.setPass(initialFroggerPass);
        }

        public void startParametersWhithKeyboard(bool theGameStarted, bool theGameOver, bool thePlayerWined, bool thePlayerDead, bool game, bool show)
        {
            StartGame();
            controlParameters.startParametersWhithKeyboard(theGameStarted, theGameOver, thePlayerWined, thePlayerDead, game, show);
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
            menorFont = Content.Load<SpriteFont>(Constants.menorFont);
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
            controlNewObstacles.setStreeLeftLimit(-screenWidth / Constants.leftAndRigthLimit);
            controlNewObstacles.setStreeRigthLimit(screenWidth + screenWidth / Constants.leftAndRigthLimit);
            controlParameters.setValidLines(screenHeight);
        }

        // Carrega tela preta no inicio de jogo aguardando um espaço para iniciar
        // Insere temanho de fonte para escrita
        // Escreve centralizado
        public void showBeforeStartScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(startGameSplash, new Rectangle(Constants.zero, Constants.zero, (int)screenWidth, (int)screenHeight), Color.White);
            String title = Constants.startMessage;
            String pressSpace = Constants.askForASpaceMessage;
            Vector2 titleSize = stateFont.MeasureString(title);
            Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);
            spriteBatch.DrawString(stateFont, title, new Vector2(screenWidth / Constants.two - titleSize.X / Constants.two, screenHeight / Constants.tree), Color.ForestGreen);
            spriteBatch.DrawString(stateFont, pressSpace, new Vector2(screenWidth / Constants.two - pressSpaceSize.X / Constants.two, screenHeight / Constants.two), Color.White);
        }

        // Desenha a pontuação
        // Desenha a quantidade de vidas
        // Desenha o timer
        public void drawInterfaceOfPontuation()
        {
            spriteBatch.DrawString(scoreFont, Constants.scoreMessage, new Vector2((float)(screenWidth * Constants.scoreNameWidth), (float)(screenHeight * Constants.scoreNameHeigth)), Color.Black);
            spriteBatch.DrawString(scoreFont, controlParameters.getscore().ToString(), new Vector2((float)(screenWidth * Constants.scoreValueWidth), (float)(screenHeight * Constants.scoreValueHeigth)), Color.Black);
            spriteBatch.DrawString(scoreFont, Constants.livesMessage, new Vector2((float)(screenWidth * Constants.livesNameWidth), (float)(screenHeight * Constants.livesNameHeigth)), Color.Black);
            spriteBatch.DrawString(scoreFont, controlParameters.getLives().ToString(), new Vector2((float)(screenWidth * Constants.livesValueWidth), (float)(screenHeight * Constants.livesValueHeigth)), Color.Black);
            spriteBatch.DrawString(scoreFont, Constants.timeMessage, new Vector2((float)(screenWidth * Constants.timeNameWidth), (float)(screenHeight * Constants.timeNameHeigth )), Color.Black);
            spriteBatch.DrawString(scoreFont, controlParameters.getLevel().ToString(), new Vector2((float)(screenWidth * Constants.timeValueWidth), (float)(screenHeight * Constants.timeValueHeigth)), Color.Black);
        }

        // Desenha tela e escrita + sprite
        public void drawStateScreen(string pressEnter, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Vector2(screenWidth / Constants.two - texture.Width / Constants.two, screenHeight / Constants.four - texture.Width / Constants.two), Color.White);
            Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);
            spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / Constants.two - pressEnterSize.X / Constants.two, (float)(screenHeight * Constants.messageConst)), Color.White);
        }

        // Desenha tela e escrita
        public void drawMessage(string pressEnter, double value)
        {        
            Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);
            spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / Constants.two - pressEnterSize.X / Constants.two, (float)(screenHeight * value)), Color.White);
        }

        // Desenha tela e escrita
        public void drawMessageHistoric(string pressEnter, float value)
        {
            Vector2 pressEnterSize = menorFont.MeasureString(pressEnter);
            spriteBatch.DrawString(menorFont, pressEnter, new Vector2(screenWidth / Constants.two - pressEnterSize.X / Constants.two, (float)(screenHeight * value)), Color.Black);
        }

        // Desenha na tela todos os historicos
        public void drawBests (List<PlayerDatas> players)
        {
            string toShow;
            float beginPosition = Constants.spacingMenu;
            drawMessageHistoric(Constants.bestMessage, beginPosition);
            beginPosition = (float)(beginPosition + Constants.spacingMenu);
            for (int i = 0; i < players.Count; i++)
            {
                toShow = string.Empty;
                toShow = (i + 1).ToString() + " : " + players[i].getName() + " / " + (players[i].getPoints()).ToString();
                drawMessageHistoric(toShow, beginPosition);
                beginPosition = (float)(beginPosition + Constants.spacingMenu);
            }
            drawMessageHistoric(Constants.pressEnterMessage, beginPosition);
        }

        // Metodo de controle do avanço de nivel
        public void verifyLevel(int levelAux)
        {
            if (levelAux > Constants.one && levelAux < (Constants.maxLevel + Constants.one))
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

        public void makeName(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.A)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'A') && (name[name.Length - 1] != 'a')) { name += "A"; } } else { name += "A"; } }
            if (state.IsKeyDown(Keys.B)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'B') && (name[name.Length - 1] != 'b')) { name += "B"; } } else { name += "B"; } }
            if (state.IsKeyDown(Keys.C)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'C') && (name[name.Length - 1] != 'c')) { name += "C"; } } else { name += "C"; } }
            if (state.IsKeyDown(Keys.D)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'D') && (name[name.Length - 1] != 'd')) { name += "D"; } } else { name += "D"; } }
            if (state.IsKeyDown(Keys.E)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'E') && (name[name.Length - 1] != 'e')) { name += "E"; } } else { name += "E"; } }
            if (state.IsKeyDown(Keys.F)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'F') && (name[name.Length - 1] != 'f')) { name += "F"; } } else { name += "F"; } }
            if (state.IsKeyDown(Keys.G)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'G') && (name[name.Length - 1] != 'g')) { name += "G"; } } else { name += "G"; } }
            if (state.IsKeyDown(Keys.H)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'H') && (name[name.Length - 1] != 'h')) { name += "H"; } } else { name += "H"; } }
            if (state.IsKeyDown(Keys.I)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'I') && (name[name.Length - 1] != 'i')) { name += "I"; } } else { name += "I"; } }
            if (state.IsKeyDown(Keys.J)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'J') && (name[name.Length - 1] != 'j')) { name += "J"; } } else { name += "J"; } }
            if (state.IsKeyDown(Keys.K)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'K') && (name[name.Length - 1] != 'k')) { name += "K"; } } else { name += "K"; } }
            if (state.IsKeyDown(Keys.L)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'L') && (name[name.Length - 1] != 'l')) { name += "L"; } } else { name += "L"; } }
            if (state.IsKeyDown(Keys.M)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'M') && (name[name.Length - 1] != 'm')) { name += "M"; } } else { name += "M"; } }
            if (state.IsKeyDown(Keys.N)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'N') && (name[name.Length - 1] != 'n')) { name += "N"; } } else { name += "N"; } }
            if (state.IsKeyDown(Keys.O)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'O') && (name[name.Length - 1] != 'o')) { name += "O"; } } else { name += "O"; } }
            if (state.IsKeyDown(Keys.P)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'P') && (name[name.Length - 1] != 'p')) { name += "P"; } } else { name += "P"; } }
            if (state.IsKeyDown(Keys.Q)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'Q') && (name[name.Length - 1] != 'q')) { name += "Q"; } } else { name += "Q"; } }
            if (state.IsKeyDown(Keys.R)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'R') && (name[name.Length - 1] != 'r')) { name += "R"; } } else { name += "R"; } }
            if (state.IsKeyDown(Keys.S)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'S') && (name[name.Length - 1] != 's')) { name += "S"; } } else { name += "S"; } }
            if (state.IsKeyDown(Keys.T)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'T') && (name[name.Length - 1] != 't')) { name += "T"; } } else { name += "T"; } }
            if (state.IsKeyDown(Keys.U)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'U') && (name[name.Length - 1] != 'u')) { name += "U"; } } else { name += "U"; } }
            if (state.IsKeyDown(Keys.V)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'V') && (name[name.Length - 1] != 'v')) { name += "V"; } } else { name += "V"; } }
            if (state.IsKeyDown(Keys.X)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'X') && (name[name.Length - 1] != 'x')) { name += "X"; } } else { name += "X"; } }
            if (state.IsKeyDown(Keys.Y)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'Y') && (name[name.Length - 1] != 'y')) { name += "Y"; } } else { name += "Y"; } }
            if (state.IsKeyDown(Keys.W)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'W') && (name[name.Length - 1] != 'w')) { name += "W"; } } else { name += "W"; } }
            if (state.IsKeyDown(Keys.Z)) { if (name.Length > 0) { if ((name[name.Length - 1] != 'Z') && (name[name.Length - 1] != 'z')) { name += "Z"; } } else { name += "Z"; } }
            if (state.IsKeyDown(Keys.NumPad0)) { if (name.Length > 0) { if ((name[name.Length - 1] != '0')) { name += "0"; } } else { name += "0"; } }
            if (state.IsKeyDown(Keys.NumPad1)) { if (name.Length > 0) { if ((name[name.Length - 1] != '1')) { name += "1"; } } else { name += "1"; } }
            if (state.IsKeyDown(Keys.NumPad2)) { if (name.Length > 0) { if ((name[name.Length - 1] != '2')) { name += "2"; } } else { name += "2"; } }
            if (state.IsKeyDown(Keys.NumPad3)) { if (name.Length > 0) { if ((name[name.Length - 1] != '3')) { name += "3"; } } else { name += "3"; } }
            if (state.IsKeyDown(Keys.NumPad4)) { if (name.Length > 0) { if ((name[name.Length - 1] != '4')) { name += "4"; } } else { name += "4"; } }
            if (state.IsKeyDown(Keys.NumPad5)) { if (name.Length > 0) { if ((name[name.Length - 1] != '5')) { name += "5"; } } else { name += "5"; } }
            if (state.IsKeyDown(Keys.NumPad6)) { if (name.Length > 0) { if ((name[name.Length - 1] != '6')) { name += "6"; } } else { name += "6"; } }
            if (state.IsKeyDown(Keys.NumPad7)) { if (name.Length > 0) { if ((name[name.Length - 1] != '7')) { name += "7"; } } else { name += "7"; } }
            if (state.IsKeyDown(Keys.NumPad8)) { if (name.Length > 0) { if ((name[name.Length - 1] != '8')) { name += "8"; } } else { name += "8"; } }
            if (state.IsKeyDown(Keys.NumPad9)) { if (name.Length > 0) { if ((name[name.Length - 1] != '9')) { name += "9"; } } else { name += "9"; } }
            if (state.IsKeyDown(Keys.Add)) { if (name.Length > 0) { if ((name[name.Length - 1] != '+')) { name += "+"; } } else { name += "+"; } }
            if (state.IsKeyDown(Keys.Subtract)) { if (name.Length > 0) { if ((name[name.Length - 1] != '-')) { name += "-"; } } else { name += "-"; } }
            if (state.IsKeyDown(Keys.Space)) { if (name.Length > 0) { if ((name[name.Length - 1] != ' ')) { name += " "; } } else { name += " "; } }
            if (state.IsKeyDown(Keys.Divide)) { if (name.Length > 0) { if ((name[name.Length - 1] != '/')) { name += "/"; } } else { name += "/"; } }
            if (state.IsKeyDown(Keys.Multiply)) { if (name.Length > 0) { if ((name[name.Length - 1] != '*')) { name += "*"; } } else { name += "*"; } }
            if (state.IsKeyDown(Keys.Separator)) { if (name.Length > 0) { if ((name[name.Length - 1] != '_')) { name += "_"; } } else { name += "_"; } }
            if (state.IsKeyDown(Keys.Delete) || state.IsKeyDown(Keys.Back)) { name = string.Empty; }
        }
    }
}
