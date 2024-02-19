using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicOpenTK;

namespace OpenTK.WPF.Demo
{
    public class OpenTKHost : System.Windows.Controls.UserControl
    {
        private Game gameWin;

        public Game GameWin { get { return gameWin; } }

        public OpenTKHost()
        {
            gameWin = new Game();
            gameWin.Run();
        }
    }
}
