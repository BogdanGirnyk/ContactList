using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using ContactList.DAL;

namespace ContactList.Filters
{



}





    public class LogActionRequestAttribute : ActionFilterAttribute, IActionFilter
    {






        private int MAX_RECORD_LEN = 2000;
        private string PASSWORD_STRING = "PASSWORD";
        private int TRACE_LEVEL_ENTRY = 2;
        private int TRACE_LEVEL_EXIT = 2;

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (System.Web.HttpContext.Current.Application["TraceLogState"] == null ||
            //    System.Web.HttpContext.Current.Application["TraceLogState"].ToString() != "ON") return;
            if (TRACE_LEVEL_ENTRY < 1) return;

            var sb = new StringBuilder();
            sb.Append("Enter: ");
            sb.Append("ActionName:").Append(filterContext.ActionDescriptor.ActionName).Append("; ");
            sb.Append("ControllerName:")
                .Append(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)
                .Append("; ");
            sb.Append("ActionParameters:{");
            var keys = filterContext.ActionParameters.Keys.ToArray();
            for (int i = 0; i < keys.Count(); i++)
            {
                sb.Append(keys[i]).Append(":");
                var value = filterContext.ActionParameters[keys[i]];
                if (keys[i].ToUpper().Contains(PASSWORD_STRING))
                {
                    sb.Append("xxxxxxxx");
                }
                else
                {
                    ParseParameterProperties(sb, value, TRACE_LEVEL_ENTRY);
                }
                if (i + 1 < keys.Count()) sb.Append("; ");
            }
            sb.Append("} ");
            var message = sb.ToString();

            ContactListContext.LogTrace(message);
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (System.Web.HttpContext.Current.Application["TraceLogState"] == null ||
            //    System.Web.HttpContext.Current.Application["TraceLogState"].ToString() != "ON") return;
            if (TRACE_LEVEL_EXIT < 1) return;

            var sb = new StringBuilder();
            sb.Append("Exit: ");
            DiscoverObjectProperties(sb, filterContext.Result, TRACE_LEVEL_EXIT);
            var message = sb.ToString();

            ContactListContext.LogTrace(message);
        }

        private void ParseParameterProperties(StringBuilder sb, Object obj, int recurse)
        {
            if (obj == null) return;
            var type = obj.GetType();
            if (type == typeof(System.String))
            {
                sb.Append(obj.ToString()).Append("; ");
            }
            else if (type == typeof(System.Int16) || type == typeof(System.Int32)
                || type == typeof(System.Int64) || type == typeof(System.UInt16)
                || type == typeof(System.UInt32) || type == typeof(System.UInt64)
                || type == typeof(System.Byte) || type == typeof(System.SByte)
                || type == typeof(System.Enum) || type == typeof(System.Single)
                || type == typeof(System.Double) || type == typeof(System.DateTime)
                || type == typeof(System.Decimal) || type == typeof(System.Char)
                || type == typeof(System.Guid) || type == typeof(System.DateTime)
                || type == typeof(System.TimeSpan) || type == typeof(System.Boolean))
            {
                sb.Append(obj.ToString()).Append("; ");
            }
            else if (type == typeof(System.Web.Mvc.FormCollection))
            {
                var formCollection = (FormCollection)obj;
                var keys = formCollection.AllKeys;
                sb.Append("{");
                foreach (var key in keys)
                {
                    if (key.ToUpper().Contains(PASSWORD_STRING))
                    {
                        sb.Append(key).Append(":xxxxxxxx; ");
                    }
                    else
                    {
                        var value = formCollection.GetValue(key);
                        sb.Append(key).Append(":").Append(value.AttemptedValue).Append("; ");
                    }
                }
                sb.Append("}; ");
            }
            else
            {
                DiscoverObjectProperties(sb, obj, recurse - 1);
            }
        }

        private void DiscoverObjectProperties(StringBuilder sb, Object obj, int recurse)
        {
            if (recurse <= 0) return;
            if (obj == null) return;
            var shortName = ShortName(obj);
            if (shortName == "TempDataDictionary"
                || shortName == "DynamicViewDataDictionary" || shortName == "ViewDataDictionary"
                || shortName == "ViewEngineCollection" || shortName == "RouteValueDictionary")
            {
                return;
            }

            sb.Append(shortName).Append(":{");

            if (obj as ICollection != null)
            {
                var count = (obj as ICollection).Count;
                for (int i = 0; i < count; i++)
                {
                    var en = (obj as ICollection).GetEnumerator();
                    while (en.MoveNext())
                    {
                        DiscoverObjectProperties(sb, en.Current, recurse - 1);
                    }
                }
                sb.Append("}; ");
                return;
            }

            var properties = obj.GetType().GetProperties();
            foreach (var info in properties)
            {
                if (info.CanRead)
                {
                    if (info.GetIndexParameters().Length > 0)
                    {
                        bool isIndexed = false;
                        foreach (var prop in properties)
                        {
                            if (prop.Name == "Count")
                            {
                                isIndexed = true;
                                break;
                            }
                        }
                        if (isIndexed)
                        {
                            for (int i = 0; i < info.GetIndexParameters().Length; i++)
                            {
                                var item = info.GetValue(obj, new Object[] { i });
                                DiscoverObjectProperties(sb, item, recurse - 1);
                            }
                        }
                        continue;
                    }

                    Object o = info.GetValue(obj, null);
                    Type type = (o == null) ? null : o.GetType();
                    if (o == null)
                    {
                        sb.Append(info.Name).Append(":; ");
                    }
                    else if (type == typeof(System.String))
                    {
                        if (info.Name.ToUpper().Contains(PASSWORD_STRING))
                        {
                            sb.Append(info.Name).Append(":xxxxxxxx; ");
                        }
                        else
                        {
                            sb.Append(info.Name).Append(":").Append((o == null) ? "" : o.ToString()).Append("; ");
                        }
                    }
                    else if (type == typeof(System.String[]))
                    {
                        sb.Append(info.Name).Append(":");
                        var oArray = o as String[];
                        var oLength = oArray.Length;

                        var sc = new StringBuilder();
                        for (int i = 0; i < oLength; i++)
                        {
                            sc.Append(oArray[i]);
                            if (i < oLength - 1) sc.Append("|");
                        }
                        sb.Append(sc.ToString()).Append("; ");
                    }
                    else if (type == typeof(System.Int16) || type == typeof(System.Int32)
                        || type == typeof(System.Int64) || type == typeof(System.UInt16)
                        || type == typeof(System.UInt32) || type == typeof(System.UInt64)
                        || type == typeof(System.Byte) || type == typeof(System.SByte)
                        || type == typeof(System.Enum) || type == typeof(System.Single)
                        || type == typeof(System.Double) || type == typeof(System.DateTime)
                        || type == typeof(System.Decimal) || type == typeof(System.Char)
                        || type == typeof(System.Guid) || type == typeof(System.DateTime)
                        || type == typeof(System.TimeSpan) || type == typeof(System.Boolean))
                    {
                        sb.Append(info.Name).Append(":").Append(o.ToString()).Append("; ");
                    }
                    else if (type == typeof(System.Exception))
                    {
                        sb.Append(info.Name).Append(":")
                            .Append((o == null) ? "" : ((Exception)o).Message).Append("; ");
                    }
                    else
                    {
                        DiscoverObjectProperties(sb, o, recurse - 1);
                    }
                }
            }
            sb.Append("}; ");
        }

        private string ShortName(Object obj)
        {
            var longName = obj.GetType().ToString();
            var longNameArray = longName.Split('.');
            return longNameArray[longNameArray.Length - 1];
        }
    }

