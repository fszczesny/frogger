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


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Metodo de inicialização
        protected override void Initialize()
        {
            base.Initialize();

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
        }

        // Metodo de descarga de elementos externos
        protected override void UnloadContent()
        {
        }

        // Metodo de atualização do estatus dos elementos
        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
    }
}
