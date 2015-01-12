//============================================================================
// DANSrv Customizable OPC DA Server
// ---------------------------------
//
// Generic OPC server part interface definitions.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//
// Copyright (C) 2003-07 Advosol Inc.    (www.advosol.com)
// All rights reserved.
//-------------------------------------------------------------------------
using System;
using System.Xml;
using System.Collections;

namespace NSPlugin
{

     //***************************************************  SAMPLE CODE
   //============================================
   // 
   public class ConfigData
   {
      public ItemData[]    Items ;
      public ItemTree      Branches;
      public char          NameSeperator;

      //---------------------------------------- constructor
      public ConfigData( int numItems, char sep )
      {
         Items = new ItemData[ numItems ] ;
         NameSeperator = sep;
         Branches = new ItemTree(sep);
      }

      //-----------------------------------------------------------------------------------
      // create a new item definition that can be assigned to an element of the 
      // item definition array
      public ItemData NewItem( string Name, int appHandle, OPCAccess aMode, object val, OPCQuality qual )
      {
         ItemData ni = new ItemData();
         ni.itemDefs = new ConfigDefs();
         ni.name = Name ;
         ni.handle = appHandle ;
         ni.itemDefs.accRight = aMode;
         ni.Value = val ;
         ni.Quality = qual ;
         ni.Timestamp = DateTime.UtcNow ;
         Branches.AddNode(Name);
         return ni ;
      }


      //-----------------------------------------------------------------------------------
      // find the array index of the item with the specified fully qualified name
      public int findIndexOf( string name )
      {
         for( int i=0 ; i<Items.Length ; ++i )
            if( Items[i].name == name )
               return i ;
         return -1 ;
      }
   }



   //============================================
   // sample item definition class
   public class ItemData : ItemElement
   {
      public int           err ;        // error creating or processing the item
      public OPCQuality    Quality ;
      public DateTime      Timestamp ;
      // add application specific data here
      public bool          isCreatedInServer;
   }

   //---------------------------------------------
   // item definition class as used in the ConfigBuilder class and Tool
   public class ItemElement
   {
      public ItemElement()                // needed for XML serializer
      {}
      public string        name ;         // fully qualified name (simple name within ConfigBuilder tool)
      public int           handle ;       // unique item handle
      public ConfigDefs    itemDefs ;     // configuration definitions
      public object        Value ;        // initial or current value
   }


   //-----------------------------------------------------------
   // Definitions for one item
   // The definiton is hierarchical. The branch level definitions are used
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
      public PropertyDefinition[] properties;
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

   // Driver internal signal (item) types
   public enum SignalType  : short 
   {
      INP = 1,
      OUT = 2,
      INOUT = 3,
      INTERN = 7
   }


   //-------------------------------------------------------
   // Item property EU Types
   public enum EUType
   {
      NoEnum =0,           // regular OPC item
      Analog =1,           // analog value item with deadband and limits
      Enumerated =2        // enumeration string are defined in EUInfo
   }

      
   //--------------------------------------------------------
   // Item Property Definition
   public class PropertyDefinition
   {
      public int     id;
      public string  name ;
      public XmlQualifiedName    dataType ;
      public string  descr ;
      public object  val ;

      public PropertyDefinition(int ID, string Name, string Descr, object Val)
      {
         id = ID;
         name = Name;
         descr = Descr;
         val = Val;
      }
   }



   //======================================
   // Items in a tree structure 
   public class ItemTree
   {
      public string nodeName;
      public string pathName;
      public bool isItem;
      public ArrayList childs ;     // ItemTree list
      public char NameSeperator;
      private ItemTree parent;


      public ItemTree( char sep )
      {
         nodeName = "";
         pathName = "";
         childs = new ArrayList();
         NameSeperator = sep;
      }


      public ItemTree GetParent()
      {
         return parent;
      }


      public ItemTree FindChild(string name)
      {
         foreach (ItemTree ch in childs)
         {
            if (ch.nodeName == name)
               return ch;
         }
         return null ; //not found
      }


      public ItemTree FindNode(string path)
      {
         string[] nodeNames = path.Split(new char[] { NameSeperator });
         ItemTree br = this;     // root
         for (int i = 0; i < nodeNames.Length; ++i)      // nodes
         {
            ItemTree ch = br.FindChild(nodeNames[i]);
            if (ch == null)     // not found
            {
               return null;
            }
            br = ch;    // child as current branch
         }
         return br;
      }


      public void AddNode( string path )
      {
         string[] nodeNames = path.Split(new char[] { NameSeperator });
         ItemTree br = this;     // root
         for( int i=0; i<nodeNames.Length-1 ; ++i )      // branches
         {
            ItemTree ch = br.FindChild(nodeNames[i]);
            if (ch == null)     // not found
            {
               ch = new ItemTree(NameSeperator);
               ch.nodeName = nodeNames[i];
               if (br.pathName == "")
                  ch.pathName = nodeNames[i];
               else
                  ch.pathName = br.pathName + NameSeperator + nodeNames[i];
               ch.parent = br;
               br.childs.Add(ch);
            }
            br = ch;    // child as current branch
         }
         ItemTree ich = new ItemTree(NameSeperator);
         ich.nodeName = nodeNames[nodeNames.Length-1];
         ich.pathName = path;
         ich.parent = br;
         ich.isItem = true;
         br.childs.Add(ich);
      }
   }

   //*************************************** END SAMPLE CODE
    

}
