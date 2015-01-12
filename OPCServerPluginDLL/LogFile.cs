using System;
using System.IO;
using System.Configuration;
using System.Security;
using System.Security.Permissions;

namespace NSPlugin
{

	//==============================================================================
   public class LogFile
	{
		private static string   LogFileName = null;
      private static string   excpMessage ;
      private static string   excpMessage1 = "No write access to log file " ;



      //------------------------------------------
      public static void Init( string fname )
      {
         try
         {
            string path  = "" ;
            int p = fname.LastIndexOf( "\\" );
            if( p < 0 )          // simple filename specified
            {
               System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
               if( asm != null )
               {
                  path = asm.Location ;    // EXE file path
                  int idx = path.LastIndexOf( "\\" );
                  if( idx >= 0) path = path.Substring(0,idx+1);
                  else          path = "";
               }
               else     // seems to be a web service
               {
                  System.Web.HttpContext hc = System.Web.HttpContext.Current;
                  if (hc != null)   // asmx web service
                     path = hc.Server.MapPath(".") + "\\";   // web dir
                  else    // WCF service
                  {
#if ! VS2003
                     path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
#endif
                  }
               }
            }
            LogFileName = path + fname ;
            excpMessage = excpMessage1 + LogFileName ;

            FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.AllAccess, LogFileName );
            string[] pathlist = { path };
            f.AddPathList(FileIOPermissionAccess.AllAccess, pathlist );
         }
         catch
         {
            LogFileName = null;   // disable logging
            throw new Exception( excpMessage );
         }      
      }

      	
      //------------------------------------------
		public static void Write( string msg )
		{
			if( LogFileName != null )
			{
            try
            {
               FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.AllAccess, LogFileName );
               string path = LogFileName.Substring( 0, LogFileName.LastIndexOf( "\\" )+1 );
               string[] pathlist = { path };
               f.AddPathList(FileIOPermissionAccess.AllAccess, pathlist );
               FileStream fs = new FileStream( LogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
               StreamWriter w = new StreamWriter(fs);	// create a stream writer 
               w.BaseStream.Seek(0, SeekOrigin.End);	// to the end of file 
               w.Write( "{0}: {1}\r\n", DateTime.Now.ToString(), msg ); 
               w.Flush();		// update underlying file
               fs.Close() ;
            }
            catch
            {
               LogFileName = null;   // disable logging
               throw new Exception( excpMessage );
            }
			}
		}

		public static void Write( string[] msg )
		{
			if( LogFileName != null )
			{
            try
            {
               FileStream fs = new FileStream( LogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
               StreamWriter w = new StreamWriter(fs);	// create a stream writer 
               w.BaseStream.Seek(0, SeekOrigin.End);	// to the end of file 
               w.Write( "{0}: {1}\r\n", DateTime.Now.ToString(), msg[0] ); 
               for( int i=1; i<msg.Length ; ++i )
                  w.Write( "           {0}\r\n", msg[i] );
               w.Flush();		// update underlying file
               fs.Close() ;
            }
            catch
            {
               LogFileName = null;   // disable logging
               throw new Exception( excpMessage );
            }			
         }
		}

//		public static void WriteErrors( string msg, OPCError[] err )
//		{
//			if( LogFileName != null )
//			{
//            try
//            {
//               FileStream fs = new FileStream( LogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
//               StreamWriter w = new StreamWriter(fs);	// create a stream writer 
//               w.BaseStream.Seek(0, SeekOrigin.End);	// to the end of file 
//               w.Write("{0}: {1}\r\n", DateTime.Now.ToString(), msg ); 
//               for( int i=0; i<err.Length ; ++i )
//                  w.Write( "         {0}: {1}\r\n", err[i].ID, err[i].Text );
//               w.Flush();		// update underlying file
//               fs.Close() ;
//            }
//            catch
//            {
//               LogFileName = null;   // disable logging
//               throw new Exception( excpMessage );
//            }		
//         }
//		}


	}
}

