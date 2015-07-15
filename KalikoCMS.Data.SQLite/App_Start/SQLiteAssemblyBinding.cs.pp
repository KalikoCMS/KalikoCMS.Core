namespace $rootnamespace$ {
    using System;
    using KalikoCMS.Core;

    public class SQLiteAssemblyBinding : IStartupSequence {
        public void Startup() {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            if (args.Name.StartsWith("System.Data.SQLite,")) {
                return typeof(System.Data.SQLite.SQLiteFactory).Assembly;
            }
            return null;
        }

        public int StartupOrder { get { return -1; } }
    }
}