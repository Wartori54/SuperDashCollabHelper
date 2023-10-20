using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;

namespace Celeste.Mod.SuperDashCollabHelper.Entities;

[CustomEntity(
    SuperDashCollabHelperModule.NAME + "/MomentumBouncerHorizontal = LoadHorizontal",
    SuperDashCollabHelperModule.NAME + "/MomentumBouncerVertical = LoadVertical")]
public class MomentumBouncer : Entity {
    private static readonly int Thickness = 8;

    public static Entity LoadHorizontal(Level level, LevelData data, Vector2 offset, EntityData entityData) {
        bool flipped = entityData.Bool("flipped");
        Directions dir = (Directions)(VerticalDirection * (flipped ? -1 : 1));

        entityData.Values["directionI"] = dir;
        return new MomentumBouncer(entityData, offset);
    }
    
    public static Entity LoadVertical(Level level, LevelData data, Vector2 offset, EntityData entityData) {
        bool flipped = entityData.Bool("flipped");
        Directions dir = (Directions)(HorizontalDirection * (flipped ? -1 : 1));
        entityData.Values["directionI"] = dir;
        return new MomentumBouncer(entityData, offset);
    }
    
    private readonly Directions direction;
    private readonly string texOverride;
    private readonly float minSpeed;
    private readonly PlayerCollider pc;
    private readonly int lenght;

    public MomentumBouncer(EntityData data, Vector2 offset) 
        : this(data.Position+offset, data.Enum("directionI", Directions.Right), 
            data.Width, data.Height,data.Attr("texture", "default00"), 
            data.Float("minSpeed", 90)) {
    }

    
    // TODO: sound
    public MomentumBouncer(Vector2 position, Directions direction, int width, int height, string texOverride, float minSpeed) : base(position) {
        this.direction = direction;
        this.texOverride = texOverride;
        this.minSpeed = minSpeed;
        lenght = Math.Abs((int)this.direction) == HorizontalDirection ? height : width;

        // Add(GFX.SpriteBank.Create("momentumBouncer"));
        Collider = GetHitbox(direction, lenght);
        Add(pc = new PlayerCollider(OnCollide));
    }

    public override void Awake(Scene scene) {
        MTexture mTexture = GFX.Game["objects/SuperDashCollabHelper/momentumBouncer/" + texOverride];
        // textures may have multiple middle tile sprites
        int num = mTexture.Width / 8;
        int tiles = lenght / 8;
        int yTilePosition = direction == Directions.Down || direction == Directions.Left ? 8 : 0;
        // remember: horizontal bouncing direction -> vertical sprite
        float rotation = Math.Abs((int)direction) == HorizontalDirection ? (float) Math.PI / 2 : 0;
        for (int i = 0; i < tiles; i++) {
            int targetTile;
            if (i == 0) {
                targetTile = 0;
            } else if (i == tiles - 1) {
                targetTile = num-1;
            } else {
                // 1-indexed and we skip the last one too
                targetTile = Calc.Random.Next(num - 2) + 1;
            }

            Image image = new Image(mTexture.GetSubtexture(targetTile * 8, yTilePosition, 8, 8));
            // rotation == 0 -> VerticalDirection
            if (rotation != 0) {
                image.Y = i * 8;
                image.X = 8;
            } else {
                image.X = i * 8;
            }

            image.Rotation = rotation;
            Add(image);
        }

        base.Awake(scene);
    }

    // You can cancel a bounce if you hit a wall behind the entity and wall kick of it, but that's so funny im keeping that
    public void OnCollide(Player player) {
        DynamicData playerData = new DynamicData(player);
        float playerRetainedSpeed = playerData.Get<float>("wallSpeedRetained");
        if (Math.Abs((int)direction) == HorizontalDirection) {
            DoOnCollide(ref player.Speed.X, playerRetainedSpeed);
        } else {
            DoOnCollide(ref player.Speed.Y, 0);
        }

        void DoOnCollide(ref float playerSpeedAxis, float retained) {
            if (playerSpeedAxis * Math.Sign((int)direction) <= 0) {
                playerSpeedAxis *= -1;
                if (Math.Abs(retained) > Math.Abs(playerSpeedAxis))
                    playerSpeedAxis = -retained;
                // edge case: we started dashing while being in the entity, so dont cancel it this time
                if (playerSpeedAxis != 0)
                    player.StateMachine.State = Player.StNormal;
                if (Math.Abs(playerSpeedAxis) < minSpeed) {
                    int dir = Math.Sign(playerSpeedAxis);
                    if (dir == 0)
                        dir = Math.Sign((int)direction);
                    playerSpeedAxis = minSpeed * dir;
                }
            }
        }
    }

    private static Hitbox GetHitbox(Directions dir, int lenght) {
        switch (dir) {
            case Directions.Down:
            case Directions.Up:
                return new Hitbox(lenght, Thickness);
            case Directions.Left:
            case Directions.Right:
            default:
                return new Hitbox(Thickness, lenght);
        }
    }
    
    public enum Directions {
      Up = -1,
      Down = 1,
      Left = -2,
      Right = 2,
    }

    private const int HorizontalDirection = 2;
    private const int VerticalDirection = 1;
}