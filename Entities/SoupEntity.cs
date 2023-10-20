using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.SuperDashCollabHelper.Entities {
    [CustomEntity(SuperDashCollabHelperModule.NAME + "/SoupEntity")]
    public class SoupEntity : Entity {
        public SoupEntity(EntityData data, Vector2 offset)
            : base(data.Position + offset) {
            Add(GFX.SpriteBank.Create("soupEntity"));
            Collider = new Hitbox(32, 32, -16, -16);
        }

        public override void Update() {
            base.Update();
        }
    }
} 