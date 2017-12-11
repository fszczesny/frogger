using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    public class Obstacles
    {      
        // Variaveis de controle
        private Texture2D texture;
        private float x;
        private float y;
        private float angle;
        private float dX;
        private float dY;
        private float scale;
        private int streat;

        // Getters e setters
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
            this.x = this.x + this.dX * Constants.obstacleAcelerationPar;
        }

        // Desenha o obstaculo
        public void Draw(SpriteBatch spriteBatch)
        {
            // Determina a posição do obstaculo
            Vector2 spritePosition = new Vector2(this.x, this.y);
            // Desenha o obstaculo
            if (this.angle == Constants.zero)
            {
                spriteBatch.Draw(texture, spritePosition, null, Color.White, 0, new Vector2(texture.Width / Constants.two, texture.Height / Constants.two), new Vector2(scale, scale), SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(texture, spritePosition, null, Color.White, 0, new Vector2(texture.Width / Constants.two, texture.Height / Constants.two), new Vector2(scale, scale), SpriteEffects.FlipHorizontally, 0f);

            }
        }

        // Verifica colisão do obstaculo com um determido obstaculo
        private bool verifyColisionWithSpecificObstacle(Obstacles obstaclesSprite)
        {
            if (this.x + this.texture.Width * this.scale * Constants.HitBoxObstacle / Constants.two < obstaclesSprite.x - obstaclesSprite.texture.Width * obstaclesSprite.scale / Constants.two)
            {
                return false;
            }
            if (this.y + this.texture.Height * this.scale * Constants.HitBoxObstacle / Constants.two < obstaclesSprite.y - obstaclesSprite.texture.Height * obstaclesSprite.scale / Constants.two)
            {
                return false;
            }
            if (this.x - this.texture.Width * this.scale * Constants.HitBoxObstacle / Constants.two > obstaclesSprite.x + obstaclesSprite.texture.Width * obstaclesSprite.scale / Constants.two)
            {
                return false;
            }
            if (this.y - this.texture.Height * this.scale * Constants.HitBoxObstacle / Constants.two > obstaclesSprite.y + obstaclesSprite.texture.Height * obstaclesSprite.scale / Constants.two)
            {
                return false;
            }
            return true;
        }

        // Verifica colisão do obstaculo com algum obstaculo presente na lista de obstaculos passada
        public bool verifyColisionObsttacleWithObstacles(List<Obstacles> obstaclesList, int postitionToIgnore)
        {
            int i;
            for (i = Constants.zero; i < obstaclesList.Count; i++)
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