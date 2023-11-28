using System;
using System.Data.SqlClient;
using Dapper;

class Program
{
    static void Main()
    {
        string server = "DESKTOP-S6VFEVC";
        string database = "SEXO";

        string connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            TransferirDadosBroxaParaPassivo(connection);

            var passivos = connection.Query("SELECT * FROM Passivos");

            foreach (var passivoItem in passivos)
            {
                Console.WriteLine($"Passivo - PKID: {passivoItem.PKID}, Nome: {passivoItem.Nome}, Centimetros: {passivoItem.Centimetros}, JornadaID: {passivoItem.JornadaID}");
            }

            static void TransferirDadosBroxaParaPassivo(SqlConnection connection)
            {
                var broxas = connection.Query("SELECT * FROM Broxas");

                foreach (var broxaItem in broxas)
                {
                    var passivo = new
                    {
                        PKID = broxaItem.PKID,
                        CreationTime = broxaItem.CreationTime,
                        Nome = broxaItem.Nome,
                        Centimetros = broxaItem.Centimetros,
                        JornadaID = broxaItem.RecordID
                    };

                    connection.Execute("INSERT INTO Passivos (PKID, CreationTime, Nome, Centimetros, JornadaID) VALUES (@PKID, @CreationTime, @Nome, @Centimetros, @JornadaID)", passivo);
                }

                //var broxa = new
                //{
                //    PKID = 1,
                //    CreationTime = DateTime.Now,
                //    Nome = "Broxa1",
                //    Centimetros = 15,
                //    RecordID = 1001
                //};

                //connection.Execute("INSERT INTO Broxas (PKID, CreationTime, Nome, Centimetros, RecordID) VALUES (@PKID, @CreationTime, @Nome, @Centimetros, @RecordID)", broxa);

                //var broxas = connection.Query("SELECT * FROM Broxas");

                //foreach (var broxaItem in broxas)
                //{
                //    Console.WriteLine($"Broxa - PKID: {broxaItem.PKID}, Nome: {broxaItem.Nome}, Centimetros: {broxaItem.Centimetros}, RecordID: {broxaItem.RecordID}");
                //}

                //var passivo = new
                //{
                //    PKID = 1,
                //    CreationTime = DateTime.Now,
                //    Nome = "Passivo1",
                //    Centimetros = 20,
                //    JornadaID = 2001
                //};

                //connection.Execute("INSERT INTO Passivos (PKID, CreationTime, Nome, Centimetros, JornadaID) VALUES (@PKID, @CreationTime, @Nome, @Centimetros, @JornadaID)", passivo);

                //var passivos = connection.Query("SELECT * FROM Passivos");

                //foreach (var passivoItem in passivos)
                //{
                //    Console.WriteLine($"Passivo - PKID: {passivoItem.PKID}, Nome: {passivoItem.Nome}, Centimetros: {passivoItem.Centimetros}, JornadaID: {passivoItem.JornadaID}");
                //}
            }

            Console.WriteLine("QUERO DA OCU");
        }
    }
}
