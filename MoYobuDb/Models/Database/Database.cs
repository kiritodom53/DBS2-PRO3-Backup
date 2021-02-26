using System;
using System.Data;
using Microsoft.AspNetCore.Mvc.Internal;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database
{
    class Database
    {
        private static Database _instance;

        private Database()
        {
            //Con = new OracleConnection("Data Source=localhost:1521/xe;User Id=dom53;Password=admin;");
            conn = new OracleConnection(
                "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))(CONNECT_DATA=(SID=xe))); User Id = dom53; Password = admin;");
            //conn = new OracleConnection("Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = ora1.uhk.cz)(PORT = 1521)))(CONNECT_DATA=(SID=orcl))); User Id = DB2mandido1; Password = DB2mandido1;");
        }

        public OracleConnection conn { get; }

        /// <summary>
        ///     Otevření spojení do databáze
        /// </summary>
        public void Open()
        {
            try
            {
                conn.Open();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        ///     Zavření spojení do databáze
        /// </summary>
        public void Close()
        {
            conn.Close();
        }

        /// <summary>
        ///     Vrací instanci databáze
        /// </summary>
        /// <returns>Instance databáze</returns>
        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
            }

            return _instance;
        }

        /// <summary>
        ///     Vytvoření příkazu pro Oracle databázy
        /// </summary>
        /// <param name="query">sql dotaz</param>
        /// <returns>Vrací Oracle příkaz pro databázy</returns>
        public OracleCommand CreateCommnad(string query)
        {
            OracleCommand cmd = new OracleCommand
            {
                Connection = this.conn, CommandText = query, CommandType = CommandType.Text
            };

            return cmd;
        }

        // TODO: Metody pro zjištěný obsahu v databázy
    }
}