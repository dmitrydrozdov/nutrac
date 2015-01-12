//-------------------------------------------------------------------------------
// OPCDA.NET Server     Configuration Builder
// ================
// Creates XML files with item definitions that can be used in the OPCDA.NET, XML-DA and
// OPC-DX customizable servers to create the items supported by the server.
//
// Content:
// Data and Constant definition classes
// Definition of the OPC recommended item properties.
//
// Author:   KHa    Sep-2003
//
// Copyright 2003   Advosol Inc.   (www.advosol.com)
// All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
using System;
using System.Collections ;
using System.Xml;


namespace ConfigBuilder
{

   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.SByte[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(short[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.UInt16[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(int[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.UInt32[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(long[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.UInt64[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.Single[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.Decimal[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.Double[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(bool[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(string[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(System.DateTime[]))]
   [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
   //===========================================================================
   // XML file data classes
   //---------------------------------------------------------------------------
   public class DefinitionList
   {
      public DefinitionList()
      {}
      public char             BranchSeperatorChar ;
      public BranchElement    DefinitionsRoot ;
   }

   
   public class BranchElement
   {
      public BranchElement()       // needed for XML serializer
      {}
      public string           name ;
      public ConfigDefs       branchDefs ;   // branch defaults

      [System.Xml.Serialization.XmlArray(IsNullable = true)]
      public BranchElement[]  subBranches;

      [System.Xml.Serialization.XmlArray(IsNullable = true)]
      public ItemElement[]    items ;
   }

   //---------------------------------------------
   public class ItemElement
   {
      public ItemElement()       // needed for XML serializer
      {}
      public string        name ;         // fully qualified name (simple name within ConfigBuilder tool)
      public int           handle ;       // unique item handle
      public ConfigDefs    itemDefs ;     // configuration diefinitions
      public object        Value ;        // initial or current value
    }


   //-----------------------------------------------------------
   // definitions for one item.
   // the definiton is hierarchical. The branch level definitions are used
   // when there is no item level definition specified.
   public class ConfigDefs
   {
      public ConfigDefs()   // needed for XML serializer
      {}
      public bool          activeDef ;

      public OPCAccess     accRight ;
      public bool          accRightSpecified;
      public XmlQualifiedName      dataType ;
      public bool          dataTypeSpecified ;

      //[System.Xml.Serialization.XmlArray(IsNullable = true)]
      public PropertyDef[] properties ;
      public OPCQuality    quality ;
      public bool          qualitySpecified ;
      public SignalType    signalType ;
      public bool          signalTypeSpecified ;
      public int           scanRate ;
      public bool          scanRateSpecified ;
      public string        deviceID ;
      public bool          deviceIDSpecified ;
      public string        deviceAddr ;
      public bool          deviceAddrSpecified ;
      public string        deviceSubAddr ;
      public bool          deviceSubAddrSpecified ;
      public string        user1 ;
      public bool          user1Specified ;
      public string        user2 ;
      public bool          user2Specified ;
   }



   
   //===========================================================================
   // ConfigBuilder Tool work data class
   //---------------------------------------------------------------------------
   public class BranchDef
   {
      public string           name ;
      public ConfigDefs       branchDefs ;
   }




   //======================================================================
   // These enumerations match the OPC specifications or the server EXE file.
   // -----------

   // OPC defined signal read/write definitions
   public enum OPCAccess : short 
   {
      UNKNOWN        = 0,
      READABLE       = 1,
      WRITEABLE      = 2,
      READWRITEABLE  = 3
   }


   public class OPCAccessSel
   {
      public static string[] All = { OPCAccess.UNKNOWN.ToString(),
                              OPCAccess.READABLE.ToString(),
                              OPCAccess.WRITEABLE.ToString(),
                              OPCAccess.READWRITEABLE.ToString() };

      public static OPCAccess getEnum( int selIdx )
      {
         switch( selIdx ) 
         {
            case 0 :    return OPCAccess.UNKNOWN ;
            case 1 :    return OPCAccess.READABLE ;
            case 2 :    return OPCAccess.WRITEABLE ;
            case 3 :    return OPCAccess.READWRITEABLE;
            default:    return OPCAccess.UNKNOWN ;
         }
      }
   }



   //======================================================================
   // Driver internal signal (item) types
   public enum SignalType  : short 
   {
      INP = 1,
      OUT = 2,
      INOUT = 3,
      INTERN = 7
   }

   public class SignalTypeSel
   {
      public static string[] All = { SignalType.INP.ToString(),
                                      SignalType.OUT.ToString(),
                                      SignalType.INOUT.ToString(),
                                      SignalType.INTERN.ToString() };

      public static SignalType getEnum( int selIdx )
      {
         switch( selIdx ) 
         {
            case 0 :    return SignalType.INP ;
            case 1 :    return SignalType.OUT ;
            case 2 :    return SignalType.INOUT ;
            case 3 :    return SignalType.INTERN;
            default:    return SignalType.INTERN ;
         }
      }
   }



   

   // OPC defined item quality values
   public enum OPCQuality : short 
   {
      BAD            = 0,
      UNCERTAIN      = 0x40,
      GOOD            = 0xC0,
      CONFIG_ERROR   = 0x4,
      NOT_CONNECTED   = 0x8,
      DEVICE_FAILURE   = 0xC,
      SENSOR_FAILURE   = 0x10,
      LAST_KNOWN      = 0x14,
      COMM_FAILURE   = 0x18,
      OUT_OF_SERVICE   = 0x1C,
      LAST_USABLE      = 0x44,
      SENSOR_CAL      = 0x50,
      EGU_EXCEEDED   = 0x54,
      SUB_NORMAL      = 0x58,
      LOCAL_OVERRIDE   = 0xD8
   }

   public class OPCQualitySel
   {
      public static string[] All = {
                                      OPCQuality.BAD.ToString(),
                                      OPCQuality.UNCERTAIN.ToString(),
                                      OPCQuality.GOOD.ToString(),
                                      OPCQuality.CONFIG_ERROR.ToString(),
                                      OPCQuality.NOT_CONNECTED.ToString(),
                                      OPCQuality.DEVICE_FAILURE.ToString(),
                                      OPCQuality.SENSOR_FAILURE.ToString(),
                                      OPCQuality.LAST_KNOWN.ToString(),
                                      OPCQuality.COMM_FAILURE.ToString(),
                                      OPCQuality.OUT_OF_SERVICE.ToString(),
                                      OPCQuality.LAST_USABLE.ToString(),
                                      OPCQuality.SENSOR_CAL.ToString(),
                                      OPCQuality.EGU_EXCEEDED.ToString(),
                                      OPCQuality.SUB_NORMAL.ToString(),
                                      OPCQuality.LOCAL_OVERRIDE.ToString()
                                   };

      public static OPCQuality getEnum( int selIdx )
      {
         switch( selIdx ) 
         {
            case 0 :  return OPCQuality.BAD ;
            case 1 :  return OPCQuality.UNCERTAIN ;
            case 2 :  return OPCQuality.GOOD ;
            case 3 :  return OPCQuality.CONFIG_ERROR ;
            case 4 :  return OPCQuality.NOT_CONNECTED ;
            case 5 :  return OPCQuality.DEVICE_FAILURE ;
            case 6 :  return OPCQuality.SENSOR_FAILURE ;
            case 7 :  return OPCQuality.LAST_KNOWN ;
            case 8 :  return OPCQuality.COMM_FAILURE ;
            case 9 :  return OPCQuality.OUT_OF_SERVICE ;
            case 10 : return OPCQuality.LAST_USABLE ;
            case 11 : return OPCQuality.SENSOR_CAL ;
            case 12 : return OPCQuality.EGU_EXCEEDED ;
            case 13 : return OPCQuality.SUB_NORMAL ;
            case 14 : return OPCQuality.LOCAL_OVERRIDE ;
            default : return OPCQuality.BAD ;
         }
      }
   }

   public class PropertyDef
   {
      public PropertyDef()       // needed for XML serializer
      {}
      public int     id;
      public string  name ;
      public XmlQualifiedName    dataType ;
      public string  descr ;
      public object  val ;
   }


   public class PropertyDefWork : PropertyDef
   {
      public bool    isActive ;

      public PropertyDefWork( int Id, string nm, Type dt, string dscr, object v, bool active )
      {
         id = Id;
         name = nm;
         dataType = drvtypes.Type.GetQName(dt) ;
         descr = dscr ;
         val = v ;
         isActive = active ;
      }

   }

   public class PropertyDefs
   {
      public static PropertyDef[] recommendedProps = 
      {
         new PropertyDefWork( 100, "engineeringUnits", typeof(string) , "EU Units", null,false),
         new PropertyDefWork( 101, "description", typeof(string) , "Item Description", null,false ),
         new PropertyDefWork( 102, "highEU", typeof(double) , "High EU", null,false  ),
         new PropertyDefWork( 103, "lowEU", typeof(double) , "Low EU", null,false  ),
         new PropertyDefWork( 104, "highIR", typeof(double) , "High Instrument Range", null,false ), 
         new PropertyDefWork( 105, "loeIR", typeof(double) , "Low Instrument Range", null,false  ),
         new PropertyDefWork( 106, "closeLabel", typeof(string) , "Contact Close Label", null,false ),
         new PropertyDefWork( 107, "openLabel", typeof(string) , "Contact Open Label", null,false  ),
         new PropertyDefWork( 108, "timeZone", typeof(int) ,    "Item Timezone", null,false ),
         new PropertyDefWork( 200, "", typeof(string) , "Default Display", null,false  ),
         new PropertyDefWork( 201, "", typeof(int) ,    "Current Foreground Color", null,false ),
         new PropertyDefWork( 202, "", typeof(int) ,    "Current Background Color", null,false  ),
         new PropertyDefWork( 203, "", typeof(bool) ,   "Current Blink", null,false  ),
         new PropertyDefWork( 204, "", typeof(string) , "BMP File", null,false),
         new PropertyDefWork( 205, "", typeof(string) , "Sound File", null,false ),
         new PropertyDefWork( 206, "", typeof(string) , "HTML File", null,false  ),
         new PropertyDefWork( 207, "", typeof(string) , "AVI File", null,false  ),
         new PropertyDefWork( 300, "", typeof(string) , "Condition Status", null,false  ),
         new PropertyDefWork( 301, "", typeof(string) , "Alarm Quick Help", null,false ),
         new PropertyDefWork( 302, "", typeof(string[]), "Alarm Area List", null,false  ),
         new PropertyDefWork( 303, "", typeof(string) , "Primary Alarm Area", null,false  ),
         new PropertyDefWork( 304, "", typeof(string) , "Condition Logic", null,false  ),
         new PropertyDefWork( 305, "", typeof(string) , "Limit Exceeded", null,false  ),
         new PropertyDefWork( 306, "", typeof(double) , "Deadband", null,false ),
         new PropertyDefWork( 307, "", typeof(double) , "HiHi Limit", null,false ),
         new PropertyDefWork( 308, "", typeof(double) , "Hi Limit", null,false ),
         new PropertyDefWork( 309, "", typeof(double) , "Lo Limit", null,false ),
         new PropertyDefWork( 310, "", typeof(double) , "LoLo Limit", null,false ),
         new PropertyDefWork( 311, "", typeof(double) , "Rate of Change Limit", null,false ),
         new PropertyDefWork( 312, "", typeof(double) , "Deviation Limit", null,false ) 
      };


      public static string[] getPropertyDescrList()
      {
         string[] names = new string[ recommendedProps.Length ];
         for( int i=0 ; i<recommendedProps.Length ; ++i )
            names[i] = recommendedProps[i].descr ;

         return names ;
      }

      public static PropertyDefWork GetProp( int id )
      {
         foreach( PropertyDefWork pd in recommendedProps )
            if( pd.id == id )
               return pd ;
         return null;
      }

      public static PropertyDefWork GetProp( string descr )
      {
         foreach( PropertyDefWork pd in recommendedProps )
            if( pd.descr == descr )
               return pd ;
         return null;
      }
   }

}
