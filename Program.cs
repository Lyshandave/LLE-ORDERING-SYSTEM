*Program.cs* (entry point)
using System;
using System.Windows.Forms;

namespace LLEORDERINGSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new OrderingSystem());
        }
    }
}
