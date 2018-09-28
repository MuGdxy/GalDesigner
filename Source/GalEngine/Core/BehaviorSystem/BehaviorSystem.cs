using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class BehaviorSystem
    {
        private readonly GameScene gameScene;
        private RequireComponents requireComponents;

        public BehaviorSystem(GameScene gameScene)
        {
            this.gameScene = gameScene;

            this.requireComponents = new RequireComponents();
        }

        public abstract void Excute();

        public GameScene GameScene { get => gameScene; }
        public RequireComponents RequireComponents { get => requireComponents; }
    }
}
