//-------------------------------------------------------------------------------
// DANSrv OPC Server     Configuration Builder
// ==================
// Creates XML files with item definitions that can be used in the OPCDA.NET, XML-DA and
// OPC-DX customizable servers to create the items supported by the server.
//
// Content:
// Load the XML configuration file into an item definition array.
// The array is one-dimensional and the tree structure is only reflected in the fully qualified
// item name. The item attributes are set according to the item and branch definitions. The 
// branch default is used when the attribute is not defiend for the item.
//
//
// Copyright 2003-2012 Advosol Inc.   (www.advosol.com)
// All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;


namespace ConfigBuilder
{
   //============================================
   public class ItemData : ItemElement
   {
      public int           Error ;        // error creating or processing the item
      public OPCQuality    Quality ;
      public DateTime      Timestamp ;
      // add application specific data here
      public bool          isCreatedInServer ;
   }


   //============================================
   public class ConfigLoader
   {
      public ConfigLoader()
      {
      }
      
      //----------------------------------------------------------------------
      public ItemData[] Items ;                       // filled with item definition from XML file
      public bool       useBranchDefaults = true ;    // use branch defaults when creating item definitions
      public string     BranchSeparator ;


      //----------------------------------------------------------------------
      /// <summary>
      /// Loads the specified XML file and deserializes into an array structure, which is 
      /// stored in this object. Access methods return selected information in the 
      /// proper type for XML-DA server calls.
      /// </summary>
      /// <param name="fileName">Path name of the XML file, containing the required item 
      /// definition lists.</param>
      public void Load( string fileName )
      {
         XmlSerializer xSer = new XmlSerializer(typeof(DefinitionList)); 
         TextReader reader  = new StreamReader( fileName );
         if( reader == null )
            throw new Exception( "The TextReader could not be created for "+ fileName );
         DefinitionList cfgList = (DefinitionList)xSer.Deserialize( reader );
         reader.Close();
         ItemElement[] def = DefToItemArray( cfgList );
         Items = new ItemData[ def.Length ];
         for( int i=0 ; i<def.Length ; ++i )
         {
            Items[i] = new ItemData();
            Items[i].handle   = def[i].handle;
            Items[i].name     = def[i].name;
            Items[i].itemDefs = def[i].itemDefs;
            Items[i].Value    = def[i].Value;
            Items[i].Timestamp= DateTime.UtcNow ;
            if( def[i].itemDefs.qualitySpecified )
               Items[i].Quality  = def[i].itemDefs.quality ;
            else
               Items[i].Quality  = OPCQuality.UNCERTAIN ;
         } 
      }

      //----------------------------------------------------------------------
      /// <summary>
      /// Loads the specified XML file from the EXE file directory
      /// and deserializes into an array structure, which is 
      /// stored in this object. Access methods return selected information in the 
      /// proper type for XML-DA server calls.
      /// </summary>
      /// <param name="fileName">Path name of the XML file, containing the required item 
      /// definition lists.</param>
      public void LoadFromExeDir( string fileName )
      {
         string path = null ;
         DefinitionList cfgList;
         System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
         if( asm != null )
         {
            path = asm.Location ;
            int idx = path.LastIndexOf( "\\" );
            if( idx >= 0) path = path.Substring(0,idx+1);
            else   path = "";
         }
         else     // seems to be a web service or WCF service
         {
            System.Web.HttpContext hc = System.Web.HttpContext.Current;
            if (hc != null)   // asmx web service
               path = hc.Server.MapPath(".") + "\\";   // web dir
            else    // must be WCF service
            {
#if ! VS2003
               path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
#endif
            }
         }
         XmlSerializer xSer = new XmlSerializer(typeof(DefinitionList));
         TextReader reader  = new StreamReader( path + fileName );
         if( reader == null )
         {
            if( NSPlugin.GenericServer.TraceLog != null )
               NSPlugin.LogFile.Write( "The TextReader could not be created for "+ fileName  );
            throw new Exception( "The TextReader could not be created for "+ fileName );
         }
         try
         {
            cfgList = (DefinitionList)xSer.Deserialize( reader );
         }
         catch( Exception ex )
         {
            if( NSPlugin.GenericServer.TraceLog != null )
               NSPlugin.LogFile.Write( "Exception at Deserialize: " + ex.Message );
            throw new Exception( "Exception at Deserialize: " + ex.Message );
         }
         reader.Close();
         ItemElement[] def = DefToItemArray( cfgList );
         Items = new ItemData[ def.Length ];
         for( int i=0 ; i<def.Length ; ++i )
         {
            Items[i] = new ItemData();
            Items[i].handle   = def[i].handle;
            Items[i].name     = def[i].name;
            Items[i].itemDefs = def[i].itemDefs;
            Items[i].Value    = def[i].Value;
            Items[i].Timestamp= DateTime.UtcNow ;
            if( def[i].itemDefs.qualitySpecified )
               Items[i].Quality  = def[i].itemDefs.quality ;
            else
               Items[i].Quality  = OPCQuality.UNCERTAIN ;
         } 
      }


      /// <summary>
      /// Loads an XML file that is embedded in the application EXE and deserializes into an 
      /// array structure, which is stored in this object. 
      /// Access methods return selected information in the proper type for XML-DA server calls.
      /// To embed the file in the application EXE, the XML file needs to be included in the
      /// Visual Studio project, and the build action for this file has to be set to 'Embedded Resource'.
      /// </summary>
      /// <param name="namespaceFileName">Name of the embedded XML file, containing the required item 
      /// definition lists. Specify the file name exactly as it is shown in the solution File list and
      /// precede it with the Namespace of the defining assembly.
      /// E.g. NSPlugin.Sample.xml</param>
      public void LoadEmbedded( string namespaceFileName )
      {
         Assembly myAsmb = System.Reflection.Assembly.GetCallingAssembly();
         Stream stream = myAsmb.GetManifestResourceStream( namespaceFileName );
         TextReader reader  = new StreamReader( stream );
         if( reader == null )
         {
            if( NSPlugin.GenericServer.TraceLog != null )
               NSPlugin.LogFile.Write( "The TextReader could not be created for "+ namespaceFileName  );
            throw new Exception( "The TextReader could not be created for "+ namespaceFileName );
         }
         XmlSerializer xSer = new XmlSerializer(typeof(DefinitionList));
         DefinitionList cfgList = (DefinitionList)xSer.Deserialize( reader );
         reader.Close();
         ItemElement[] def  = DefToItemArray( cfgList );
         Items = new ItemData[ def.Length ];
         for( int i=0 ; i<def.Length ; ++i )
         {
            Items[i] = new ItemData();
            Items[i].handle   = def[i].handle;
            Items[i].name     = def[i].name;
            Items[i].itemDefs = def[i].itemDefs;
            Items[i].Value    = def[i].Value;
            Items[i].Timestamp= DateTime.UtcNow ;
            if( def[i].itemDefs.qualitySpecified )
               Items[i].Quality  = def[i].itemDefs.quality ;
            else
               Items[i].Quality  = OPCQuality.UNCERTAIN ;
         }
      }

   
      /// <summary>
      /// Loads the XML text from the specified stream and deserializes into an array structure, 
      /// which is stored in this object. Access methods return selected information in the 
      /// proper type for XML-DA server calls. This method overload is provided to give the 
      /// flexibility to read the XML text from anywhere, e.g. a memory buffer. The user has to
      /// create the Stream appropriate for the source.
      /// </summary>
      /// <param name="stream">System.IO.Stream to read the XML text from.</param>
      public void Load( Stream stream )
      {
         TextReader reader  = new StreamReader( stream );
         if( reader == null )
            throw new Exception( "The TextReader could not be created" );
         XmlSerializer xSer = new XmlSerializer(typeof(DefinitionList));
         DefinitionList cfgList = (DefinitionList)xSer.Deserialize( reader );
         reader.Close();
         ItemElement[] def  = DefToItemArray( cfgList );
         Items = new ItemData[ def.Length ];
         for( int i=0 ; i<def.Length ; ++i )
         {
            Items[i] = new ItemData();
            Items[i].handle   = def[i].handle;
            Items[i].name     = def[i].name;
            Items[i].itemDefs = def[i].itemDefs;
            Items[i].Value    = def[i].Value;
            Items[i].Timestamp= DateTime.UtcNow ;
            if( def[i].itemDefs.qualitySpecified )
               Items[i].Quality  = def[i].itemDefs.quality ;
            else
               Items[i].Quality  = OPCQuality.UNCERTAIN ;
         } 
      }



      //----------------------------------------------------------------------------
      // Convert the configuration into an item definition array.
      // The item parameters are constructed from the item and branch definitions. If a
      // parameter is not defined with the item, then the branch definition is used.
      public ItemElement[] DefToItemArray( DefinitionList cfgList )
      {
         ArrayList itemList = new ArrayList();    // of ItemElement
         BranchSeparator = cfgList.BranchSeperatorChar.ToString();
         handleBranch( cfgList.DefinitionsRoot, "", itemList ) ;

         return (ItemElement[])itemList.ToArray( typeof(ItemElement) );
      }


      // recursively handles the items in the branch
      private void handleBranch( BranchElement be, string path, ArrayList itemList )
      {
         if( path != "" )
            path += BranchSeparator ;

         foreach( ItemElement ie in be.items )
         {
            ConfigDefs icfg = ie.itemDefs ;
            if( icfg.activeDef )       // item is defined active
            {
               if( useBranchDefaults )
               {
                  ie.name = path + ie.name ;
                  ConfigDefs bcfg = be.branchDefs ;
                  if( ! icfg.accRightSpecified && bcfg.accRightSpecified )
                  {
                     icfg.accRight = bcfg.accRight ;
                     icfg.accRightSpecified = true ;
                  }
                  if( ! icfg.dataTypeSpecified && bcfg.dataTypeSpecified )
                  {
                     icfg.dataType = bcfg.dataType ;
                     icfg.dataTypeSpecified = true ;
                  }
                  if( ! icfg.deviceAddrSpecified && bcfg.deviceAddrSpecified )
                  {
                     icfg.deviceAddr = bcfg.deviceAddr ;
                     icfg.deviceAddrSpecified = true ;
                  }
                  if( ! icfg.deviceIDSpecified && bcfg.deviceIDSpecified )
                  {
                     icfg.deviceID = bcfg.deviceID ;
                     icfg.deviceIDSpecified = true ;
                  }
                  if( ! icfg.deviceSubAddrSpecified && bcfg.deviceSubAddrSpecified )
                  {
                     icfg.deviceSubAddr = bcfg.deviceSubAddr ;
                     icfg.deviceSubAddrSpecified = true ;
                  }
                  if( ! icfg.qualitySpecified && bcfg.qualitySpecified )
                  {
                     icfg.quality = bcfg.quality ;
                     icfg.qualitySpecified = true ;
                  }
                  if( ! icfg.scanRateSpecified && bcfg.scanRateSpecified )
                  {
                     icfg.scanRate = bcfg.scanRate ;
                     icfg.scanRateSpecified = true ;
                  }
                  if( ! icfg.signalTypeSpecified && bcfg.signalTypeSpecified )
                  {
                     icfg.signalType = bcfg.signalType ;
                     icfg.signalTypeSpecified = true ;
                  }
                  if( ! icfg.user1Specified && bcfg.user1Specified )
                  {
                     icfg.user1 = bcfg.user1 ;
                     icfg.user1Specified = true ;
                  }
                  if( ! icfg.user2Specified && bcfg.user2Specified )
                  {
                     icfg.user2 = bcfg.user2 ;
                     icfg.user2Specified = true ;
                  }
                  if( (icfg.properties==null) && (bcfg.properties!=null) )
                     icfg.properties = bcfg.properties ;
               }
               itemList.Add( ie );
            }
         }
         
         foreach( BranchElement sbe in be.subBranches )
         {
            if( useBranchDefaults )    // use parent branch definitions as default
            {
               ConfigDefs bcfg = be.branchDefs ;
               ConfigDefs sbcfg = sbe.branchDefs ;
               if( ! sbcfg.accRightSpecified && bcfg.accRightSpecified )
               {
                  sbcfg.accRight = bcfg.accRight ;
                  sbcfg.accRightSpecified = true ;
               }
               if( ! sbcfg.dataTypeSpecified && bcfg.dataTypeSpecified )
               {
                  sbcfg.dataType = bcfg.dataType ;
                  sbcfg.dataTypeSpecified = true ;
               }
               if( ! sbcfg.deviceAddrSpecified && bcfg.deviceAddrSpecified )
               {
                  sbcfg.deviceAddr = bcfg.deviceAddr ;
                  sbcfg.deviceAddrSpecified = true ;
               }
               if( ! sbcfg.deviceIDSpecified && bcfg.deviceIDSpecified )
               {
                  sbcfg.deviceID = bcfg.deviceID ;
                  sbcfg.deviceIDSpecified = true ;
               }
               if( ! sbcfg.deviceSubAddrSpecified && bcfg.deviceSubAddrSpecified )
               {
                  sbcfg.deviceSubAddr = bcfg.deviceSubAddr ;
                  sbcfg.deviceSubAddrSpecified = true ;
               }
               if( ! sbcfg.qualitySpecified && bcfg.qualitySpecified )
               {
                  sbcfg.quality = bcfg.quality ;
                  sbcfg.qualitySpecified = true ;
               }
               if( ! sbcfg.scanRateSpecified && bcfg.scanRateSpecified )
               {
                  sbcfg.scanRate = bcfg.scanRate ;
                  sbcfg.scanRateSpecified = true ;
               }
               if( ! sbcfg.signalTypeSpecified && bcfg.signalTypeSpecified )
               {
                  sbcfg.signalType = bcfg.signalType ;
                  sbcfg.signalTypeSpecified = true ;
               }
               if( ! sbcfg.user1Specified && bcfg.user1Specified )
               {
                  sbcfg.user1 = bcfg.user1 ;
                  sbcfg.user1Specified = true ;
               }
               if( ! sbcfg.user2Specified && bcfg.user2Specified )
               {
                  sbcfg.user2 = bcfg.user2 ;
                  sbcfg.user2Specified = true ;
               }
               if( (sbcfg.properties==null) && (bcfg.properties!=null) )
                  sbcfg.properties = bcfg.properties ;
            }

            handleBranch( sbe, path + sbe.name, itemList );
         }

      }


      //-----------------------------------------------------------------------------------
      public int findIndexOf( string name )
      {
         for( int i=0 ; i<Items.Length ; ++i )
            if( Items[i].name == name )
               return i ;
         return -1 ;
      }

      //-----------------------------------------------------------------------------------
      public int findIndexOf( int handle )
      {
         for( int i=0 ; i<Items.Length ; ++i )
            if( Items[i].handle == handle )
               return i ;
         return -1 ;
      }

      //-----------------------------------------------------------------------------------
      /// <summary>
      /// Create all configured items
      /// </summary>
      /// <param name="createInServer"></param>
      /// <returns></returns>
      public int CreateServerItems( NSPlugin.AddItem createInServer )
      {
         int rtc = 0 ;
         foreach( ItemData item in Items )
         {
            int sRate = -1 ;                       // assume undefined
            if( item.itemDefs.scanRateSpecified )
               sRate = item.itemDefs.scanRate ;

            if( item.Value == null )               // no init value specified
            {
               if( item.itemDefs.dataTypeSpecified )
               {
                  Type vt = drvtypes.Type.GetSystemType( item.itemDefs.dataType );
                  if (vt != null )
                  {
                  if( vt.IsArray )
                  {
                     Type vte =  vt.GetElementType() ;
                     object ev = CreateObject( vte );
                     ArrayList al = new ArrayList();
                     al.Add( ev ) ;
                     item.Value = al.ToArray(vte);
                  }
                  else
                     item.Value = CreateObject( vt );
               }
               else
                  item.Value = "" ;   // init as string type
            }
               else
                  item.Value = "";   // init as string type
            }

            int rc = createInServer( item.handle, item.name, (int)item.itemDefs.accRight,
                                     item.Value, (short)OPCQuality.GOOD, DateTime.UtcNow, sRate );
            if( rc != 0 )
            {
               item.Error = rc ;
               rc = 1 ;
            }

         }
         return rtc ;
      }


      private object CreateObject( Type typ )
      {
         if( typ == typeof(object) )
            return new object();
         if( typ == typeof(sbyte) )
            return new sbyte();
         if( typ == typeof(byte) )
            return new byte();
         if( typ == typeof(short) )
            return new short();
         if( typ == typeof(ushort) )
            return new ushort();
         if( typ == typeof(int) )
            return new int();
         if( typ == typeof(uint) )
            return new uint();
         if( typ == typeof(long) )
            return new long();
         if( typ == typeof(ulong) )
            return new ulong();
         if( typ == typeof(float) )
            return new float();
         if( typ == typeof(double) )
            return new double();
         if( typ == typeof(decimal) )
            return new decimal();
         if( typ == typeof(DateTime) )
            return new DateTime();
         if( typ == typeof(TimeSpan) )
            return new TimeSpan();
         if( typ == typeof(bool) )
            return new bool();
         if( typ == typeof(string) )
            return "";
         if( typ == typeof(XmlQualifiedName) )
            return new XmlQualifiedName();
         return null ;
      }

   }

}



