using SimpleBankManagementSystems.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankManagementSystems
{
    class Program
    {
        
        static void Main(string[] args)
        {
            LoginService loginService = new LoginService();
           // loginService.DisplayLoginMenu();
            MainMenu mainMenu = new MainMenu();
            //mainMenu.PressEscapeToBackMainMenu();
            mainMenu.DisplayMenu();
        }


    }
}
