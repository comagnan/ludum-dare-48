using System;
using System.Collections.Generic;
using LD48.Framework;
using LD48.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace LD48.Actors
{
    public class PlayerActor : KillableActor
    {
        private const float PLAYER_ACCELERATION = 0.3f;
        private const float MAX_PLAYER_SPEED = 1f;
        private const float PLAYER_FRICTION = 9f;

        private const int PLAYER_IDLE_TRIGGER_IN_MS = 1000;
        private const int PLAYER_INVINCIBILITY_TIME_IN_MS = 1000;
        private const int MP_DRAIN_PER_MS = 1;
        private const int DAMAGE_PER_MS = 1;

        private readonly ContentManager m_Content;

        // Ye Old Animation Block.
        private AnimationPlayer m_AnimationPlayer;
        private Animation m_StaticAnimation;

        private Animation m_IdleAnimation;

        private Animation m_MovingRightAnimation;
        private Animation m_RollingRightAnimation;

        private Texture2D m_Shadow;

        private TimeSpan m_MovementTimeSpan;
        private TimeSpan m_IdleTimeSpan;
        private TimeSpan m_InvincibilityTimeSpan;

        private bool m_Rolling;
        private bool m_IsDodgeButtonEnabled;

        public bool IsDodging => m_Rolling && m_MovementTimeSpan.TotalMilliseconds > 30 && m_MovementTimeSpan.TotalMilliseconds < 370;

        public bool IsInvincible => m_InvincibilityTimeSpan > TimeSpan.Zero;
        public int CurrentMP { get; set; }
        public int MaxMP { get; set; }

        public PlayerActor(Vector2 p_InitialPosition,
                           int p_MaxHp,
                           int p_MaxMp,
                           ContentManager p_ContentManager)
        {
            // Set base actor attributes.
            CurrentPosition = p_InitialPosition;
            CurrentVelocity = new Vector2(0, 0);
            MaxHP = CurrentHP = p_MaxHp;
            MaxMP = CurrentMP = p_MaxMp;
            Solid = true;
            CollisionBoxSize = new Point(8, 9);

            m_Content = p_ContentManager;
            m_Rolling = false;
            m_IsDodgeButtonEnabled = false;
            m_MovementTimeSpan = TimeSpan.Zero;
            m_IdleTimeSpan = TimeSpan.Zero;
            m_AnimationPlayer = new AnimationPlayer();
        }

        public void Update(GameTime p_GameTime,
                           in InputController p_InputController,
                           in TiledMapTileLayer p_TileLayer)
        {
            if (m_Rolling) {
                m_MovementTimeSpan = m_MovementTimeSpan.Add(p_GameTime.ElapsedGameTime);
                if (m_MovementTimeSpan.TotalMilliseconds > 401) {
                    m_MovementTimeSpan = TimeSpan.Zero;
                    m_Rolling = false;
                }
            } else if (m_IsDodgeButtonEnabled && p_InputController.IsButtonDown(InputConfiguration.Confirm)) {
                if (CurrentVelocity.X != 0 || CurrentVelocity.Y != 0) {
                    m_Rolling = true;
                    m_MovementTimeSpan = TimeSpan.Zero;
                }
            } else {
                float dx = 0f;
                float dy = 0f;
                if (p_InputController.IsButtonDown(InputConfiguration.Right)) {
                    dx = PLAYER_ACCELERATION;
                } else if (p_InputController.IsButtonDown(InputConfiguration.Left)) {
                    dx = -PLAYER_ACCELERATION;
                }

                if (p_InputController.IsButtonDown(InputConfiguration.Down)) {
                    dy = PLAYER_ACCELERATION;
                } else if (p_InputController.IsButtonDown(InputConfiguration.Up)) {
                    dy = -PLAYER_ACCELERATION;
                }

                CurrentVelocity = CalculateVelocity(p_GameTime, dx, dy);

                if (dx != 0 || dy != 0) {
                    m_MovementTimeSpan = m_MovementTimeSpan.Add(p_GameTime.ElapsedGameTime);
                    m_IdleTimeSpan = TimeSpan.Zero;
                } else {
                    m_MovementTimeSpan = TimeSpan.Zero;
                    m_IdleTimeSpan = m_IdleTimeSpan.Add(p_GameTime.ElapsedGameTime);
                }
            }

            m_IsDodgeButtonEnabled = !p_InputController.IsButtonDown(InputConfiguration.Confirm);

            if (IsInvincible) {
                m_InvincibilityTimeSpan = m_InvincibilityTimeSpan.Subtract(p_GameTime.ElapsedGameTime);
            }

            ApplyCollision(p_GameTime, p_TileLayer);
            MoveActor(p_GameTime);
            m_AnimationPlayer.Update(p_GameTime);
        }

        public override void Update(GameTime p_Time)
        {
            // For the player, use a custom call.
            throw new NotImplementedException();
        }

        public override void Draw(GameTime p_GameTime,
                                  SpriteBatch p_SpriteBatch)
        {
            base.Draw(p_GameTime, p_SpriteBatch);

            if (m_IdleAnimation == null) {
                Texture2D idleAnimation = m_Content.Load<Texture2D>("Characters/alexis_idle");
                m_IdleAnimation = new Animation(idleAnimation,
                    new List<int> {
                        400,
                        200,
                        400,
                        200
                    },
                    16,
                    true);
                Texture2D staticAnimation = m_Content.Load<Texture2D>("Characters/alexis");
                m_StaticAnimation = new Animation(staticAnimation, new List<int> { 100 }, 16, true);
                Texture2D movingRightAnimation = m_Content.Load<Texture2D>("Characters/alexis_right_walk");
                m_MovingRightAnimation = new Animation(movingRightAnimation,
                    new List<int> {
                        200,
                        200,
                        200,
                        200
                    },
                    16,
                    true);
                Texture2D rollingRightAnimation = m_Content.Load<Texture2D>("Characters/alexis_right_roll");
                m_RollingRightAnimation = new Animation(rollingRightAnimation,
                    new List<int> {
                        140,
                        110,
                        80,
                        70
                    },
                    16,
                    false);

                m_Shadow = m_Content.Load<Texture2D>("Characters/shadow");
            }

            p_SpriteBatch.Draw(m_Shadow,
                new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, 16, 16),
                new Rectangle(0, 0, 16, 16),
                Color.White * 0.75f,
                0f,
                new Vector2(8, 0),
                SpriteEffects.None,
                0);

            if (m_Rolling) {
                m_AnimationPlayer.PlayAnimation(m_RollingRightAnimation);
            } else if (m_IdleTimeSpan.TotalMilliseconds > PLAYER_IDLE_TRIGGER_IN_MS) {
                m_AnimationPlayer.PlayAnimation(m_IdleAnimation);
            } else if (m_IdleTimeSpan == TimeSpan.Zero) {
                m_AnimationPlayer.PlayAnimation(m_MovingRightAnimation);
            } else {
                m_AnimationPlayer.PlayAnimation(m_StaticAnimation);
            }

            int yPosition = m_Rolling
                ? (int) (CurrentPosition.Y - 5 * Math.Sin(m_MovementTimeSpan.TotalMilliseconds / 190))
                : (int) CurrentPosition.Y;
            bool visible = m_InvincibilityTimeSpan == TimeSpan.Zero || m_InvincibilityTimeSpan.TotalMilliseconds % 200 < 100;
            m_AnimationPlayer.Draw(p_SpriteBatch, new Vector2((int) CurrentPosition.X, yPosition), Flip, visible);
        }

        public void ApplyHit(Vector2 p_Force)
        {
            //CurrentVelocity = new Vector2(CurrentVelocity.X + p_Force.X, CurrentVelocity.Y + p_Force.Y);
            if (!IsInvincible) {
                CurrentHP -= 1;
                CurrentVelocity = p_Force;
                m_InvincibilityTimeSpan = TimeSpan.FromMilliseconds(PLAYER_INVINCIBILITY_TIME_IN_MS);
            }
        }

        /// <inheritdoc />
        public override Rectangle GetCollisionBox()
        {
            Rectangle baseRectangle = base.GetCollisionBox();
            return new Rectangle(baseRectangle.Left, baseRectangle.Top + 1, baseRectangle.Width, baseRectangle.Height + 1);
        }

        private void ApplyCollision(GameTime p_GameTime,
                                    in TiledMapTileLayer p_Layer)
        {
            Vector2 futurePosition = GetEstimatedPosition(p_GameTime);

            ushort x1 = (ushort) ((futurePosition.X - 8) / 8);
            ushort x2 = (ushort) ((futurePosition.X + 8) / 8);
            ushort y1 = (ushort) ((futurePosition.Y - 8) / 8);
            ushort y2 = (ushort) ((futurePosition.Y + 8) / 8);
            TiledMapTile topLeft = p_Layer.GetTile(x1, y1);
            TiledMapTile topRight = p_Layer.GetTile(x2, y1);
            TiledMapTile bottomLeft = p_Layer.GetTile(x1, y2);
            TiledMapTile bottomRight = p_Layer.GetTile(x2, y2);
            bool topLeftGuilty = topLeft.GlobalIdentifier == 402 || topLeft.GlobalIdentifier == 403;
            bool topRightGuilty = topRight.GlobalIdentifier == 402 || topRight.GlobalIdentifier == 403;
            bool bottomLeftGuilty = bottomLeft.GlobalIdentifier == 402 || bottomLeft.GlobalIdentifier == 403;
            bool bottomRightGuilty = bottomRight.GlobalIdentifier == 402 || bottomRight.GlobalIdentifier == 403;

            TiledMapTile tileToCheck;
            if (topLeftGuilty) {
                tileToCheck = topLeft;
            } else if (topRightGuilty) {
                tileToCheck = topRight;
            } else if (bottomLeftGuilty) {
                tileToCheck = bottomLeft;
            } else if (bottomRightGuilty) {
                tileToCheck = bottomRight;
            } else {
                return;
            }

            bool xFirst = Math.Abs(tileToCheck.X - CurrentPosition.X) < Math.Abs(tileToCheck.Y - CurrentPosition.Y);
            if (xFirst) {
                Vector2 xPosition = new(futurePosition.X, CurrentPosition.Y);
                y1 = (ushort) ((xPosition.Y - 8) / 8);
                y2 = (ushort) ((xPosition.Y + 8) / 8);
                topLeft = p_Layer.GetTile(x1, y1);
                topRight = p_Layer.GetTile(x2, y1);
                bottomLeft = p_Layer.GetTile(x1, y2);
                bottomRight = p_Layer.GetTile(x2, y2);
                topLeftGuilty = topLeft.GlobalIdentifier == 402 || topLeft.GlobalIdentifier == 403;
                topRightGuilty = topRight.GlobalIdentifier == 402 || topRight.GlobalIdentifier == 403;
                bottomLeftGuilty = bottomLeft.GlobalIdentifier == 402 || bottomLeft.GlobalIdentifier == 403;
                bottomRightGuilty = bottomRight.GlobalIdentifier == 402 || bottomRight.GlobalIdentifier == 403;

                if (topLeftGuilty || topRightGuilty || bottomLeftGuilty || bottomRightGuilty) {
                    futurePosition = futurePosition.SetX(CurrentPosition.X);
                    CurrentVelocity = CurrentVelocity.SetX(0);
                }

                x1 = (ushort) ((futurePosition.X - 8) / 8);
                x2 = (ushort) ((futurePosition.X + 8) / 8);
                y1 = (ushort) ((futurePosition.Y - 8) / 8);
                y2 = (ushort) ((futurePosition.Y + 8) / 8);
                topLeft = p_Layer.GetTile(x1, y1);
                topRight = p_Layer.GetTile(x2, y1);
                bottomLeft = p_Layer.GetTile(x1, y2);
                bottomRight = p_Layer.GetTile(x2, y2);
                topLeftGuilty = topLeft.GlobalIdentifier == 402 || topLeft.GlobalIdentifier == 403;
                topRightGuilty = topRight.GlobalIdentifier == 402 || topRight.GlobalIdentifier == 403;
                bottomLeftGuilty = bottomLeft.GlobalIdentifier == 402 || bottomLeft.GlobalIdentifier == 403;
                bottomRightGuilty = bottomRight.GlobalIdentifier == 402 || bottomRight.GlobalIdentifier == 403;

                if (topLeftGuilty || topRightGuilty || bottomLeftGuilty || bottomRightGuilty) {
                    CurrentVelocity = CurrentVelocity.SetY(0);
                }
            } else {
                Vector2 yPosition = new(CurrentPosition.X, futurePosition.Y);
                x1 = (ushort) ((yPosition.X - 8) / 8);
                x2 = (ushort) ((yPosition.X + 8) / 8);
                topLeft = p_Layer.GetTile(x1, y1);
                topRight = p_Layer.GetTile(x2, y1);
                bottomLeft = p_Layer.GetTile(x1, y2);
                bottomRight = p_Layer.GetTile(x2, y2);
                topLeftGuilty = topLeft.GlobalIdentifier == 402 || topLeft.GlobalIdentifier == 403;
                topRightGuilty = topRight.GlobalIdentifier == 402 || topRight.GlobalIdentifier == 403;
                bottomLeftGuilty = bottomLeft.GlobalIdentifier == 402 || bottomLeft.GlobalIdentifier == 403;
                bottomRightGuilty = bottomRight.GlobalIdentifier == 402 || bottomRight.GlobalIdentifier == 403;

                if (topLeftGuilty || topRightGuilty || bottomLeftGuilty || bottomRightGuilty) {
                    futurePosition = futurePosition.SetY(CurrentPosition.Y);
                    CurrentVelocity = CurrentVelocity.SetY(0);
                }

                x1 = (ushort) ((futurePosition.X - 8) / 8);
                x2 = (ushort) ((futurePosition.X + 8) / 8);
                y1 = (ushort) ((futurePosition.Y - 8) / 8);
                y2 = (ushort) ((futurePosition.Y + 8) / 8);
                topLeft = p_Layer.GetTile(x1, y1);
                topRight = p_Layer.GetTile(x2, y1);
                bottomLeft = p_Layer.GetTile(x1, y2);
                bottomRight = p_Layer.GetTile(x2, y2);
                topLeftGuilty = topLeft.GlobalIdentifier == 402 || topLeft.GlobalIdentifier == 403;
                topRightGuilty = topRight.GlobalIdentifier == 402 || topRight.GlobalIdentifier == 403;
                bottomLeftGuilty = bottomLeft.GlobalIdentifier == 402 || bottomLeft.GlobalIdentifier == 403;
                bottomRightGuilty = bottomRight.GlobalIdentifier == 402 || bottomRight.GlobalIdentifier == 403;

                if (topLeftGuilty || topRightGuilty || bottomLeftGuilty || bottomRightGuilty) {
                    CurrentVelocity = CurrentVelocity.SetX(0);
                }
            }
        }

        public int GetRoomId()
        {
            return (int) (Math.Floor(CurrentPosition.X / 640) + 50 * Math.Floor(CurrentPosition.Y / 360));
        }

        public void Reset(Vector2 p_Position)
        {
            CurrentHP = MaxHP;
            CurrentMP = MaxMP;
            CurrentVelocity = Vector2.Zero;
            CurrentPosition = p_Position;
        }

        private Vector2 CalculateVelocity(GameTime p_GameTime,
                                          float p_Dx,
                                          float p_Dy)
        {
            Vector2 previousVelocity = new(ApplyFriction(p_GameTime, CurrentVelocity.X), ApplyFriction(p_GameTime, CurrentVelocity.Y));

            float multiplier = 1;
            if (p_Dx != 0 && p_Dy != 0) {
                multiplier = 1 / (float) Math.Sqrt(2);
            }

            Vector2 multipliedVelocity = new(previousVelocity.X + p_Dx * multiplier, previousVelocity.Y + p_Dy * multiplier);
            double hypotenuse = Math.Sqrt(multipliedVelocity.X * multipliedVelocity.X + multipliedVelocity.Y * multipliedVelocity.Y);

            return hypotenuse > MAX_PLAYER_SPEED
                ? new Vector2((float) (MAX_PLAYER_SPEED * multipliedVelocity.X / hypotenuse),
                    (float) (MAX_PLAYER_SPEED * multipliedVelocity.Y / hypotenuse))
                : multipliedVelocity;
        }

        private float ApplyFriction(GameTime p_GameTime,
                                    float p_Vector)
        {
            double vectorWithFriction = p_Vector * (1 - p_GameTime.ElapsedGameTime.TotalSeconds * PLAYER_FRICTION);
            if (Math.Abs(vectorWithFriction) < 0.02) {
                return 0;
            }

            return (float) vectorWithFriction;
        }
    }
}