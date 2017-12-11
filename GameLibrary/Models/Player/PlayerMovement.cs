using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Player
{
    public class PlayerMovement : ICharacterMovement
    {
        private float MaxWidth { get; set; }
        private float MaxHeight { get; set; }
        private IPoint CurrentLoc { get; set; }
        private IPoint RespawnLoc { get; set; }

        public PlayerMovement(float w, float h, IPoint loc, IPoint respawn)
        {
            MaxWidth = w;
            MaxHeight = h;
            CurrentLoc = loc ?? throw new ArgumentNullException(nameof(loc));
            RespawnLoc = respawn ?? throw new ArgumentNullException(nameof(respawn));
        }

        public void ResetPlayerToRespawn()
        {
            CurrentLoc = RespawnLoc;
        }
        public void PerformMovement()
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
                                if (CurrentLoc.Y < MaxHeight - 1)
                                    CurrentLoc.Y++;
                            }
                            break;
                        case 2:
                            {
                                if (CurrentLoc.Y > 0)
                                    CurrentLoc.Y--;
                            }
                            break;
                        case 3:
                            {
                                if (CurrentLoc.X > 0)
                                    CurrentLoc.X--;
                            }
                            break;
                        case 4:
                            {
                                if (CurrentLoc.X < MaxWidth - 1)
                                    CurrentLoc.X++;
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
