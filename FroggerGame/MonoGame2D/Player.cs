using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    class Player
    {
        public Texture2D texture
        {
            get;
        }

        // coordenada x do centro do player
        public float x
        {
            get;
            set;
        }

        // coordenada y do centro do player
        public float y
        {
            get;
            set;
        }

        // Angulo central do player
        public float angle
        {
            get;
            set;
        }

        // Escala do sprite do player
        public float scale
        {
            get;
            set;
        }

        // Construtor da classe
        public Player(GraphicsDevice graphicsDevice, string textureName, float scale)
        {
            this.scale = scale;
            var stream = TitleContainer.OpenStream(textureName);
            texture = Texture2D.FromStream(graphicsDevice, stream);
        }

        // Função de atualização do estado do player (Até então não utilizada)
        public void Update(float elapsedTime)
        {

        }

        // Desenha o player
        public void Draw(SpriteBatch spriteBatch)
        {
            // Determina posição do player
            Vector2 spritePosition = new Vector2(this.x, this.y);
            // Desenha o sprite
            spriteBatch.Draw(texture, spritePosition, null, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }
    }
}