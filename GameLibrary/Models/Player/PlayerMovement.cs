using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Player
{
    public class PlayerMovement : ICharacterMovement
    {
        public float MaxWidth { get; private set; }
        public float MaxHeight { get; private set; }

        public PlayerMovement(float w, float h)
        {
            MaxWidth = w;
            MaxHeight = h;
        }

        public void ResetPlayerToRespawn(IPoint loc, IPoint respawn)
        {
            loc = respawn;
        }
        public void PerformMovement(IPoint loc)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("1. Move Up");
                    Console.WriteLine("2. Move Down");
                    Console.WriteLine("3. Move Left");
                    Console.WriteLine("4. Move Right");
                    int option = int.Parse(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            {
                                if (loc.Y < MaxHeight - 1)
                                    loc.Y++;
                            }
                            break;
                        case 2:
                            {
                                if (loc.Y > 0)
                                    loc.Y--;
                            }
                            break;
                        case 3:
                            {
                                if (loc.X > 0)
                                    loc.X--;
                            }
                            break;
                        case 4:
                            {
                                if (loc.X < MaxWidth - 1)
                                    loc.X++;
                            }
                            break;
                        default:
                            {
                                Console.WriteLine("Invalid direction\r\n");
                                continue;
                            }
                    }
                    return;
                }
                catch (Exception e)
                {
                    StaticHelperClass.PrintException(e, 2);
                }
            }
        }
    }
}
