﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using BoxWorld = Box2DX.Dynamics.World;

namespace DemonDoor
{
    class World
    {
        public World(Vector2 gravity)
        {
            // set up the world.
            AABB aabb = new AABB();
            aabb.LowerBound = new Vec2(-10000f, -10000f);
            aabb.UpperBound = new Vec2(10000f, 10000f);

            _boxWorld = new BoxWorld(aabb, Utility.X2BVec2(gravity), true);

            // set up the ground.
            BodyDef groundDef = new BodyDef();
            groundDef.Position.Set(0f, -10f);

            _ground = _boxWorld.CreateBody(groundDef);

            PolygonDef groundBox = new PolygonDef();
            groundBox.SetAsBox(1000f, 10f);

            _ground.CreateShape(groundBox);
            _ground.SetMass(new MassData { Mass = 0.0f });

            _listener = new _Listener();
            _boxWorld.SetContactListener(_listener);
        }

        public void Simulate(GameTime now)
        {
            TimeSpan step = now.ElapsedGameTime;

            _boxWorld.Step((float)step.TotalSeconds, 6, 2);
        }

        internal Body AddBody(BodyDef bodyDef)
        {
            return _boxWorld.CreateBody(bodyDef);
        }

        private class _Listener : ContactListener
        {
            public override void Add(ContactPoint point)
            {
                //base.Add(point);
                //Console.Out.WriteLine("Add");
                base.Add(point);
            }

            public override void Persist(ContactPoint point)
            {

                //Console.Out.WriteLine("Persist");

                base.Persist(point);
            }

            public override void Remove(ContactPoint point)
            {

                //Console.Out.WriteLine("Remove");

                base.Remove(point);
            }

            public override void Result(ContactResult point)
            {

                //Console.Out.WriteLine("Result");
                base.Result(point);
            }
        }

        private _Listener _listener;

        private BoxWorld _boxWorld;
        private Body _ground;
    }
}
