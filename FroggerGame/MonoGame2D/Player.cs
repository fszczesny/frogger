using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    public class Player
    {
        private const float HITBOXSCALE = 0.25f;

        private Texture2D texture;
        private float x;
        private float y;
        private float angle;
        private float scale;

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void setX(float X)
        {
            this.x = X;
        }

        public void setY(float Y)
        {
            this.y = Y;
        }

        public void setAngle(float a)
        {
            this.angle = a;
        }
        public void setScale(float scale)
        {
            this.scale = scale;
        }

        public Texture2D getTexture()
        {
            return this.texture;
        }

        public float getX()
        {
            return this.x;
        }
        public float getY()
        {
            return this.y;
        }
        public float getAngle()
        {
            return this.angle;
        }
        public float getScale()
        {
            return this.scale;
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

        // Verifica colisão do player com algum obstaculo presente na lista de obstaculos passada
        public bool verifyColisionWithObstacles(List<Obstacles> obstaclesList)
        {
            int i;
            for (i=0; i < obstaclesList.Count; i++)
            {
                if (this.verifyColisionWithSpecificObstacle(obstaclesList.ElementAt(i)))
                {
                    return true;
                }
            }
            return false;
        }

        // Verifica colisão do player com um determido obstaculo
        private bool verifyColisionWithSpecificObstacle(Obstacles obstaclesSprite)
        {
            if (this.x + this.texture.Width * this.scale * HITBOXSCALE / 2 < obstaclesSprite.getX() - obstaclesSprite.getTexture().Width * obstaclesSprite.getScale() / 2)
            {
                return false;
            }
            if (this.y + this.texture.Height * this.scale * HITBOXSCALE / 2 < obstaclesSprite.getY() - obstaclesSprite.getTexture().Height * obstaclesSprite.getScale() / 2)
            {
                return false;
            }
            if (this.x - this.texture.Width * this.scale * HITBOXSCALE / 2 > obstaclesSprite.getX() + obstaclesSprite.getTexture().Width * obstaclesSprite.getScale() / 2)
            {
                return false;
            }
            if (this.y - this.texture.Height * this.scale * HITBOXSCALE / 2 > obstaclesSprite.getY() + obstaclesSprite.getTexture().Height * obstaclesSprite.getScale() / 2)
            {
                return false;
            }
            return true;
        }
    }
}