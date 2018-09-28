using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace GalEngine.Extension
{
    public class ImageRenderSystem : BehaviorSystem
    {
        private void RenderImage(GameObject gameObject, Matrix3x2 baseMatrix)
        {
            if (RequireComponents.IsPass(gameObject) is true)
            {
                var transform = gameObject.GetComponent<Transform>();
                var imageShape = gameObject.GetComponent<ImageShape>();
                var matrix = baseMatrix * transform.Matrix;

                if (imageShape.Visible is false) return;

                Systems.Graphics.SetTransform(matrix);

                if (GameResource.IsBitmapExist(imageShape.ImageName) is true)
                {
                    var image = GameResource.GetBitmap(imageShape.ImageName);

                    Systems.Graphics.DrawBitmap(image, new RectangleF(0, 0, imageShape.Size.Width, imageShape.Size.Height),
                        imageShape.Opacity);
                }
            }

            var nextBaseMatrix = Matrix3x2.Identity;

            if (gameObject.IsComponentExist<Transform>() is true)
                nextBaseMatrix = baseMatrix * gameObject.GetComponent<Transform>().Matrix;
            else nextBaseMatrix = baseMatrix;

            foreach (var child in gameObject.Children)
            {
                RenderImage(child, nextBaseMatrix);
            }
        }

        public ImageRenderSystem(GameScene gameScene) : base(gameScene)
        {
            RequireComponents.SetRequireComponentType<Transform>();
            RequireComponents.SetRequireComponentType<ImageShape>();
        }

        public override void Excute()
        {
            RenderImage(GameScene.Root, Matrix3x2.Identity);
        }
    }
}
