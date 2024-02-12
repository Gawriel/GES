using System;
using System.IO;

namespace GES
{
    public class LogManage
    {
        public string logFileName;
        public LogManage(string logFileName)
        {
            this.logFileName = logFileName;
        }

        public void Create()
        {
            StreamWriter sw = File.CreateText(logFileName);
            sw.Close();
        }

        public void Write(string text, bool orchestrator = true, bool console = true, bool file = true)
        {
            StreamWriter sw = File.AppendText(logFileName);
            if (file) Log(sw, text);
            sw.Close();
            if (console) Console.WriteLine(text);
        }


        #region Funzioni private

        private void Log(StreamWriter sw, string text)
        {
            // Data e ora in testa alla riga
            string sNow = DateTime.Now.ToString() + "    ";
            sw.Write(sNow);

            // Split 
            string[] tokens = text.Split('\n');

            // Se vi sono più righe, costruiamo l'header
            string header = "";
            if (tokens.Length > 1)
            {
                for (int j = 1; j <= sNow.Length; j++)
                    header += " ";
            }

            // Scrittura di ogni riga (la prima dopo data e ora, le successive incolonnate)
            for (int i = 0; i < tokens.Length; i++)
            {
                if (i == 0) sw.WriteLine(tokens[i]);
                else sw.WriteLine(header + tokens[i]);
            }

            // Flush
            sw.Flush();
        }

        #endregion

    }
}
