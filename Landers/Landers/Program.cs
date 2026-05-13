using Raylib_cs;
using System.Numerics;

class Rocket
{
    public Vector2 position;
    public Vector2 velocity;
    public float rotation;
    public float fuel = 100f;

    public bool thrusting = false;

    const float gravity = 0.2f;
    const float thrustPower = 0.4f;

    public Rocket(Vector2 startPos)
    {
        position = startPos;
        velocity = Vector2.Zero;
        rotation = 0f;
    }

    public void Update()
    {
        thrusting = false;

        if (Raylib.IsKeyDown(KeyboardKey.A)) rotation -= 2f;
        if (Raylib.IsKeyDown(KeyboardKey.D)) rotation += 2f;

        if (Raylib.IsKeyDown(KeyboardKey.W) && fuel > 0)
        {
            thrusting = true;
            fuel -= 0.3f;

            float rad = rotation * (float)Math.PI / 180f;

            Vector2 force = new Vector2(
                (float)Math.Sin(rad),
                -(float)Math.Cos(rad)
            );

            velocity += force * thrustPower;
        }

        velocity.Y += gravity;

        position += velocity;
    }

    public void Draw()
    {
        Rectangle rect = new Rectangle(position.X, position.Y, 20, 40);
        Vector2 origin = new Vector2(10, 20);

        Raylib.DrawRectanglePro(rect, origin, rotation, Color.White);

        if (thrusting)
        {
            Rectangle flame = new Rectangle(position.X, position.Y + 25, 10, 20);
            Vector2 flameOrigin = new Vector2(5, -10);

            Raylib.DrawRectanglePro(flame, flameOrigin, rotation, Color.Orange);
        }
    }
}

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "Lunar Lander");
        Raylib.SetTargetFPS(60);

        Rocket rocket = new Rocket(new Vector2(400, 100));

        Rectangle landingPad = new Rectangle(300, 550, 200, 20);

        bool gameOver = false;
        string resultText = "";

        while (!Raylib.WindowShouldClose())
        {
            if (!gameOver)
            {
                rocket.Update();

                if (rocket.position.Y + 20 >= landingPad.Y &&
                    rocket.position.X > landingPad.X &&
                    rocket.position.X < landingPad.X + landingPad.Width)
                {
                    if (rocket.velocity.Length() < 2f)
                    {
                        resultText = "That was so fucking cool!";
                    }
                    else
                    {
                        resultText = "Holy shit you are so bad!";
                    }

                    gameOver = true;
                    rocket.velocity = Vector2.Zero;
                }
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawRectangleRec(landingPad, Color.Gray);

            rocket.Draw();

            Raylib.DrawText($"Fuel: {rocket.fuel:0}", 10, 10, 20, Color.Green);

            if (gameOver)
            {
                Raylib.DrawText(resultText, 300, 200, 30, Color.Yellow);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}