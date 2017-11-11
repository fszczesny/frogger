using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    class Obstacles
    {
        // Imagem do sprite
        public Texture2D texture
        {
            get;
        }

        // Coordenadas x centrais do obstatulo
        public float x
        {
            get;
            set;
        }

        // Coordenadas y centrais do obstaculo
        public float y
        {
            get;
            set;
        }

        // Angulo central do obstaculo
        public float angle
        {
            get;
            set;
        }

        // Acelearção em x do obstaculo
        public float dX
        {
            get;
            set;
        }

        // Aceleração em y do obstaculo
        public float dY
        {
            get;
            set;
        }

        // Escala do sprite
        public float scale
        {
            get;
            set;
        }

        // Construtor
        public Obstacles(GraphicsDevice graphicsDevice, string textureName, float scale)
        {
            this.scale = scale;
            var stream = TitleContainer.OpenStream(textureName);
            texture = Texture2D.FromStream(graphicsDevice, stream);
        }

        // Atualiza os dados do obstaculo
        public void Update(float elapsedTime)
        {

        }

        // Desenha o obstaculo
        public void Draw(SpriteBatch spriteBatch)
        {
            // Determina a posição do obstaculo
            Vector2 spritePosition = new Vector2(this.x, this.y);
            // Desenha o obstaculo
            spriteBatch.Draw(texture, spritePosition, null, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }
    }
}