//===================================================================================
// DANSrv OPC DA .NET customizable Server          Customization Plugin .Net Assembly
// --------------------------------------
//
// C# customization plugin with .NET interface to the generic OPC server part.
// This module defines the items from an XML file, using the Professional Edition
// ConfigBuilder support class.
// A simulation thread periodically changes the value of some items. 
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//
// Copyright (C) 2003-07 Advosol Inc.    (www.advosol.com)
// All rights reserved.
//-------------------------------------------------------------------------
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace NSPlugin
{

   //===============================================================================
   // OPC Server configuration and IO Handling
   //===============================================================================
   public class AppPlugin : GenericServer
   {

      //-----------------------------------------------------------------
      /// <summary>
      /// This method is called from the generic server EXE at startup.
      /// It creates all items supported by the OPC server.
      /// This method needs to call the callback method "AddItem" to add the item to the server's address space.
      /// The Item IDs are fully qualified names ( eg. Dev1.Chn5.Temp )
      /// The generic server part creates an appropriate hierarchical address space.
      /// The sample code defines the application item handle as the the buffer array index.
      /// This handle is passed in the calls from the generic server to identify the item.
      /// It should allow quick access to the signal state buffer.
      /// The handle may be implemented differently depending on the application.
      /// </summary>
      /// <param name="pszParamas">[in] String as defined in the server command-line 
      /// during the server registration.
      /// OPCappsrv /RegServer /PARAM:string </param>
      /// <returns>HRESULTS error/success Code</returns>
      new public int CreateServerItems( string cmdParams ) 
      {
         if( TraceLog != null )
            LogFile.Write( "CreateServerItems" );
         
         //////////////////  TO-DO  /////////////////
         // Create all items supported by this server.
         int rtc = Config.CreateServerItems( cbAddItem );   // the file was loaded in GetServerparameters

         if( TraceLog != null )
            LogFile.Write( "   Items created" );

         // create a thread for simulating signal changes
         // Most applications do NOT need this additional thread. They do 
         // the IO refresh handling in the in the refresh cycle thread of the generic 
         // server ( in ReadHWInputs / WriteHWOutputs )
         myThread = new Thread( new ThreadStart( UpdateThread ) ) ;
         myThread.Name = "Item Update/Simulation" ;
         myThread.Priority = ThreadPriority.AboveNormal;
         myThread.Start();
         return HRESULTS.S_OK;
      }


      //---------------------------------------------------------------
      // This method is called from the generic server at startup, when the first client connects.
      // It defines the application specific server parameters and operating modes.
      // The configuration definitions are read from the file "DANSrv.exe.config"
      // and the item set from the ConfigBuilder file "DANSrv.Items.XML"
      new public int GetServerParameters(out int UpdatePeriod, out int BrowseMode, out int validateMode, out char BranchDelemitter)
      {
         if (TraceLog != null)
            LogFile.Write("GetServerParameters");

         // Default Values
         UpdatePeriod = 100;		            // ms
         BrowseMode = (int)BROWSEMODE.REAL;        // browse the real address space (generic part internally)
         validateMode = (int)ValidateMode.Never;   // never call Plugin.ValidateItems 
         BranchDelemitter = '.';

         AppSettingsReader ar = new AppSettingsReader();
         object val = null;
         //------- UpdatePeriod Definition
         try
         {
            val = null;
            val = ar.GetValue("UpdatePeriod", typeof(int));
            if (val != null)
               UpdatePeriod = Convert.ToInt32(val);
         }
         catch
         { }

         //------ BowseMode definition
         try
         {
            val = null;
            val = ar.GetValue("BrowseMode", typeof(string));
            if (val != null)
            {
               if (val.ToString().ToUpper() == "VIRTUAL")
                  BrowseMode = (int)BROWSEMODE.VIRTUAL;
            }
         }
         catch
         { }

         //------ ValidateMode definition
         try
         {
            val = null;
            val = ar.GetValue("ValidateMode", typeof(string));
            if (val != null)
            {
               if (val.ToString().ToUpper() == "UNKNOWNITEMS")
                  validateMode = (int)ValidateMode.UnknownItems;
               else if (val.ToString().ToUpper() == "ALWAYS")
                  validateMode = (int)ValidateMode.Always;
            }
         }
         catch
         { }

         //------ BranchDelemitter definition
         try
         {
            val = null;
            val = ar.GetValue("BranchDelemitter", typeof(char));
            if (val != null)
               BranchDelemitter = Convert.ToChar(val);
         }
         catch
         { }

         //------ Item definitions XML File. If exists then use BranchDelimitter definition from XML file
         string xmlFileName = "DANSrv.Items.XML";
         try
         {
            val = null;
            val = ar.GetValue("ItemConfigFile", typeof(string));
            if (val != null)
               xmlFileName = (string)val;
         }
         catch
         { }

         if (TraceLog != null)
            LogFile.Write("ItemConfigFile = " + xmlFileName);

         string path = null;
         System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
         if (asm != null)
         {
            path = asm.Location;
            int idx = path.LastIndexOf("\\");
            if (idx >= 0) path = path.Substring(0, idx + 1);
            else path = "";
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
         string itemxml = path + xmlFileName;

         if (TraceLog != null)
            LogFile.Write("ItemConfigFile path = " + itemxml);

         bool isLoaded = false;
         Config = new ConfigBuilder.ConfigLoader();
         if (File.Exists(itemxml))
         {
            try
            {
               if (TraceLog != null)
                  LogFile.Write("LoadFromExeDir");

               Config.LoadFromExeDir(xmlFileName);             // load from external file
               if (Config.BranchSeparator != null)
                  BranchDelemitter = Config.BranchSeparator[0];
               isLoaded = true;
               if (TraceLog != null)
                  LogFile.Write("     file loaded");
            }
            catch
            { }
         }
         if (!isLoaded)
         {
            try
            {
               if (TraceLog != null)
                  LogFile.Write("LoadEmbedded");

               Config.LoadEmbedded("NSPlugin." + xmlFileName);    // load from embedded file
               isLoaded = true;
               if (TraceLog != null)
                  LogFile.Write("     file loaded");
            }
            catch
            { }
         }
         if (!isLoaded)
         {
            Config = null;
            System.Diagnostics.EventLog elog = new System.Diagnostics.EventLog("Application");
            elog.Source = "DANSrv";
            elog.WriteEntry("File " + itemxml + " not found", System.Diagnostics.EventLogEntryType.Error);
         }
         return HRESULTS.S_OK;
      }



      //----------------------------------------------------------------
      /// <summary>
      /// This method is called from the generic server when a shutdown is executed 
      /// because all clients have disconnected.
      /// To ensure proper process shutdown, all used communication channels should 
      /// be closed and threads terminated before this method returns.
      /// </summary>
      new public void  ShutdownSignal()
      {
         //////////////////  TO-DO  /////////////////
         // close the device communication

         // terminate the simulation thread
         StopThread = new ManualResetEvent( false );
         StopThread.WaitOne( 5000, true );
         StopThread.Close();
         StopThread = null;
      }



      //-------------------------------------------------------------------------------------
      /// <summary>
      /// Refreshes the items listed in the appHandles array in the cache.
      /// This method is called when an OPC client executes:
      /// - a read from Device 
      /// - a read with maxAge ( only for the items that are too old in the cache )
      /// </summary>
      /// <param name="appHandles"></param>
      /// <returns></returns>
      new public int RefreshItems( int[] appHandles )
      {
         // TO-DO:  read the requested items from the device

         foreach( int aHnd in appHandles )   // write the new values into the generic server cache
         {
            SetItemValue( aHnd, Config.Items[aHnd].Value, (short)Config.Items[aHnd].Quality, DateTime.UtcNow );
         }
         return 0;
      }


      //-------------------------------------------------------------------------------------
      /// <summary>
      /// This method is called when a client executes a 'write' server call. 
      /// The items specified in the appHandles array need to be written to the device. 
      /// The generic server updates the cache for these items.
      /// </summary>
      /// <param name="instanceHandle">Handle the identifies the calling client application. 
      /// The method GetServerInstancesInfo can be used to get name information for this handle.</param>
      /// <param name="values">object with handle, value, quality, timestamp</param>
      /// <param name="errors">array with S_OK or error codes on return.</param>
      /// <returns></returns>
      new public int WriteItems(int instanceHandle, DeviceItemValue[] values, out int[] errors)
      {
         int rtc = HRESULTS.S_OK;
         errors = new int[ values.Length ];           // result array
         for( int i=0 ; i<values.Length ; ++i )       // init to S_OK
            errors[i] = HRESULTS.S_OK;

         // TO-DO: write the new values to the device
         // sample code
         for( int i=0 ; i<values.Length ; ++i )       // handle all items
         {
            int ItemHandle = values[i].Handle ;

            if (Config.Items[ItemHandle].name == "ServerInfo.Shutdown Request")
            {
               if (values[i].Value.GetType() == typeof(bool))
               {
                  if ((bool)values[i].Value)
                  {
                     ShutDownRequest("OPC DA Client requested shutdown.");
                  }
               }
            }
            // check for EUType enumerated
            bool isEnum = false;
            string[] euInfo = new string[0];
            if (Config.Items[ItemHandle].itemDefs.properties != null)
            {
               try
               {
                  foreach (var prop in Config.Items[ItemHandle].itemDefs.properties)
                  {
                     if (prop.id == 7)   //EUType
                     {
                        if ((int)prop.val == 2)  // enumerator
                        {
                           isEnum = true;
                           foreach (var peu in Config.Items[ItemHandle].itemDefs.properties)
                           {
                              if (peu.id == 8)   //EUInfo
                              {
                                 euInfo = (string[])peu.val;
                              }
                           }
                        }

                        else if ((int)prop.val == 1)  // analog
                        {
                           double euHigh = double.MaxValue;
                           double euLow = double.MinValue;
                           foreach (var peu in Config.Items[ItemHandle].itemDefs.properties)
                           {
                              if (peu.id == 102)   //highEU
                                 euHigh = (double)peu.val;
                              else if (peu.id == 103)   //lowEU
                                 euLow = (double)peu.val;
                           }
                           double dv = Convert.ToDouble(values[i].Value);
                           if (dv < euLow || dv > euHigh)
                           {
                              errors[i] = HRESULTS.OPC_E_RANGE;
                              rtc = HRESULTS.S_FALSE;
                           }
                        }
                     }
                  }
               }
               catch { }
            }
            if (isEnum)
            {
               uint idx = uint.MaxValue ;
               try
               {
                  idx = Convert.ToUInt32(values[i].Value);
               }
               catch { }
               if (idx > euInfo.Length)
               {
                  errors[i] = HRESULTS.OPC_E_RANGE;
                  rtc = HRESULTS.S_FALSE;
               }
               else
               {
                  if (string.IsNullOrEmpty(euInfo[idx]))
                  {
                     errors[i] = HRESULTS.OPC_E_RANGE;
                     rtc = HRESULTS.S_FALSE;
                  }
               }
            }

            if (errors[i] == HRESULTS.S_OK)
            {
               Config.Items[ItemHandle].Value = values[i].Value;
               if (values[i].QualitySpecified)
                  Config.Items[ItemHandle].Quality = (ConfigBuilder.OPCQuality)values[i].Quality;
               if (values[i].TimestampSpecified)
                  Config.Items[ItemHandle].Timestamp = values[i].Timestamp;
            }
         }

         return rtc;
      }

      //-------------------------------------------------------------------------------------
      /// <summary>
      /// This method overload is called from the Standard Edition generic server.
      /// </summary>
      /// <param name="values">object with handle, value, quality, timestamp</param>
      /// <param name="errors">array with S_OK or error codes on return.</param>
      /// <returns></returns>
      new public int WriteItems(DeviceItemValue[] values, out int[] errors)
      {
         return WriteItems(0, values, out errors);
      }



      
      //========================================================= Item Properties
      //-----------------------------------------------------------------
      /// <summary>
      /// Get all properties of the specified item
      /// </summary>
      /// <param name="ItemHandle"></param>
      /// <param name="IDs"></param>
      /// <param name="Names"></param>
      /// <param name="Descriptions"></param>
      /// <param name="ValueTypes"></param>
      /// <returns></returns>
      new public int QueryProperties( int ItemHandle, 
         out int numProp, out int[] IDs, out string[] Names,
         out string[] Descriptions, out object[] Values ) 
      {
         //***************************************************  SAMPLE CODE
         if( Config.Items[ItemHandle].itemDefs.properties == null )     // item has no properties
         {
            numProp = 0 ;
            IDs = null;
            Names = null;
            Descriptions = null;
            Values = null ;
            return HRESULTS.S_FALSE ;
         }
         else
         {
            numProp = Config.Items[ItemHandle].itemDefs.properties.Length ;
            IDs = new int[numProp];
            Names = new string[numProp];
            Descriptions = new string[numProp];
            Values = new object[numProp];
            for( int i=0 ; i<numProp ; ++i )
            {
               IDs[i] = Config.Items[ItemHandle].itemDefs.properties[i].id;
               Names[i] = Config.Items[ItemHandle].itemDefs.properties[i].name;
               Descriptions[i] = Config.Items[ItemHandle].itemDefs.properties[i].descr;
               Values[i] = Config.Items[ItemHandle].itemDefs.properties[i].val;
            }
         }
         return HRESULTS.S_OK;
         //*************************************** END SAMPLE CODE
      }


      //-----------------------------------------------------------------
      /// <summary>
      /// Returns the value of the requested property for the requested item.
      /// </summary>
      /// <param name="ItemHandle"></param>
      /// <param name="IDs"></param>
      /// <param name="Values">null if property not defined</param>
      /// <returns>S_FALSE if item has no properties</returns>
      new public int GetPropertyValue( int ItemHandle, int pID, out object Value ) 
      {
         Value = null;
         //***************************************************  SAMPLE CODE
         if( Config.Items[ItemHandle].itemDefs.properties == null )     // item has no properties
         {
            return HRESULTS.S_FALSE ;
         }
         else
         {
            int len = Config.Items[ItemHandle].itemDefs.properties.Length ;
            for( int i=0 ; i<len ; ++i )     // search requested item property
            {
               if( pID == Config.Items[ItemHandle].itemDefs.properties[i].id )
                  Value = Config.Items[ItemHandle].itemDefs.properties[i].val ;
            }
         }
         //*************************************** END SAMPLE CODE
         return HRESULTS.S_OK;
      }


      //-----------------------------------------------------------------------------------------
      /// <summary>
      /// Returns the values of the requested properties for one item.       ( Professional edition only )
      /// This method is called in virtual browse mode for items that are not yet added to the generic cache.
      /// <param name="returnAll"></param>
      /// <param name="numProp"></param>
      /// <param name="propertyIDs"></param>
      /// <param name="Values"></param>
      /// <param name="errors"></param>
      /// <returns></returns>
      new public int GetItemProperties( string itemID, bool returnValues, bool returnAll, ref int numProp, 
                                        ref int[] propertyIDs,  out string[] propertyNames, out string[] propertyDescr, 
                                        out Object[] Values, out int[]errors  )
      {
         Values = null ;
         propertyNames = null;
         propertyDescr = null;
         errors = null ;
         int idx = Config.findIndexOf( itemID );
         if( idx < 0 )
            return HRESULTS.OPC_E_UNKNOWNITEMID ;

         ConfigBuilder.ItemData item = Config.Items[idx] ;
         int[] pids = propertyIDs ;
         if( returnAll )
         {
            numProp = item.itemDefs.properties.Length + 6 ;    // include the OPC standard properties
            pids = new int[ numProp ];
            for( int i=0 ; i<6 ; ++ i )
            {
               pids[i] = i + 1 ;
            }
            for( int i=6 ; i<numProp ; ++ i )
               pids[i] = item.itemDefs.properties[i-6].id ;
         }

         propertyNames = new string[ numProp ];
         propertyDescr = new string[ numProp ];
         errors = new int[ numProp ];

         for( int i=6 ; i<numProp ; ++ i )   // fill in property description
         {
            if( pids[i] <= 6 )   // standard property
            {
               propertyNames[i] = DescrStandardProperties[ pids[i] ] ;
               propertyDescr[i] = DescrStandardProperties[ pids[i] ] ;
            }
            else
            {
               propertyNames[i] = item.itemDefs.properties[i].name ;
               propertyDescr[i] = item.itemDefs.properties[i].descr ;
            }
         }

         if( returnValues )
         {
            Values = new object[ numProp ];

            for( int i=0 ; i<numProp ; ++ i )
            {
               propertyIDs[i] = pids[i] ;
               bool propFound = false ;
               if( pids[i] < 6 )    // standard property
                  propFound = true ;
               else
               {
                  for( int p=0 ; p<item.itemDefs.properties.Length ; ++p )
                     if( item.itemDefs.properties[i].id == pids[i] )
                     {
                        propFound = true ;
                        break;
                     }
               }
               if( propFound )
               {
                  if( returnValues )
                  {
                     switch( pids[i] )
                     {
                        case 1:           // data type
                           Values[i] = item.Value.GetType();
                           break ;
                        case 2:           // value
                           Values[i] = item.Value;
                           break ;
                        case 3:           // quality
                           Values[i] = (short)item.Quality;
                           break ;
                        case 4:           // timestamp
                           Values[i] = item.Timestamp;
                           break ;
                        case 5:           // access rights
                           Values[i] = (short)item.itemDefs.accRight;
                           break ;
                        case 6:           // scan rate
                           Values[i] = item.itemDefs.scanRate;
                           break ;
                        default:
                           Values[i] = item.itemDefs.properties[i].val ;
                           break;
                     }
                  }
               }
               else
                  errors[i] = HRESULTS.OPC_E_INVALID_PID ;
            }
         }


         return HRESULTS.E_NOTIMPL ;
      }





      //***************************************************  SAMPLE CODE

      //-----------------------------------------------------------------
      /// <summary>
      /// Sample method that shows how config definitions can be read from the 
      /// adaptation assembly config file xxxx.exe.config
      /// See the sample config file for the definition syntax.
      /// </summary>
      /// <param name="name">key of the configuration definition</param>
      /// <returns>Value as object</returns>
      public string ReadAppConfiguation( string name )
      {
         object val;
         try
         {
            AppSettingsReader ar = new AppSettingsReader();
            string str = "";
            val = ar.GetValue( name, str.GetType() );
            ar = null;
         }
         catch      // file or definition not found
         {
            val = "1" ;      // use default value
         }
         return val.ToString() ;
      }
      //*************************************** END SAMPLE CODE



      //***************************************************  SAMPLE CODE

      //----------------------------------------------------------------------------
      // DATA DEFINITIONS
      // Important: All data needs to be defined as STATIC.
      // This is important because this class is used in multiple instances.
      //----------------------------------------------------------------------------
      // Driver internal signal state buffer. The Item Handle is the array index.
      static Thread              myThread ;
      static ManualResetEvent    StopThread = null ;
      static int                 CountConnectedClients ;
      static public ConfigBuilder.ConfigLoader Config = null;

 

      //------------------------------------------------------------
      // Device simulation thread
      void UpdateThread() 
      {
         short RampInc = 5 ;
         double SineArg = 0.0 ;
         Random rand = new Random();
         for(;;)   // forever thread loop
         {   
            // increment readable items of type Int, Short, Float or Double
            for( int i=0 ; i<Config.Items.Length ; ++ i )
            {
               if( ( Config.Items[i] != null )
                  && ( Config.Items[i].name.StartsWith( "Simulated" ) )   )
               {
                  int ar = (int)Config.Items[i].itemDefs.accRight ;
                  if( (ar == (int)OPCAccess.READABLE) || (ar == (int)OPCAccess.READWRITEABLE) )
                  {
                     if( Config.Items[i].name.EndsWith( "Step" ) )  
                     {
                        short v = (short)Config.Items[i].Value ;
                        v += 5;
                        if( v > 100 )     v = 0 ;
                        Config.Items[i].Value = v  ;
                     }
                     else if( Config.Items[i].name.EndsWith( "Ramp" ) )  
                     {
                        short v = (short)Config.Items[i].Value ;
                        v += RampInc;
                        if( ( v >= 100 ) || ( v <= 0 ) )   RampInc *= -1;
                        Config.Items[i].Value = v  ;
                     }
                     else if( Config.Items[i].name.EndsWith( "Random" ) )  
                     {
                        Config.Items[i].Value = rand.Next( 1000000 )  ;
                     }
                     else if( Config.Items[i].name.EndsWith( "Sine" ) )  
                     {
                        SineArg += 0.25;
                        if( SineArg > 2.0 )     SineArg = 0.0 ;
                        Config.Items[i].Value = Math.Sin( SineArg * Math.PI ) ;
                     }
                     else if( Config.Items[i].name.EndsWith( "Signal" ) )  
                     {
                        bool v = (bool)Config.Items[i].Value ;
                        Config.Items[i].Value = ! v ;
                     }
                     // change timestamp of modified item
                     Config.Items[i].Timestamp = DateTime.UtcNow ;

                     // update server cache for this item
                     SetItemValue(Config.Items[i].handle, Config.Items[i].Value,
                        (short)Config.Items[i].Quality, DateTime.UtcNow);
                  }
               }
            }

            ////sample of a refresh on demand
            //int[] hnd;
            //GetRefreshNeed(0, 1000, out hnd);
            //for (int r = 0; r < hnd.Length; ++r)
            //{
            //   int ii = hnd[r];
            //   // update server cache for this item
            //   SetItemValue(Config.Items[ii].handle, Config.Items[ii].Value,
            //      (short)Config.Items[ii].Quality, DateTime.UtcNow);
            //}

            Thread.Sleep( 1000 ) ;   // ms

            if( StopThread != null )
            {
               StopThread.Set();
               return;               // terminate the thread
            }
         }
      }
      //*************************************** END SAMPLE CODE


   }


}
