using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace DapperExemplo1
{
    class Program
    {
        async static Task Main(string[] args)
        {
            //Listar();
            //Listar2();
            //Listar3();
            //ObterUm("angelo");
            //ListagemMultipla();
            //Inserir();
            await InserirAsync();
            Console.ReadKey();
        }
        static void Listar()
        {
            IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true");
            db.Open();
            var result = db.Query<dynamic>("select * from Utilizadors");

            foreach(var item in result)
            {
                Console.WriteLine($"Id: {item.Id} - Nome: {item.Nome}");
            }

            db.Close();
        }
        static void Listar2()
        {
            IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true");
            db.Open();
            var result = db.Query<Utilizador>("select * from Utilizadors");

            foreach (var item in result)
            {
                Console.WriteLine($"Id: {item.Id} - Nome: {item.Nome}");
            }

            db.Close();
        }
        static void Listar3()
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true"))
            {
                var result = db.Query<Utilizador>("select * from Utilizadors").ToList();

                foreach (var item in result)
                {
                    Console.WriteLine($"Id: {item.Id} - Nome: {item.Nome}");
                }
            }
        }
        static void ObterUm(string userName)
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true"))
            {
                var result = db.QueryFirst<Utilizador>("select * from Utilizadors where UserName=@userName", new { userName });
                Console.WriteLine("--------------||-----------------");
                Console.WriteLine($"Id: {result.Id} - Nome: {result.Nome}");
            }
        }
        static void ListagemMultipla()
        {
            var consulta = string.Format(@"
                                                                select * from AspNetUsers;
                                                                select * from Utilizadors where UserName='angelo'");

            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true"))
            {
                using (var resultado = db.QueryMultiple(consulta))
                {
                    var AspNetUsers = resultado.Read().ToList();
                    var Utilizadors = resultado.Read().Single();

                    foreach (var item in AspNetUsers)
                    {
                        Console.WriteLine($"Id: {item.Id} - Email: {item.Email}");
                    }
                    Console.WriteLine($"Id: {Utilizadors.Id} - Nome: {Utilizadors.Nome}");
                }

            }
        }
        static void Inserir()
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true"))
            {
                var result = db.Execute("insert into Utilizadors (Id, UserName, Nome, Password, DataRegisto, Status) values(newid(),'teste','teste', 'teste', getdate(), 1)");
                Console.WriteLine(result);
            }
        }
        async static Task<int> InserirAsync()
        {
            using (IDbConnection db = new SqlConnection("Server=localhost;Database=Utanga;Integrated Security=true"))
            {
                var result = await db.ExecuteAsync("insert into Utilizadors (Id, UserName, Nome, Password, DataRegisto, Status) values(newid(),'teste','teste', 'teste', getdate(), 1)");
                return result;
            }
        }
    }
}
