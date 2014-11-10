
The SQLite provider for Kaliko CMS has been installed.

In some environment you might face an error saying: "System.Configuration.ConfigurationErrorsException: 
Database provider System.Data.SQLite.SQLiteFactory, System.Data.SQLite, Version=1.0.86.0, Culture=neutral, 
PublicKeyToken=db937bc2d44ff139 not installed" after a first deploy.

You can fix this by adding an assembly binding to the version of SystemData.SQLite that you have in your 
project. For instance for version 0.94 it would look like:

Assembly binding
<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Data.SQLite" publicKeyToken="db937bc2d44ff139" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-1.0.94.0" newVersion="1.0.94.0" />
      </dependentAssembly>
    </assemblyBinding>
</runtime>

Be sure to replace the version number with the actual version of SQLite in your project (you find this 
on either the SQLite DLL itself or the NuGet package.

For more information about getting started using Kaliko CMS, please visit: 
http://kaliko.com/cms/get-started
