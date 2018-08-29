using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace Dependency
{
    class Program
    {
        static void Main(string[] args)
        {
            // string connectionString = @"server=testdependencydb.ccofsau8yzbw.us-east-1.rds.amazonaws.com;database=TestD;user id=dbUser2018;password=9X9=eighty1;";
            // var changeListener = new DatabaseChangeListener(connectionString);
            // changeListener.OnChange += () => {
            //  Console.WriteLine("There was a change");
            //  changeListener.Start(@"SELECT PersonID from dbo.Persons");
            // };
            // changeListener.Start(@"SELECT PersonID from dbo.Persons");
            RegisterForNotification(false);
            // Console.WriteLine("Hello Billy");
            Console.ReadLine();
        }

        public static void RegisterForNotification(bool stop)
        {
            var connectionString = @"server=testdependencydb.ccofsau8yzbw.us-east-1.rds.amazonaws.com;database=TestD;user id=dbUser2018;password=9X9=eighty1;";
            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine(connection.State);
                connection.Open();
                Console.WriteLine(connection.State);
               // SqlDependency.Stop(connectionString, "PersonChangeMessages");
                var queryString = @"SELECT LastName from dbo.Persons";
                using (var oCommand = new SqlCommand(queryString, connection))
                {
                    // Starting the listener infrastructure...

                   // SqlDependency.Stop(connectionString, "PersonChangeMessages");
                    // if(stop) SqlDependency.Stop(connectionString, "PersonChangeMessages");
                    SqlDependency.Start(connectionString, "PersonChangeMessages");

                    var oDependency = new SqlDependency(oCommand, "Service=PersonChangeNotifications", 0);
                    oDependency.OnChange += OnNotificationChange;

                    // NOTE: You have to execute the command, or the notification will never fire.
                   using(SqlDataReader reader =  oCommand.ExecuteReader())
                   {
                       Console.WriteLine(reader.HasRows);
                   }
                    //SqlDependency.Stop(connectionString, "PersonChangeMessages");
                }
            }
        }


        private static void OnNotificationChange(object sender, SqlNotificationEventArgs e)
        {
            var dependency = sender as SqlDependency;
            Console.WriteLine($"OnDependencyChange Event fired. SqlNotificationEventArgs: Info={e.Info}, Source={e.Source}, Type={e.Type}");
            Console.WriteLine(e.Info != SqlNotificationInfo.Error);

            // //Re-register the SqlDependency. 
            if ((e.Info != SqlNotificationInfo.Error)
        && (e.Type != SqlNotificationType.Subscribe))
            {

                RegisterForNotification(true);
            }
            else
            {
                RegisterForNotification(true);
            }
            //        var dependency = sender as SqlDependency;

            // if (dependency != null) dependency.OnChange -= OnNotificationChange;
            // if (e != null)
            // {
            //     e();
            // }

        }

        static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            Console.WriteLine("Hi!");
        }
    }
    public class DatabaseChangeListener
    {


        public DatabaseChangeListener(string connectionString)
        {
            this.connectionString = connectionString;
            SqlDependency.Stop(connectionString, "PersonsChangeMessages");
            SqlDependency.Start(connectionString, "PersonsChangeMessages");
            connection = new SqlConnection(connectionString);
        }


        ~DatabaseChangeListener()
        {
            SqlDependency.Stop(connectionString, "PersonsChangeMessages");
        }



        private readonly string connectionString;
        private readonly SqlConnection connection;

        public delegate void NewMessage();
        public event NewMessage OnChange;



        public DataTable Start(string changeQuery)
        {
            using (SqlCommand cmd = new SqlCommand(changeQuery, connection) { Notification = null })
            {
                SqlDependency dependency = new SqlDependency(cmd, "Service=PersonsChangeNotifications", 1);
                Console.WriteLine(dependency.HasChanges);
                Console.WriteLine(dependency.Id);
                dependency.OnChange += NotifyOnChange;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (DataTable dt = new DataTable())
                {
                    dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                    return dt;
                }
            }
        }

        void NotifyOnChange(object sender, SqlNotificationEventArgs e)
        {
            var dependency = sender as SqlDependency;
            Console.WriteLine($"OnDependencyChange Event fired. SqlNotificationEventArgs: Info={e.Info}, Source={e.Source}, Type={e.Type}");


            if (dependency != null) dependency.OnChange -= NotifyOnChange;
            if (OnChange != null)
            {
                OnChange();
            }
        }



    }
}
