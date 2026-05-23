using Microsoft.Xna.Framework.Graphics;

namespace SuperTanks.Systems
{
    internal class Animation
    {
        private Texture2D[] images;
        private int currentImage;
        private int imagesSize;
        private int counter;
        private int delay = 5;
        internal int Total { get; set; }

        public Animation (Texture2D[] images)
        {
            this.images = images;
            imagesSize = images.Length;
        }

        public void Update()
        {
            if (delay <= 0) return;

            counter++;

            if (counter == delay)
            {
                currentImage++;
                counter = 0;
            }
            if (currentImage == imagesSize)
            {
                currentImage = 0;
                Total++;
            }
        }

        public Texture2D getImage() { return images[currentImage]; }
    }
}
