//-------------------------------------------------------------------------------
// DANSrv OPC Server     Configuration Builder
// =================
// Creates XML files with item definitions that can be used in the OPC DA DANSrv, XML-DA and 
// OPC-DX customizable servers to create the items supported by the server.
//
// Content:
// Defines well-known OPC data type names and system type, VARIANT type mappings.
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
//
// (c) Copyright 2002-2003 The OPC Foundation
// ALL RIGHTS RESERVED.
//
// DISCLAIMER:
//  This code is provided by the OPC Foundation solely to assist in 
//  understanding and use of the appropriate OPC Specification(s) and may be 
//  used as set forth in the License Grant section of the OPC Specification.
//  This code is provided as-is and without warranty or support of any sort
//  and is subject to the Warranty and Liability Disclaimers which appear
//  in the printed OPC Specification.
//-------------------------------------------------------------------------------

using System;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace drvtypes
{
   // defines well known namespaces
   public class Namespace
   {
      public const string XML_SCHEMA          = "http://www.w3.org/2001/XMLSchema";
      public const string XML_SCHEMA_INSTANCE = "http://www.w3.org/2001/XMLSchema-instance";
      public const string OPC                 = "http://schemas.opcfoundation.org/OPC/";
      public const string OPC_SAMPLE          = "http://schemas.opcfoundation.org/OPCSample/";
      public const string OPC_DATA_ACCESS     = "http://opcfoundation.org/webservices/XMLDA/10/";
      public const string OPC_DATA_EXCHANGE   = "http://opcfoundation.org/webservices/DX/10/";
   }

   // defines well known data type names, variant types and system types
   public class Type
   {
      // fully qualified names for all known data types
      public static readonly XmlQualifiedName ANY_TYPE       = new XmlQualifiedName("anyType",              Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName SBYTE          = new XmlQualifiedName("byte",                 Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName BYTE           = new XmlQualifiedName("unsignedByte",         Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName SHORT          = new XmlQualifiedName("short",                Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName USHORT         = new XmlQualifiedName("unsignedShort",        Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName INT            = new XmlQualifiedName("int",                  Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName UINT           = new XmlQualifiedName("unsignedInt",          Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName LONG           = new XmlQualifiedName("long",                 Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName ULONG          = new XmlQualifiedName("unsignedLong",         Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName FLOAT          = new XmlQualifiedName("float",                Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName DOUBLE         = new XmlQualifiedName("double",               Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName DECIMAL        = new XmlQualifiedName("decimal",              Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName DATETIME       = new XmlQualifiedName("dateTime",             Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName TIME           = new XmlQualifiedName("time",                 Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName DATE           = new XmlQualifiedName("date",                 Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName DURATION       = new XmlQualifiedName("duration",             Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName BOOLEAN        = new XmlQualifiedName("boolean",              Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName STRING         = new XmlQualifiedName("string",               Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName QNAME          = new XmlQualifiedName("QName",                Namespace.XML_SCHEMA);
      public static readonly XmlQualifiedName BINARY         = new XmlQualifiedName("base64Binary",         Namespace.XML_SCHEMA);   
      public static readonly XmlQualifiedName ARRAY_SHORT    = new XmlQualifiedName("ArrayOfShort",         Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_USHORT   = new XmlQualifiedName("ArrayOfUnsignedShort", Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_INT      = new XmlQualifiedName("ArrayOfInt",           Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_UINT     = new XmlQualifiedName("ArrayOfUnsignedInt",   Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_LONG     = new XmlQualifiedName("ArrayOfLong",          Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_ULONG    = new XmlQualifiedName("ArrayOfUnsignedLong",  Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_FLOAT    = new XmlQualifiedName("ArrayOfFloat",         Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_DOUBLE   = new XmlQualifiedName("ArrayOfDouble",        Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_DECIMAL  = new XmlQualifiedName("ArrayOfDecimal",       Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_DATETIME = new XmlQualifiedName("ArrayOfDateTime",      Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_BOOLEAN  = new XmlQualifiedName("ArrayOfBoolean",       Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_STRING   = new XmlQualifiedName("ArrayOfString",        Namespace.OPC_DATA_ACCESS);
      public static readonly XmlQualifiedName ARRAY_ANY_TYPE = new XmlQualifiedName("ArrayOfAnyType",       Namespace.OPC_DATA_ACCESS);

      // variant types for all known data types.
      public class VarType
      {
         public static readonly VarEnum ANY_TYPE       = VarEnum.VT_VARIANT;
         public static readonly VarEnum SBYTE          = VarEnum.VT_I1;
         public static readonly VarEnum BYTE           = VarEnum.VT_UI1;
         public static readonly VarEnum SHORT          = VarEnum.VT_I2;
         public static readonly VarEnum USHORT         = VarEnum.VT_UI2;
         public static readonly VarEnum INT            = VarEnum.VT_I4;
         public static readonly VarEnum UINT           = VarEnum.VT_UI4;
         public static readonly VarEnum LONG           = VarEnum.VT_I8;
         public static readonly VarEnum ULONG          = VarEnum.VT_UI8;
         public static readonly VarEnum FLOAT          = VarEnum.VT_R4;
         public static readonly VarEnum DOUBLE         = VarEnum.VT_R8;
         public static readonly VarEnum DECIMAL        = VarEnum.VT_CY;
         public static readonly VarEnum DATETIME       = VarEnum.VT_DATE;
         public static readonly VarEnum TIME           = VarEnum.VT_DATE;
         public static readonly VarEnum DATE           = VarEnum.VT_DATE;
         public static readonly VarEnum DURATION       = VarEnum.VT_I4;
         public static readonly VarEnum BOOLEAN        = VarEnum.VT_BOOL;
         public static readonly VarEnum STRING         = VarEnum.VT_BSTR;
         public static readonly VarEnum QNAME          = VarEnum.VT_BSTR;
         public static readonly VarEnum BINARY         = VarEnum.VT_ARRAY | VarEnum.VT_UI1;
         public static readonly VarEnum ARRAY_SHORT    = VarEnum.VT_ARRAY | VarEnum.VT_I2;
         public static readonly VarEnum ARRAY_USHORT   = VarEnum.VT_ARRAY | VarEnum.VT_UI2;
         public static readonly VarEnum ARRAY_INT      = VarEnum.VT_ARRAY | VarEnum.VT_I4;
         public static readonly VarEnum ARRAY_UINT     = VarEnum.VT_ARRAY | VarEnum.VT_UI4;
         public static readonly VarEnum ARRAY_LONG     = VarEnum.VT_ARRAY | VarEnum.VT_I8;
         public static readonly VarEnum ARRAY_ULONG    = VarEnum.VT_ARRAY | VarEnum.VT_UI8;
         public static readonly VarEnum ARRAY_FLOAT    = VarEnum.VT_ARRAY | VarEnum.VT_R4;
         public static readonly VarEnum ARRAY_DOUBLE   = VarEnum.VT_ARRAY | VarEnum.VT_R8;
         public static readonly VarEnum ARRAY_DECIMAL  = VarEnum.VT_ARRAY | VarEnum.VT_CY;
         public static readonly VarEnum ARRAY_DATETIME = VarEnum.VT_ARRAY | VarEnum.VT_DATE;
         public static readonly VarEnum ARRAY_BOOLEAN  = VarEnum.VT_ARRAY | VarEnum.VT_BOOL;
         public static readonly VarEnum ARRAY_STRING   = VarEnum.VT_ARRAY | VarEnum.VT_BSTR;
         public static readonly VarEnum ARRAY_ANY_TYPE = VarEnum.VT_ARRAY | VarEnum.VT_VARIANT;
      }

      // system data types for all known data types
      public class SystemType
      {
         public static readonly System.Type ANY_TYPE       = typeof(object);
         public static readonly System.Type SBYTE          = typeof(sbyte);
         public static readonly System.Type BYTE           = typeof(byte);
         public static readonly System.Type SHORT          = typeof(short);
         public static readonly System.Type USHORT         = typeof(ushort);
         public static readonly System.Type INT            = typeof(int);
         public static readonly System.Type UINT           = typeof(uint);
         public static readonly System.Type LONG           = typeof(long);
         public static readonly System.Type ULONG          = typeof(ulong);
         public static readonly System.Type FLOAT          = typeof(float);
         public static readonly System.Type DOUBLE         = typeof(double);
         public static readonly System.Type DECIMAL        = typeof(decimal);
         public static readonly System.Type DATETIME       = typeof(DateTime);
         public static readonly System.Type TIME           = typeof(DateTime);
         public static readonly System.Type DATE           = typeof(DateTime);
         public static readonly System.Type DURATION       = typeof(TimeSpan);
         public static readonly System.Type BOOLEAN        = typeof(bool);
         public static readonly System.Type STRING         = typeof(string);
         public static readonly System.Type QNAME          = typeof(XmlQualifiedName);
         public static readonly System.Type BINARY         = typeof(byte[]);
         public static readonly System.Type ARRAY_SHORT    = typeof(short[]);
         public static readonly System.Type ARRAY_USHORT   = typeof(ushort[]);
         public static readonly System.Type ARRAY_INT      = typeof(int[]);
         public static readonly System.Type ARRAY_UINT     = typeof(uint[]);
         public static readonly System.Type ARRAY_LONG     = typeof(long[]);
         public static readonly System.Type ARRAY_ULONG    = typeof(ulong[]);
         public static readonly System.Type ARRAY_FLOAT    = typeof(float[]);
         public static readonly System.Type ARRAY_DOUBLE   = typeof(double[]);
         public static readonly System.Type ARRAY_DECIMAL  = typeof(decimal[]);
         public static readonly System.Type ARRAY_DATETIME = typeof(DateTime[]);
         public static readonly System.Type ARRAY_BOOLEAN  = typeof(bool[]);
         public static readonly System.Type ARRAY_STRING   = typeof(string[]);
         public static readonly System.Type ARRAY_ANY_TYPE = typeof(object[]);
      }


      //---------------------------------------------------------------------------------------
      // returns an array of all known qualified names for types
      public static XmlQualifiedName[] GetQNames()
      {
         ArrayList names = new ArrayList();
         FieldInfo[] fields = typeof(Type).GetFields(BindingFlags.Static | BindingFlags.Public);
         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(Type));
            if (current != null && current.GetType() == typeof(XmlQualifiedName))
            {
               names.Add(current);
            }
         }
         return (XmlQualifiedName[])names.ToArray(typeof(XmlQualifiedName));
      }

      //---------------------------------------------------------------------------------------
      // returns an array of all known qualified names for types
      public static string[] GetNames()
      {
         ArrayList names = new ArrayList();
         FieldInfo[] fields = typeof(Type).GetFields(BindingFlags.Static | BindingFlags.Public);
         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(Type));
            if (current != null && current.GetType() == typeof(XmlQualifiedName))
            {
               string currentname = ((XmlQualifiedName)current).Name ;
               names.Add(currentname);
            }
         }
         return (string[])names.ToArray(typeof(string));
      }

      //---------------------------------------------------------------------------------------
      // returns the type for the selection index
      public static System.Type GetTypeForIndex( int idx )
      {
         FieldInfo[] fields = typeof(Type.SystemType).GetFields(BindingFlags.Static | BindingFlags.Public);
         object current = fields[idx].GetValue(typeof(Type.SystemType));
         return (System.Type)current;
      }

      //---------------------------------------------------------------------------------------
      // returns the type for the selection index
      public static int GetIndexForType( System.Type type )
      {
         FieldInfo[] fields = typeof(Type.SystemType).GetFields(BindingFlags.Static | BindingFlags.Public);
         int i = 0;
         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(Type.SystemType));
            if( (System.Type)current == type )
               return i ;
            else
               ++i ;
         }
         return -1 ;
      }



      //---------------------------------------------------------------------------------------
      // returns an array of all known qualified names for conversion selection types
      public static XmlQualifiedName[] GetSelectionQNames()
      {
         ArrayList names = new ArrayList();
         FieldInfo[] fields = typeof(Type).GetFields(BindingFlags.Static | BindingFlags.Public);
         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(Type));
            if (current != null && current.GetType() == typeof(XmlQualifiedName))
            {
               string cn = ((XmlQualifiedName)current).Name ;
               if( !(cn == "QName") && !(cn =="base64Binary") && !(cn == "anyType") && !(cn.StartsWith("Array")) )
                  names.Add(current);
            }
         }
         return (XmlQualifiedName[])names.ToArray(typeof(XmlQualifiedName));
      }

      //---------------------------------------------------------------------------------------
      // returns an array of all known qualified names for conversion selection types
      public static string[] GetSelectionNames()
      {
         ArrayList names = new ArrayList();
         FieldInfo[] fields = typeof(Type).GetFields(BindingFlags.Static | BindingFlags.Public);
         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(Type));
            if (current != null && current.GetType() == typeof(XmlQualifiedName))
            {
               string cn = ((XmlQualifiedName)current).Name ;
               if( !(cn == "QName") && !(cn =="base64Binary") && !(cn == "anyType") && !(cn.StartsWith("Array")) )
                  names.Add(cn);
            }
         }
         return (string[])names.ToArray(typeof(string));
      }

      //---------------------------------------------------------------------------------------
      // finds the qualified name for the data type for the specified object
      public static XmlQualifiedName GetQName(object data)
      {
         return (data != null)?GetQName(data.GetType()):null;
      }

      //---------------------------------------------------------------------------------------
      // finds the qualified name for the data type for the specified object
      public static string GetName(object data)
      {
         return (data != null) ? GetQName(data.GetType()).Name : null;
      }
   
      //---------------------------------------------------------------------------------------
      // finds the qualified name for the data type with the specified system type
      public static XmlQualifiedName GetQName(System.Type type)
      {
         FieldInfo[] fields = typeof(SystemType).GetFields(BindingFlags.Static | BindingFlags.Public);

         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(SystemType));

            if (type == (System.Type)current)
            {
               return (XmlQualifiedName)typeof(Type).GetField(field.Name).GetValue(typeof(Type));
            }
         }
         return GetQName( typeof(string) ) ;
      }


      
      //---------------------------------------------------------------------------------------
      // finds the qualified name for the data type with the specified variant type
      public static XmlQualifiedName GetName(VarEnum type)
      {
         FieldInfo[] fields = typeof(VarType).GetFields(BindingFlags.Static | BindingFlags.Public);

         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(VarType));

            if (current != null && current.GetType() == typeof(VarEnum))
            {
               if (type == (VarEnum)current)
               {
                  return (XmlQualifiedName)typeof(Type).GetField(field.Name).GetValue(typeof(Type));
               }
            }
         }

         return null;
      }


      //---------------------------------------------------------------------------------------
      // finds the system type for the data type with the specified qualified name
      public static System.Type GetSystemType(XmlQualifiedName name)
      {
         string symbolName = GetSymbolName(name);

         if (symbolName == null)
         {
            return null;
         }

         FieldInfo[] fields = typeof(Type.SystemType).GetFields(BindingFlags.Static | BindingFlags.Public);

         foreach (FieldInfo field in fields)
         {
            if (field.Name != symbolName)
            {
               continue;
            }

            object current = field.GetValue(typeof(Type.SystemType));

            if (current != null)
            {
               return (System.Type)current;
            }
         }

         return null;
      }


      //---------------------------------------------------------------------------------------
      // checks if the type specified with the qualified name is an array type
      public static bool IsArray(XmlQualifiedName name)
      {
         System.Type type = GetSystemType(name);
         return (type != null && type.IsArray);
      }

   
      
      //---------------------------------------------------------------------------------------
      // finds the element type for the array data type with the specified qualified name
      public static XmlQualifiedName GetElementType(XmlQualifiedName name)
      {
         System.Type type = GetSystemType(name);

         if (type == null || !type.IsArray)
         {
            return null;
         }

         return GetQName(type.GetElementType());
      }

      
      
      //---------------------------------------------------------------------------------------
      // finds the variant type for the data type with the specified qualified name
      public static VarEnum GetVarType(XmlQualifiedName name)
      {
         string symbolName = GetSymbolName(name);

         if (symbolName == null)
         {
            return VarEnum.VT_EMPTY;
         }

         FieldInfo[] fields = typeof(Type.VarType).GetFields(BindingFlags.Static | BindingFlags.Public);

         foreach (FieldInfo field in fields)
         {
            if (field.Name != symbolName)
            {
               continue;
            }

            object current = field.GetValue(typeof(Type.VarType));

            if (current != null && current.GetType() == typeof(VarEnum))
            {
               return (VarEnum)current;
            }
         }

         return VarEnum.VT_EMPTY;
      }

      
      
      
      //---------------------------------------------------------------------------------------
      // finds the symbolic name for a property with the specified qualified name
      private static string GetSymbolName(XmlQualifiedName name)
      {
         if (name == null) return null;

         FieldInfo[] fields = typeof(Type).GetFields(BindingFlags.Static | BindingFlags.Public);

         foreach (FieldInfo field in fields)
         {
            object current = field.GetValue(typeof(Type));

            if (current != null && current.GetType() == typeof(XmlQualifiedName))
            {
               if (name == (XmlQualifiedName)current)
               {
                  return field.Name;
               }
            }
         }

         return null;
      }
   }
}
