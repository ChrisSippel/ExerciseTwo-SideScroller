using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExerciseTwo_SideScrollerAnswer
{
    public class Character
    {
        private Vector2 _position;
        private Vector2 _dimensions = new Vector2(50, 50);

        public Rectangle Rectangle
        {
            get;
            private set;
        }

        public Character(Vector2 startingPosition)
        {
            _position = startingPosition;
        }

        public void Update(IEnumerable<Ground> groundToCheck)
        {
            Vector2 velocity = DetermineVelocity();

            bool moveDown;
            bool moveRight;
            bool moveLeft;
            bool moveUp;
            DetermineAllowedMovement(groundToCheck, velocity, out moveDown, out moveRight, out moveLeft, out moveUp);

            if (moveDown)
            {
                _position.Y += velocity.Y;
            }

            if (moveRight && velocity.X > 0)
            {
                _position.X += velocity.X;
            }

            if (moveLeft && velocity.X < 0)
            {
                _position.X += velocity.X;
            }

            if (moveUp && velocity.Y < 0)
            {
                _position.Y += velocity.Y;
            }

            Rectangle = new Rectangle(_position.ToPoint(), _dimensions.ToPoint());
        }

        private Vector2 DetermineVelocity()
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 velocity = new Vector2(0, 0);

            if (state.IsKeyDown(Keys.Right) && state.IsKeyDown(Keys.Left))
            {
                velocity.X = 0;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                velocity.X = 2;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                velocity.X = -2;
            }

            if (state.IsKeyDown(Keys.Space))
            {
                velocity.Y = -2;
            }
            else
            {
                velocity.Y = 1;
            }

            return velocity;
        }

        private void DetermineAllowedMovement(IEnumerable<Ground> groundToCheck, Vector2 velocity, out bool moveDown, out bool moveRight, out bool moveLeft, out bool moveUp)
        {
            moveDown = true;
            moveRight = true;
            moveLeft = true;
            moveUp = true;

            Vector2 expectedPosition = _position + velocity;
            Rectangle expectedRectangle = new Rectangle(expectedPosition.ToPoint(), _dimensions.ToPoint());
            foreach (Ground ground in groundToCheck)
            {
                if (expectedRectangle.Intersects(ground.Bounds))
                {
                    Rectangle collisionRectangle = Rectangle.Intersect(expectedRectangle, ground.Bounds);
                    if (collisionRectangle.Height == 1)
                    {
                        if (collisionRectangle.Bottom - collisionRectangle.Center.Y == 1)
                        {
                            moveDown &= false;
                        }
                        else
                        {
                            moveUp &= false;
                        }
                    }
                    else
                    {

                    }

                    // The character is within the height of the ground
                    if (expectedRectangle.Bottom >= ground.Bounds.Top &&
                        expectedRectangle.Top <= ground.Bounds.Bottom)
                     {
                        if (expectedRectangle.Right > ground.Bounds.Left &&
                            expectedRectangle.Right < ground.Bounds.Right)
                        {
                            moveRight &= false;
                        }
                        else if (expectedRectangle.Left < ground.Bounds.Right &&
                                expectedRectangle.Left > ground.Bounds.Left)
                        {
                            moveLeft &= false;
                        }
                    }

                    // We continue because you can only ever be on top, to the right, to the left, or underneath a square never a combination.
                    ////int rightIntersection = expectedRectangle.Right - ground.Bounds.Left;
                    ////int leftIntersection = expectedRectangle.Left - ground.Bounds.Right;
                    ////int topIntersection = expectedRectangle.Top - ground.Bounds.Bottom;
                    ////int bottomIntersection = expectedRectangle.Bottom - ground.Bounds.Top;




                     

                    

                    ////if (expectedRectangle.Top < ground.Bounds.Bottom &&
                    ////    expectedRectangle.Top > ground.Bounds.Top)
                    ////{
                    ////    moveUp &= false;
                    ////}

                    ////if (expectedRectangle.Bottom > ground.Bounds.Top &&
                    ////    expectedRectangle.Bottom < ground.Bounds.Bottom)
                    ////{
                    ////    moveDown &= false;
                    ////}
                }
            }
        }
    }
}
