using System;
using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

using XNAVERGE;
using Microsoft.Xna.Framework.Input;

namespace DemonDoor
{
    class DoorController : ICollidable, IDrawableThing
    {

        DoorSprite sprite;

        public DoorController(World w, Vector2 r, Vector2 size, DoorSprite s)
        {
            _fsBody = w.NewBody();
            _fsBody.Position = r;

            PolygonShape shape = new PolygonShape(1.0f);
            shape.SetAsBox(size.X, size.Y);

            _fsFixture = _fsBody.CreateFixture(shape, this);
            _fsFixture.IsSensor = true;

            _fsFixture.OnCollision += PhysicsCollided;
            _fsFixture.OnSeparation += PhysicsSeparated;
            _fsFixture.OnCollision += BehaviorCollided;

            sprite = s;
        }

        private bool PhysicsCollided(Fixture f1, Fixture f2, Contact contact)
        {
            Fixture self = null, other = null;

            if (f1 == _fsFixture)
            {
                self = f1;
                other = f2;
            }
            else if (f2 == _fsFixture)
            {
                self = f2;
                other = f1;
            }

            if (other.UserData is CivvieController && !_alreadyShot.Contains(other))
            {
                CivvieController c = other.UserData as CivvieController;
                //Console.WriteLine("collided with corpse {0}, kickin' it", c);

                other.Body.ApplyLinearImpulse(Impulse);
                _alreadyShot.Add(other);
            }

            return false;
        }

        private void PhysicsSeparated(Fixture f1, Fixture f2)
        {
            Fixture self = null, other = null;

            if (f1 == _fsFixture)
            {
                self = f1;
                other = f2;
            }
            else if (f2 == _fsFixture)
            {
                self = f2;
                other = f1;
            }

            if (_alreadyShot.Contains(other))
            {
                _alreadyShot.Remove(other);
            }
        }

        private bool BehaviorCollided(Fixture f1, Fixture f2, Contact contact)
        {
            Fixture self = null, other = null;

            if (f1 == _fsFixture)
            {
                self = f1;
                other = f2;
            }
            else if (f2 == _fsFixture)
            {
                self = f2;
                other = f1;
            }

            if (other.UserData is ICollidable)
            {
                this.Collided(other.UserData as ICollidable);
                (other.UserData as ICollidable).Collided(this);
            }

            return false;
        }

        public void Collided(ICollidable other)
        {

        }

        public Vector2 Position
        {
            get
            {
                return _fsBody.Position;
            }
        }

        public int GetX() { return sprite.Sprite.x; }
        public int GetY() { return sprite.Sprite.y; }
        RenderDelegate _myDrawDelegate;
        public RenderDelegate GetDrawDelegate()
        {
            if (_myDrawDelegate != null) return _myDrawDelegate;

            _myDrawDelegate = (int x, int y) =>
            {

                Vector2 screen = Coords.Physics2Screen(new Vector2 { X = Position.X, Y = Position.Y });

                // maybe update the screen here?

                sprite.Sprite.x = (int)screen.X - 19;
                sprite.Sprite.y = (int)screen.Y - 12;
                sprite.Sprite.Draw();
            };

            return _myDrawDelegate;
        }

        public Vector2 Impulse { get; set; }

        private Fixture _fsFixture;
        private Body _fsBody;

        private ISet<Fixture> _alreadyShot = new HashSet<Fixture>();



        private const float MaxGunImpulse = 2000;
        private const float MinGunImpulse = 0;
        private const float GunImpulseKick = 1000;
        private const float GunImpulseDecayTime = 4;

        public float GunImpulse { get; private set; }
        private TimeSpan _gunLastGameTime = TimeSpan.Zero;
        private bool _gunLatch = false;

        public void UpdateGunImpulse(GameTime gameTime)
        {
            // check gun key, kick if newly pressed
            {
                bool revGun = Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.E);

                if (revGun && !_gunLatch)
                    GunImpulse += GunImpulseKick;
            }

            // apply a bit of decay
            {
                float decayPerSecond = MaxGunImpulse / GunImpulseDecayTime;
                GunImpulse -= (float)(gameTime.TotalGameTime - _gunLastGameTime).TotalSeconds * decayPerSecond;
                _gunLastGameTime = gameTime.TotalGameTime;
            }

            // and limit to range
            GunImpulse = Math.Max(MinGunImpulse, GunImpulse);
            GunImpulse = Math.Min(MaxGunImpulse, GunImpulse);

            // set animation speed
            if(GunImpulse / MaxGunImpulse > 0.8) {
                sprite.SetAnimationState(DoorSprite.AnimationState.Fast);
            } else if (GunImpulse / MaxGunImpulse > 0.5) {
                sprite.SetAnimationState(DoorSprite.AnimationState.Medium);
            } else if (GunImpulse / MaxGunImpulse > 0.1) {
                sprite.SetAnimationState(DoorSprite.AnimationState.Slow);
            } else {
                sprite.SetAnimationState(DoorSprite.AnimationState.Stopped);
            }
                
        }

        public string DoorSpeedDescription
        {
            get
            {
                if (GunImpulse < 0.1 * MaxGunImpulse)
                {
                    return "mild";
                }
                else if (GunImpulse < 0.2 * MaxGunImpulse)
                {
                    return "moderate";
                }
                else if (GunImpulse < 0.3 * MaxGunImpulse)
                {
                    return "a little much";
                }
                else if (GunImpulse < 0.4 * MaxGunImpulse)
                {
                    return "way too much";
                }
                else if (GunImpulse < 0.5 * MaxGunImpulse)
                {
                    return "worrisome";
                }
                else if (GunImpulse < 0.6 * MaxGunImpulse)
                {
                    return "crazy";
                }
                else if (GunImpulse < 0.7 * MaxGunImpulse)
                {
                    return "warranty-voiding";
                }
                else if (GunImpulse < 0.8 * MaxGunImpulse)
                {
                    return "¡picante!";
                }
                else
                {
                    return "¡muy picante!";
                }
            }
        }
    }
}
