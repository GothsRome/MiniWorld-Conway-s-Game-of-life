using System.Security.Cryptography;
using System.Windows.Forms;
namespace Conway_s_Game_of_life
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); 
            Application.Run(new Form1());
            return;
        }
    }
}