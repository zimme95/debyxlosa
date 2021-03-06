﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems
{
    public class MoveSystem : ISystem
    {
        private int _ScreenWitdh { get; }
        private int _ScreenHight { get; }
        private const float acceleration = 1.0f;
        private float friction = 0.0005f;


        public MoveSystem()
        {

            this._ScreenHight = GamePropertyManager.Instance.getGraphics().Viewport.Height;
            this._ScreenWitdh = GamePropertyManager.Instance.getGraphics().Viewport.Width;
        }

        public void Update(GameTime _gameTime)
        {
            Dictionary<int, EntityComponent> _VelocityDict = ComponentManager.Instance.getComponentDictionary<VelocityComponent>();
            Dictionary<int, EntityComponent> _PositionDict = ComponentManager.Instance.getComponentDictionary<PositionComponent>();
            Dictionary<int, EntityComponent> _ActionDirectionDict = ComponentManager.Instance.getComponentDictionary<ActionDirectionComponent>();
            Dictionary<int, EntityComponent> _BarComponent = ComponentManager.Instance.getComponentDictionary<BarComponent>();
            EntityComponent actionComp;
            EntityComponent posComp;
            EntityComponent barComp ;
            //använd _gameTime
            var dT = (float)_gameTime.ElapsedGameTime.TotalSeconds;
            if (_VelocityDict != null && _PositionDict !=null && _ActionDirectionDict !=null && _BarComponent != null)
            {
                foreach (VelocityComponent vel in _VelocityDict.Values)
                {
                    if (_PositionDict.TryGetValue(vel.EntityId, out posComp))
                    {
                        PositionComponent pos = (PositionComponent)posComp;
                        if (_ActionDirectionDict.TryGetValue(vel.EntityId, out actionComp))
                        {
                            ActionDirectionComponent actiondir = (ActionDirectionComponent)actionComp;
                            RotationComponent rotation = ComponentManager.Instance.getComponentByID<RotationComponent>(vel.EntityId);
                            if (actiondir.Left)
                            {
                                rotation.Rotation -= 5.0f * dT;
                                actiondir.Left = false;
                            }
                            if (actiondir.Right)
                            {
                                rotation.Rotation += 5.0f * dT;
                                actiondir.Right = false;
                            }
                            if (actiondir.Up)
                            {

                                vel.VelX += (float)Math.Cos(rotation.Rotation - 1.5f) * acceleration;
                                vel.VelY += (float)Math.Sin(rotation.Rotation - 1.5f) * acceleration;
                                //actiondir.Up = false;
                            }
                            else if (vel.VelX != 0 || vel.VelY != 0)
                            {
                                float i = vel.VelX;
                                float j = vel.VelY;

                                vel.VelX = i -= friction * i;
                                vel.VelY = j -= friction * j;
                            }

                            if (_BarComponent.TryGetValue(vel.EntityId, out barComp))
                            {
                                BarComponent barComponent = (BarComponent)barComp;
                                if (barComponent.bar >= 1f)
                                {
                                    if (actiondir.Special)
                                    {
                                        barComponent.bar = 0.0f;
                                        barComponent.seconds = 0.0f;
                                        vel.VelX *= 2f;
                                        vel.VelY *= 2f;
                                    }

                                }
                                if(barComponent.bar < 1f)
                                {
                                    barComponent.seconds += (float)_gameTime.ElapsedGameTime.TotalSeconds;
                                    barComponent.bar = barComponent.seconds / barComponent.span;
                                    if(barComponent.bar > 1f)
                                    {
                                        barComponent.bar = 1f;
                                    }
                                }

                            }                            
                        }
                        //Förflyttning
                        pos.X += (vel.VelX * dT);
                        pos.Y += (vel.VelY * dT);


                        //Håll Entitet innanför fönsterkanter
                        if (pos.X > _ScreenWitdh)  //TODO ändra till spritebredd
                            pos.X = _ScreenWitdh;
                        if (pos.X < 0)
                            pos.X = 0;
                        if (pos.Y > _ScreenHight)
                            pos.Y = _ScreenHight;
                        if (pos.Y < 0)
                            pos.Y = 0;

                        //Om kollision med kant byt vinkel
                        if (pos.X >= _ScreenWitdh || pos.X <= 0)
                        {
                            vel.VelX *= -1;
                        }

                        if (pos.Y >= _ScreenHight || pos.Y <= 0)
                        {
                            vel.VelY *= -1;
                        }
                    }
                }
            }         
        }
    }
}
