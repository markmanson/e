Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Security

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Unikaihatsu Software Pvt. Ltd.")> 
<Assembly: AssemblyProduct("eExam tool")> 
<Assembly: AssemblyCopyright("by Unikaihatsu Software Pvt. Ltd.")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: CLSCompliant(True)> 
<Assembly: PermissionSet(SecurityAction.RequestOptional, Unrestricted := True)>
<Assembly: AllowPartiallyTrustedCallers>
<assembly: log4net.Config.XmlConfigurator(ConfigFile: = "log4net.config", Watch: = true)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("7642C2F3-42F7-413C-9D10-E0A79D618F12")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:

<Assembly: AssemblyVersion("1.0.*")> 
