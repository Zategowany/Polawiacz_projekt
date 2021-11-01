using System;

namespace Polawiacz_gra
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
            for(int i = 0; i<5; i++)
            {
                int x = 0;
                x++;
            }
        }
    }
}
