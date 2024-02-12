using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GES
{
    public static class AppGlobal
    {
        private static AppSettingsReader appReader = new AppSettingsReader();
        public static LogManage log = new LogManage(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ges.log");
        public static string dataBasePath = appReader.GetValue("DataBase", typeof(string)).ToString();
    }
}
