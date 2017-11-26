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
        private const float HITBOXSCALE = 0.75f;
        
        private Texture2D texture;
        private float x;
        private float y;
        private float angle;
        private float dX;
        private float dY;
        private float scale;
        private int streat;

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
        public void setDx(float dx)
        {
            this.dX = dx;
        }
        public void setDy(float dy)
        {
            this.dY = dy;
        }
        public void setStreat(int s)
        {
            this.streat = s;
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
        public float getDx()
        {
            return this.dX;
        }
        public float getDy()
        {
            return this.dY;
        }
        public int getStreat()
        {
            return this.streat;
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
            this.x = this.x + this.dX * 3;
        }

        // Desenha o obstaculo
        public void Draw(SpriteBatch spriteBatch)
        {
            // Determina a posição do obstaculo
            Vector2 spritePosition = new Vector2(this.x, this.y);
            // Desenha o obstaculo
            spriteBatch.Draw(texture, spritePosition, null, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        // Verifica colisão do obstaculo com um determido obstaculo
        private bool verifyColisionWithSpecificObstacle(Obstacles obstaclesSprite)
        {
            if (this.x + this.texture.Width * this.scale * HITBOXSCALE / 2 < obstaclesSprite.x - obstaclesSprite.texture.Width * obstaclesSprite.scale / 2)
            {
                return false;
            }
            if (this.y + this.texture.Height * this.scale * HITBOXSCALE / 2 < obstaclesSprite.y - obstaclesSprite.texture.Height * obstaclesSprite.scale / 2)
            {
                return false;
            }
            if (this.x - this.texture.Width * this.scale * HITBOXSCALE / 2 > obstaclesSprite.x + obstaclesSprite.texture.Width * obstaclesSprite.scale / 2)
            {
                return false;
            }
            if (this.y - this.texture.Height * this.scale * HITBOXSCALE / 2 > obstaclesSprite.y + obstaclesSprite.texture.Height * obstaclesSprite.scale / 2)
            {
                return false;
            }
            return true;
        }

        // Verifica colisão do obstaculo com algum obstaculo presente na lista de obstaculos passada
        public bool verifyColisionObsttacleWithObstacles(List<Obstacles> obstaclesList, int postitionToIgnore)
        {
            int i;
            for (i = 0; i < obstaclesList.Count; i++)
            {
                if (i != postitionToIgnore)
                {
                    if (this.verifyColisionWithSpecificObstacle(obstaclesList.ElementAt(i)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Destrutor
        ~Obstacles()
        {

        }
    }
}