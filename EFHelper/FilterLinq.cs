
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EFHelper.Helper
{
    public class FilterLinq
    {
        
        public static Expression GetParameterExpression(string _Coloperator, string _FieldType, bool _Isnullable, Expression _ColumnNameProperty, Expression _ColumnValue)
        {
            Expression expParam = null;
            bool myInOperator = false;
            if (_ColumnNameProperty != null)
            {
                if (_Coloperator == "in" || _Coloperator == "like")
                    myInOperator = true;
                else
                    myInOperator = false;
                Type myT = null;

                myT = GetConvertedType(_FieldType, _Isnullable, myInOperator);
                if (_Coloperator == "=")
                {


                    if (_Isnullable)
                    {
                        expParam = Expression.Equal(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }
                    else
                    {
                        expParam = Expression.Equal(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }


                }
                else if (_Coloperator == ">")
                {

                    if (_Isnullable)
                    {
                        expParam = Expression.GreaterThan(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }
                    else
                    {
                        expParam = Expression.GreaterThan(_ColumnNameProperty, _ColumnValue);
                    }
                }
                else if (_Coloperator == ">=")
                {
                    if (_Isnullable)
                    {
                        expParam = Expression.GreaterThanOrEqual(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }
                    else
                    {
                        expParam = Expression.GreaterThanOrEqual(_ColumnNameProperty, _ColumnValue);
                    }
                }
                else if (_Coloperator == "<")
                {
                    if (_Isnullable)
                    {
                        expParam = Expression.LessThan(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }
                    else
                    {
                        expParam = Expression.LessThan(_ColumnNameProperty, _ColumnValue);
                    }
                }
                else if (_Coloperator == "<=")
                {
                    if (_Isnullable)
                    {
                        expParam = Expression.LessThanOrEqual(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }
                    else
                    {
                        expParam = Expression.LessThanOrEqual(_ColumnNameProperty, _ColumnValue);
                    }
                }

                else if (_Coloperator == "in")
                {
                    var method = GetInMethodInfo(_FieldType, _Isnullable);
                    var myleft = Expression.Convert(_ColumnValue, myT);
                    expParam = Expression.Call(myleft, method, _ColumnNameProperty);
                }
                else if (_Coloperator == "like")
                {
                    //MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);
                    //MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);
                    //var method = GetInMethodInfo(_FieldType, _Isnullable);
                    //var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    //var myleft = Expression.Convert(_ColumnValue, myT);
                    var myleft = Expression.Convert(_ColumnValue, typeof(string));
               
                    // expParam = Expression.Call(myleft, method, _ColumnNameProperty);
                    expParam = Expression.Call(_ColumnNameProperty, method, myleft);
                }
                else if (_Coloperator == "<>")
                {
                    if (_Isnullable)
                    {
                        expParam = Expression.NotEqual(Expression.Convert(_ColumnNameProperty, myT), Expression.Convert(_ColumnValue, myT));
                    }
                    else
                    {
                        expParam = Expression.NotEqual(_ColumnNameProperty, _ColumnValue);
                    }
                }
            }

            return expParam;


        }
        public static object GetConvertedValue(string _Fieldtype, object myvalue, string _Coloperator, bool isnullable)
        {
            string value = string.Empty;
            bool myInOperator = false;
            if (_Coloperator == "in" || _Coloperator == "like")
                myInOperator = true;
            else
                myInOperator = false;
            if (myvalue != null)
            {
                value = myvalue.ToString();
                if (!isnullable)
                {
                    if (_Fieldtype == "int32")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<int>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToInt32(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToInt32(value);
                        }


                    }
                    else if (_Fieldtype == "int16")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Int16>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToInt16(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToInt16(value);
                        }
                    }
                    else if (_Fieldtype == "int64")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Int64>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToInt64(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToInt64(value);
                        }
                    }
                    else if (_Fieldtype == "single")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Single>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToSingle(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToSingle(value);
                        }
                    }
                    else if (_Fieldtype == "double")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<double>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDouble(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDouble(value);
                        }
                    }

                    else if (_Fieldtype == "float")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<double>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDouble(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDouble(value);
                        }
                    }

                    else if (_Fieldtype == "char")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Char>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToChar(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToChar(value);
                        }
                    }
                    else if (_Fieldtype == "boolean")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<bool>();
                            foreach (var item in value.Split('|'))
                            {
                                string myValue = string.Empty;
                                if (item.Trim() == "1")
                                {
                                    myValue = "true";
                                }
                                else if (item.Trim() == "0")
                                {
                                    myValue = "false";
                                }
                                t.Add(Convert.ToBoolean(myValue));
                            }
                            return t;
                        }
                        else
                        {
                            if (value.Trim() == "1")
                            {
                                value = "true";
                            }
                            else if (value.Trim() == "0")
                            {
                                value = "false";
                            }

                            return Convert.ToBoolean(value);
                        }
                    }
                    else if (_Fieldtype == "string")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<string>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(item);
                            }
                            return t;
                        }
                        else
                        {
                            return value;
                        }
                    }
                    else if (_Fieldtype == "guid")
                    {
                        return new Guid(myvalue.ToString());
                    }
                    else if (_Fieldtype == "decimal")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<decimal>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDecimal(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDecimal(value);
                        }
                    }
                    else if (_Fieldtype == "byte")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<decimal>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToByte(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToByte(value);
                        }
                    }
                    else if (_Fieldtype == "datetime")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<DateTime>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDateTime(Convert.ToDateTime(item).ToString("yyyy-MM-dd HH:mm:ss")));
                                //t.Add(Convert.ToDateTime(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDateTime(Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss"));
                            //return Convert.ToDateTime(value);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (_Fieldtype == "int32")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<int>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToInt32(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToInt32(value);
                        }


                    }
                    else if (_Fieldtype == "int16")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<Int16>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToInt16(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToInt16(value);
                        }
                    }
                    else if (_Fieldtype == "int64")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<Int64>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToInt64(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToInt64(value);
                        }
                    }
                    else if (_Fieldtype == "single")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<Single>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToSingle(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToSingle(value);
                        }
                    }
                    else if (_Fieldtype == "double")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<double>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDouble(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDouble(value);
                        }
                    }

                    else if (_Fieldtype == "float")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<double>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDouble(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDouble(value);
                        }
                    }

                    else if (_Fieldtype == "char")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<Char>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToChar(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToChar(value);
                        }
                    }
                    else if (_Fieldtype == "boolean")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<bool>>();
                            foreach (var item in value.Split('|'))
                            {
                                string myValue = string.Empty;
                                if (value.Trim() == "1")
                                {
                                    myValue = "true";
                                }
                                else if (value.Trim() == "0")
                                {
                                    myValue = "false";
                                }
                                t.Add(Convert.ToBoolean(myValue));
                            }
                            return t;
                        }
                        else
                        {
                            if (value.Trim() == "1")
                            {
                                value = "true";
                            }
                            else if (value.Trim() == "0")
                            {
                                value = "false";
                            }
                            return Convert.ToBoolean(value);
                        }
                    }
                    else if (_Fieldtype == "string")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<string>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(item);
                            }
                            return t;
                        }
                        else
                        {
                            return value;
                        }
                    }
                    else if (_Fieldtype == "guid")
                    {
                        return new Guid(myvalue.ToString());
                    }
                    else if (_Fieldtype == "decimal")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<decimal>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDecimal(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToDecimal(value);
                        }
                    }
                    else if (_Fieldtype == "byte")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<decimal>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToByte(item));
                            }
                            return t;
                        }
                        else
                        {
                            return Convert.ToByte(value);
                        }
                    }
                    else if (_Fieldtype == "datetime")
                    {
                        if (value.Contains("|") || myInOperator)
                        {
                            var t = new List<Nullable<DateTime>>();
                            foreach (var item in value.Split('|'))
                            {
                                t.Add(Convert.ToDateTime(Convert.ToDateTime(item).ToString("yyyy-MM-dd HH:mm:ss")));

                            }
                            return t;
                        }
                        else
                        {
                            //   return Convert.ToDateTime(value);

                            return Convert.ToDateTime(Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss"));
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }

        }

        public static MethodInfo GetInMethodInfo(string _Fieldtype, bool isNullable)
        {
            MethodInfo t = null;
            if (!isNullable)
            {
                if (_Fieldtype == "int32")
                {
                    t = typeof(List<int>).GetMethod("Contains", new[] { typeof(int) });
                }
                else if (_Fieldtype == "int16")
                {
                    t = typeof(List<short>).GetMethod("Contains", new[] { typeof(short) });
                }
                else if (_Fieldtype == "int64")
                {
                    t = typeof(List<long>).GetMethod("Contains", new[] { typeof(long) });
                }
                else if (_Fieldtype == "single")
                {
                    t = typeof(List<Single>).GetMethod("Contains", new[] { typeof(Single) });
                }
                else if (_Fieldtype == "double")
                {
                    t = typeof(List<double>).GetMethod("Contains", new[] { typeof(double) });
                }
                else if (_Fieldtype == "float")
                {
                    t = typeof(List<float>).GetMethod("Contains", new[] { typeof(float) });
                }

                else if (_Fieldtype == "char")
                {
                    t = typeof(List<char>).GetMethod("Contains", new[] { typeof(char) });
                }
                else if (_Fieldtype == "boolean")
                {
                    t = typeof(List<bool>).GetMethod("Contains", new[] { typeof(bool) });
                }
                else if (_Fieldtype == "string")
                {
                    t = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
                }
                else if (_Fieldtype == "decimal")
                {
                    t = typeof(List<decimal>).GetMethod("Contains", new[] { typeof(decimal) });
                }
                else if (_Fieldtype == "datetime")
                {
                    t = typeof(List<DateTime>).GetMethod("Contains", new[] { typeof(DateTime) });

                }
                else if (_Fieldtype == "byte")
                {
                    t = typeof(List<Byte>).GetMethod("Contains", new[] { typeof(Byte) });

                }
                else if (_Fieldtype == "guid")
                {
                    t = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) });

                }

            }
            else
            {
                if (_Fieldtype == "int32")
                {
                    t = typeof(List<Nullable<int>>).GetMethod("Contains", new[] { typeof(Nullable<int>) });
                }
                else if (_Fieldtype == "int16")
                {
                    t = typeof(List<Nullable<short>>).GetMethod("Contains", new[] { typeof(Nullable<short>) });
                }
                else if (_Fieldtype == "int64")
                {
                    t = typeof(List<Nullable<long>>).GetMethod("Contains", new[] { typeof(Nullable<long>) });
                }
                else if (_Fieldtype == "single")
                {
                    t = typeof(List<Nullable<Single>>).GetMethod("Contains", new[] { typeof(Nullable<Single>) });
                }
                else if (_Fieldtype == "double")
                {
                    t = typeof(List<Nullable<double>>).GetMethod("Contains", new[] { typeof(Nullable<double>) });
                }
                else if (_Fieldtype == "float")
                {
                    t = typeof(List<Nullable<float>>).GetMethod("Contains", new[] { typeof(Nullable<float>) });
                }
                else if (_Fieldtype == "char")
                {
                    t = typeof(List<Nullable<char>>).GetMethod("Contains", new[] { typeof(Nullable<char>) });
                }
                else if (_Fieldtype == "boolean")
                {
                    t = typeof(List<bool>).GetMethod("Contains", new[] { typeof(bool) });
                }
                else if (_Fieldtype == "string")
                {
                    t = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
                }
                else if (_Fieldtype == "decimal")
                {
                    t = typeof(List<Nullable<decimal>>).GetMethod("Contains", new[] { typeof(Nullable<decimal>) });
                }
                else if (_Fieldtype == "datetime")
                {
                    t = typeof(List<Nullable<DateTime>>).GetMethod("Contains", new[] { typeof(Nullable<DateTime>) });

                }
                else if (_Fieldtype == "byte")
                {
                    t = typeof(List<Nullable<Byte>>).GetMethod("Contains", new[] { typeof(Nullable<Byte>) });

                }
                else if (_Fieldtype == "guid")
                {
                    t = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) });
                }
            }
            return t;
        }

        public static Type GetConvertedType(string _Fieldtype, bool _isNullable, bool _isINoperator)
        {
            Type t = null;


            if (_isNullable)
            {
                if (_Fieldtype == "int32")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<int>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<int>>);
                    }
                }
                else if (_Fieldtype == "int16")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<short>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<short>>);
                    }
                }
                else if (_Fieldtype == "int64")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<long>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<long>>);
                    }
                }
                else if (_Fieldtype == "single")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<Single>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<Single>>);
                    }
                }
                else if (_Fieldtype == "double")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<double>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<double>>);
                    }
                }

                else if (_Fieldtype == "float")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<float>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<float>>);
                    }
                }

                else if (_Fieldtype == "char")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<char>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<char>>);
                    }
                }
                else if (_Fieldtype == "boolean")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<bool>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<bool>>);
                    }
                }
                else if (_Fieldtype == "string")
                {

                    if (!_isINoperator)
                    {
                        t = typeof(string);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<string>);
                    }
                }
                else if (_Fieldtype == "decimal")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<decimal>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<decimal>>);
                    }
                }
                else if (_Fieldtype == "guid")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Guid);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Guid>);
                    }
                }
                else if (_Fieldtype == "byte")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Byte);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Byte>);
                    }
                }
                else if (_Fieldtype == "datetime")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Nullable<DateTime>);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Nullable<DateTime>>);
                    }
                }

            }
            else
            {
                if (_Fieldtype == "int32")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(int);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<int>);
                    }
                }
                else if (_Fieldtype == "int16")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(short);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<short>);
                    }
                }
                else if (_Fieldtype == "int64")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(long);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<long>);
                    }
                }
                else if (_Fieldtype == "single")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Single);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Single>);
                    }
                }
                else if (_Fieldtype == "double")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(double);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<double>);
                    }
                }
                else if (_Fieldtype == "float")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(float);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<float>);
                    }
                }

                else if (_Fieldtype == "char")
                {

                    if (!_isINoperator)
                    {
                        t = typeof(char);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<char>);
                    }
                }
                else if (_Fieldtype == "boolean")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(bool);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<bool>);
                    }
                }
                else if (_Fieldtype == "string")
                {

                    if (!_isINoperator)
                    {
                        t = typeof(string);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<string>);
                    }
                }
                else if (_Fieldtype == "decimal")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(decimal);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<decimal>);
                    }
                }
                else if (_Fieldtype == "guid")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Guid);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Guid>);
                    }
                }
                else if (_Fieldtype == "byte")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(Byte);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<Byte>);
                    }
                }
                else if (_Fieldtype == "datetime")
                {
                    if (!_isINoperator)
                    {
                        t = typeof(DateTime);
                    }
                    else
                    {
                        t = typeof(System.Collections.Generic.List<DateTime>);
                    }
                }
            }
            return t;
        }

        public static DbType GetConvertedDbType(string _Fieldtype)
        {
            if (_Fieldtype == "int32")
            {
                return DbType.Int32;
            }
            else if (_Fieldtype == "int16")
            {
                return DbType.Int16;
            }
            else if (_Fieldtype == "int64")
            {
                return DbType.Int64;
            }
            else if (_Fieldtype == "single")
            {
                return DbType.Single;
            }
            else if (_Fieldtype == "double")
            {
                return DbType.Double;
            }
            else if (_Fieldtype == "float")
            {
                return DbType.Double;
            }
            else if (_Fieldtype == "char")
            {
                return DbType.String;
            }
            else if (_Fieldtype == "boolean")
            {
                return DbType.Boolean;
            }
            else if (_Fieldtype == "string")
            {
                return DbType.String;
            }
            else if (_Fieldtype == "decimal")
            {
                return DbType.Decimal;
            }
            else if (_Fieldtype == "guid")
            {
                return DbType.Guid;
            }
            else if (_Fieldtype == "byte")
            {
                return DbType.Byte;
            }
            else if (_Fieldtype == "datetime")
            {
                return DbType.DateTime;
            }
            else
            {
                return DbType.String;
            }
        }

        public static Expression<Func<T, Boolean>> GetWhereClause<T>(List<SearchField> SearchFieldList)
        {
            string _nulltype = string.Empty;
            string _fieldtype = string.Empty;
            string _fullname = string.Empty;
            string _colname = string.Empty;
            string _coloperator = string.Empty;
            object _colvalue = null;
            bool isnullable = false;
            _nulltype = "nullable`1";
            ParameterExpression pe = Expression.Parameter(typeof(T), typeof(T).Name);
            Expression combined = null;
            Expression e1 = null;
            Expression columnNameProperty = null;
            Expression columnValue = null;
           

            string propname = string.Empty;
            for (int i = 0; i < SearchFieldList.Count; i++)
            {
                if (SearchFieldList[i].Name == null)
                    SearchFieldList.Remove(SearchFieldList[i]);

            }
            for (int i = 0; i < SearchFieldList.Count; i++)
            {

                SearchField item = SearchFieldList[i];
                e1 = null;
               
                isnullable = false;
                propname = string.Empty;
                _fieldtype = string.Empty;
                _colname = null;
                _colvalue = null;
                _coloperator = null;
                if (!string.IsNullOrEmpty(item.Name))
                {
                    if (!string.IsNullOrEmpty(item.Name) & !string.IsNullOrEmpty(item.Value1) & !string.IsNullOrEmpty(item.Operator))
                    {
                        _colname = item.Name.Trim();
                        _colvalue = item.Value1;
                        _coloperator = item.Operator.Trim().ToLower();
                        //columnValue = Expression.Constant(_colvalue);
                        try
                        {
                            columnNameProperty = Expression.Property(pe, _colname);
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        foreach (FieldInfo property in (typeof(T).GetRuntimeFields()))
                        {

                            propname = property.Name;
                            propname = propname.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                            if (propname.ToLower() == _colname.ToLower())
                            {
                                isnullable = false;
                                _fieldtype = property.FieldType.Name.ToLower();
                                if (_fieldtype == _nulltype)
                                {
                                    _fullname = property.FieldType.FullName.ToLower().Split(',')[0].ToString();
                                    _fieldtype = _fullname.Replace("system." + _nulltype + "[[", string.Empty).Replace("system.", string.Empty);
                                    isnullable = true;
                                }
                                break;
                            }

                        }
                        if (_fieldtype == "datetime")
                        {
                            string mydatefrom = Convert.ToDateTime(_colvalue).ToString("yyyy-MM-dd") + " " + "00:00:00";
                            string mydateto = Convert.ToDateTime(_colvalue).ToString("yyyy-MM-dd") + " " + "23:59:59";
                            if (_coloperator == "=")
                            {
                                string mydatetooperator = "<=";
                                string mydatefromoperator = ">=";
                                item.Value1 = mydatefrom;
                                item.Operator = mydatefromoperator;
                                _colvalue = mydatefrom;
                                _coloperator = mydatefromoperator;
                                SearchFieldList.Add(new SearchField { Name = _colname, Operator = mydatetooperator, Value1 = mydateto });
                            }
                            else if (_coloperator == ">" || _coloperator == ">=")
                            {
                                _colvalue = mydatefrom;
                            }
                            else if (_coloperator == "<" || _coloperator == "<=")
                            {
                                string mydate = Convert.ToDateTime(_colvalue).ToString("yyyy-MM-dd") + " " + "23:00:00";
                                _colvalue = mydateto;
                            }

                        }
                      
                        if(_coloperator == "like")
                        {
                            _colvalue = Convert.ToString(_colvalue);
                             columnValue = Expression.Constant(_colvalue);
                        }
                        else
                        {
                            _colvalue = GetConvertedValue(_fieldtype, _colvalue, _coloperator, isnullable);
                            columnValue = Expression.Constant(_colvalue);
                        }
                        e1 = GetParameterExpression(_coloperator, _fieldtype, isnullable, columnNameProperty, columnValue);
                    }


                }

                if (e1 != null)
                {
                    if (combined == null)
                    {
                        combined = e1;
                    }
                    else
                    {
                        combined = Expression.And(combined, e1);
                    }
                }

            }


            if (combined != null)
            {
                return Expression.Lambda<Func<T, Boolean>>(combined, new ParameterExpression[] { pe });
            }
            else
            {
                return null;
            }
        }
        public static string GetWhereClause2<T>(List<SearchField> SearchFieldList)
        {
            string _nulltype = string.Empty;
            string _fieldtype = string.Empty;
            string _fullname = string.Empty;
            string _colname = string.Empty;
            string _coloperator = string.Empty;
            object _colvalue = null;
            bool isnullable = false;
            _nulltype = "nullable`1";
            ParameterExpression pe = Expression.Parameter(typeof(T), typeof(T).Name);
            Expression combined = null;
            Expression e1 = null;
            Expression columnNameProperty = null;
            Expression columnValue = null;
            

            string propname = string.Empty;
            for (int i = 0; i < SearchFieldList.Count; i++)
            {
                if (SearchFieldList[i].Name == null)
                    SearchFieldList.Remove(SearchFieldList[i]);

            }
            for (int i = 0; i < SearchFieldList.Count; i++)
            {

                SearchField item = SearchFieldList[i];
                e1 = null;
              
                isnullable = false;
                propname = string.Empty;
                _fieldtype = string.Empty;
                _colname = null;
                _colvalue = null;
                _coloperator = null;
                if (!string.IsNullOrEmpty(item.Name))
                {
                    if (!string.IsNullOrEmpty(item.Name) & !string.IsNullOrEmpty(item.Value1) & !string.IsNullOrEmpty(item.Operator))
                    {
                        _colname = item.Name.Trim();
                        _colvalue = item.Value1;
                        _coloperator = item.Operator.Trim().ToLower();
                        //columnValue = Expression.Constant(_colvalue);
                        try
                        {
                            columnNameProperty = Expression.Property(pe, _colname);
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        foreach (FieldInfo property in (typeof(T).GetRuntimeFields()))
                        {

                            propname = property.Name;
                            propname = propname.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                            if (propname.ToLower() == _colname.ToLower())
                            {
                                isnullable = false;
                                _fieldtype = property.FieldType.Name.ToLower();
                                if (_fieldtype == _nulltype)
                                {
                                    _fullname = property.FieldType.FullName.ToLower().Split(',')[0].ToString();
                                    _fieldtype = _fullname.Replace("system." + _nulltype + "[[", string.Empty).Replace("system.", string.Empty);
                                    isnullable = true;
                                }
                                break;
                            }

                        }
                        if (_fieldtype == "datetime")
                        {
                            string mydatefrom = Convert.ToDateTime(_colvalue).ToString("yyyy-MM-dd") + " " + "00:00:00";
                            string mydateto = Convert.ToDateTime(_colvalue).ToString("yyyy-MM-dd") + " " + "23:59:59";
                            if (_coloperator == "=")
                            {
                                string mydatetooperator = "<=";
                                string mydatefromoperator = ">=";
                                item.Value1 = mydatefrom;
                                item.Operator = mydatefromoperator;
                                _colvalue = mydatefrom;
                                _coloperator = mydatefromoperator;
                                SearchFieldList.Add(new SearchField { Name = _colname, Operator = mydatetooperator, Value1 = mydateto });
                            }
                            else if (_coloperator == ">" || _coloperator == ">=")
                            {
                                _colvalue = mydatefrom;
                            }
                            else if (_coloperator == "<" || _coloperator == "<=")
                            {
                                string mydate = Convert.ToDateTime(_colvalue).ToString("yyyy-MM-dd") + " " + "23:00:00";
                                _colvalue = mydateto;
                            }

                        }
                        if (_coloperator == "like")
                        {
                            _colvalue = Convert.ToString(_colvalue);
                            columnValue = Expression.Constant(_colvalue);
                        }
                        else
                        {
                            _colvalue = GetConvertedValue(_fieldtype, _colvalue, _coloperator, isnullable);
                            columnValue = Expression.Constant(_colvalue);
                        }
                        e1 = GetParameterExpression(_coloperator, _fieldtype, isnullable, columnNameProperty, columnValue);
                    }


                }

                if (e1 != null)
                {
                    if (combined == null)
                    {
                        combined = e1;
                    }
                    else
                    {
                        combined = Expression.And(combined, e1);
                    }
                }

            }

            var s = combined.ToString();
            return s;

            //if (combined != null)
            //{
            //    return Expression.Lambda<Func<T, Boolean>>(combined, new ParameterExpression[] { pe });
            //}
            //else
            //{
            //    return null;
            //}
        }

        public static IQueryable<T> OrderByDynamic<T>(IQueryable<T> query, string sortColumn, bool ascending) where T : class
        {
            // Dynamically creates a call like this: query.OrderBy(p =&gt; p.SortColumn)
            var parameter = Expression.Parameter(typeof(T), "p");

            PropertyInfo checkColumn = GenHelperEF.Getinstance.GetColumnProps<T>(sortColumn);

            if (checkColumn != null)
            {
                /*Jika kolom sortcolumn ada di table T nya*/
                string command = "OrderBy";

                if (!ascending)
                {
                    command = "OrderByDescending";
                }

                Expression resultExpression = null;

                PropertyInfo pi = null;
                string propname = string.Empty;
                foreach (PropertyInfo property in (typeof(T).GetRuntimeProperties()))
                {

                    propname = property.Name;
                    propname = propname.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Trim();
                    if (propname.ToLower() == sortColumn.ToLower())
                    {
                        pi = property;
                        break;
                    }

                }

                // var property = typeof(T).GetProperty(sortColumn);
                // this is the part p.SortColumn
                var propertyAccess = Expression.MakeMemberAccess(parameter, pi);

                // this is the part p =&gt; p.SortColumn
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
                resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { typeof(T), pi.PropertyType },
                   query.Expression, Expression.Quote(orderByExpression));

                return query.Provider.CreateQuery<T>(resultExpression);
            }
            else
            {
                return query;
            }
        }
        public static Expression<Func<T, bool>> CreateContainsFilterExpression<T>(List<long> propertyValue)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "type");
            var propertyExpression = Expression.Property(parameterExpression, "IdentityId");
            MethodInfo method = typeof(List<long>).GetMethod("Contains", new[] { typeof(long) });
            var someValue = Expression.Constant(propertyValue);
            var containsMethodExpression = Expression.Call(someValue, method, propertyExpression);
            var lambdaExpression = Expression.Lambda<Func<T, bool>>(containsMethodExpression, parameterExpression);
            return lambdaExpression;
        }
        public static Expression<Func<T, List<string>, bool>> FilterByCode<T>(List<string> codes)
        {
            // Method info for List<string>.Contains(code).
            var methodInfo = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });

            // List of codes to call .Contains() against.
            var instance = Expression.Variable(typeof(List<string>), "codes");

            var param = Expression.Parameter(typeof(T), "j");
            var left = Expression.Property(param, "Code");
            var expr = Expression.Call(instance, methodInfo, Expression.Property(param, "Code"));

            // j => codes.Contains(j.Code)
            return Expression.Lambda<Func<T, List<string>, bool>>(expr, new ParameterExpression[] { param, instance });
        }
        private static SearchField SetActiveboolTrueParameter<T>() where T : class
        {
            SearchField setactivebooltrue = GenHelperEF.Getinstance.SetActiveBoolOneParameter<T>();
            return setactivebooltrue;
        }
        public static IQueryable<T> QueryGeneratorList<T>(IQueryable<T> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {

            if (SearchFieldList == null)
            {
                SearchFieldList = new List<SearchField>();
            }
            //Menambahkan ActiveBool True
            SearchFieldList.Add(SetActiveboolTrueParameter<T>());
            // toptake = toptake == 0 ? defaultTakeList : toptake;
            //string s=GetWhereClause2<T>(SearchFieldList);
            var whereClause = GetWhereClause<T>(SearchFieldList);
            if (whereClause != null)
            {
                queryable = queryable.Where(whereClause);
            }
            if (!string.IsNullOrEmpty(sortColumn))
            {
                queryable = OrderByDynamic<T>(queryable, sortColumn, isascending);
            }
            else
            {
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null)
                {
                    sortColumn = pi.Name;
                    isascending = false;
                    queryable = OrderByDynamic<T>(queryable, sortColumn, isascending).AsQueryable();
                }
            }
            if (toptake != 0)
            {
                queryable = queryable.Take(toptake);
            }
            return queryable;
        }
        public static IQueryable<T> QueryGeneratorListInnerJoin<T>(IQueryable<T> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake, params string[] includes) where T : class
        {

            if (SearchFieldList == null)
            {
                SearchFieldList = new List<SearchField>();
            }
            //Menambahkan ActiveBool True
            SearchFieldList.Add(SetActiveboolTrueParameter<T>());
            //toptake = toptake == 0 ? defaultTakeList : toptake;
            queryable = GenHelperEF.Getinstance.IncludeMultiple<T>(queryable, includes);
            var whereClause = GetWhereClause<T>(SearchFieldList);
            if (whereClause != null)
            {
                queryable = queryable.Where(whereClause);
            }
            if (!string.IsNullOrEmpty(sortColumn))
            {
                queryable = OrderByDynamic<T>(queryable, sortColumn, isascending);
            }
            else
            {
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<T>();
                if (pi != null)
                {
                    sortColumn = pi.Name;
                    isascending = false;
                    queryable = OrderByDynamic<T>(queryable, sortColumn, isascending).AsQueryable();
                }
            }
            if (toptake != 0)
            {
                queryable = queryable.Take(toptake);
            }

            return queryable;
        }
        public static IQueryable<TSource> QueryGeneratorListTResult<TSource, TResult>(IQueryable<TSource> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where TSource : class
        {
            IQueryable<TSource> result = null;
            if (SearchFieldList == null)
            {
                SearchFieldList = new List<SearchField>();
            }
            //Menambahkan ActiveBool True
            SearchFieldList.Add(SetActiveboolTrueParameter<TSource>());
            //toptake = toptake == 0 ? defaultTakeList : toptake;
            var whereClause = GetWhereClause<TSource>(SearchFieldList);
            if (whereClause != null)
            {
                queryable = queryable.Where(whereClause);
            }
            if (!string.IsNullOrEmpty(sortColumn))
            {
                queryable = OrderByDynamic<TSource>(queryable, sortColumn, isascending);
            }
            else
            {
                PropertyInfo pi = GenHelperEF.Getinstance.GetIdentityColumnProps<TSource>();
                if (pi != null)
                {
                    sortColumn = pi.Name;
                    isascending = false;
                    queryable = OrderByDynamic<TSource>(queryable, sortColumn, isascending).AsQueryable();
                }
            }
            if (toptake != 0)
            {
                queryable = queryable.Take(toptake);
            }

            //Expression<Func<TSource, TResult>> selector = GenHelperEF.Getinstance.SelectorTResult<TSource, TResult>();
            //if (selector != null)
            //{

            //    result = queryable.Select(selector);
            //}

            return result;
        }

        public static IQueryable<T> GetListQueryableAllDirtyData<T>(IQueryable<T> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            //toptake = toptake == 0 ? 100 : toptake;
            var whereClause = GetWhereClause<T>(SearchFieldList);
            IQueryable<T> where = queryable;
            if (whereClause != null)
            {
                where = queryable.Where(whereClause).AsQueryable();
            }
            var orderBy = OrderByDynamic<T>(where, sortColumn, isascending).AsQueryable();
            //  var takeResult = orderBy.Take(toptake).AsQueryable();
            if (toptake != 0)
            {
                orderBy = orderBy.Take(toptake).AsQueryable();
                // queryable = queryable.Take(toptake);
            }
            return orderBy;
        }
        public static async Task<IEnumerable<T>> GetListAsyncAllDirtyData<T>(IQueryable<T> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            SearchFieldList.Add(SetActiveboolTrueParameter<T>());
            //toptake = toptake == 0 ? 100 : toptake;
            var whereClause = GetWhereClause<T>(SearchFieldList);
            IQueryable<T> where = queryable;
            if (whereClause != null)
            {
                where = queryable.Where(whereClause).AsQueryable();
            }
            var orderBy = OrderByDynamic<T>(where, sortColumn, isascending).AsQueryable();
            if (toptake != 0)
            {
                orderBy = orderBy.Take(toptake).AsQueryable();
                // queryable = queryable.Take(toptake);
            }


            //var takeResult = orderBy.Take(toptake).AsQueryable();
            return await orderBy.ToListAsync();
        }
        public static IEnumerable<T> GetListAllDirtyData<T>(IQueryable<T> queryable, List<SearchField> SearchFieldList, string sortColumn, bool isascending, int toptake) where T : class
        {
            SearchFieldList.Add(SetActiveboolTrueParameter<T>());

            //toptake = toptake == 0 ? 100 : toptake;
            var whereClause = GetWhereClause<T>(SearchFieldList);
            IQueryable<T> where = queryable;
            if (whereClause != null)
            {
                where = queryable.Where(whereClause).AsQueryable();
            }
            var orderBy = OrderByDynamic<T>(where, sortColumn, isascending).AsQueryable();
            // var takeResult = orderBy.Take(toptake).AsQueryable();
            if (toptake != 0)
            {
                orderBy = orderBy.Take(toptake).AsQueryable();
                // queryable = queryable.Take(toptake);
            }

            return orderBy.ToList();
        }
        public static List<PropertyInfo> PropertyColNotNull<T>(T entity) where T : class
        {
            string _nulltype = "nullable`1";
            string _fullname = string.Empty;
            List<PropertyInfo> resultAwal = new List<PropertyInfo>();
            List<PropertyInfo> result = new List<PropertyInfo>();
            resultAwal = (from sa in entity.GetType().GetProperties().AsQueryable() where sa.GetValue(entity) != null select sa).ToList();
            foreach (PropertyInfo property in resultAwal)
            {
                string _Fieldtype = property.PropertyType.Name.ToLower();
                string myfield = property.Name.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty);
                if (_Fieldtype == _nulltype)
                {
                    _fullname = property.PropertyType.FullName.ToLower().Split(',')[0].ToString();
                    _Fieldtype = _fullname.Replace("system." + _nulltype + "[[", string.Empty).Replace("system.", string.Empty);

                }
                if (_Fieldtype != "datetime" & myfield != "activebool" & myfield != "boolactive" & myfield != "insertby" & myfield != "insertbyid")
                {
                    result.Add(property);
                }
                if(_Fieldtype =="datetime" &(myfield =="updatedate" || myfield=="updatetime"))
                {
                    if(property.CanWrite)
                    {
                        property.SetValue(entity, (object)DateTime.Now);
                    }
                    result.Add(property);

                }
            }
            return result;
        }
        private static List<PropertyInfo> PropertyColNotNullDatetime<T>(T entity) where T : class
        {
            string _nulltype = "nullable`1";
            string _fullname = string.Empty;
            List<PropertyInfo> resultAwal = new List<PropertyInfo>();
            List<PropertyInfo> result = new List<PropertyInfo>();
            resultAwal = (from sa in entity.GetType().GetProperties().AsQueryable() where sa.GetValue(entity) != null select sa).ToList();
            foreach (PropertyInfo property in resultAwal)
            {
                string _Fieldtype = property.PropertyType.Name.ToLower();
                if (_Fieldtype == _nulltype)
                {
                    _fullname = property.PropertyType.FullName.ToLower().Split(',')[0].ToString();
                    _Fieldtype = _fullname.Replace("system." + _nulltype + "[[", string.Empty).Replace("system.", string.Empty);

                }
                if (_Fieldtype == "datetime")
                {
                    string myfield = string.Empty;
                    myfield = property.Name.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty);
                    if (myfield != "insertdate" & myfield != "inserttime")
                    {
                        result.Add(property);
                    }
                }

            }
            return result;
        }
        private static List<PropertyInfo> PropertyColNotNullActiveBool<T>(T entity) where T : class
        {
            string _fullname = string.Empty;
            List<PropertyInfo> resultAwal = new List<PropertyInfo>();
            List<PropertyInfo> result = new List<PropertyInfo>();
            resultAwal = (from sa in entity.GetType().GetProperties().AsQueryable() where sa.GetValue(entity) != null select sa).ToList();
            foreach (PropertyInfo property in resultAwal)
            {
                string myfield = string.Empty;
                myfield = property.Name.Trim().ToLower().Replace("_", "").Replace("<", string.Empty).Replace(">k__BackingField", string.Empty);
                if (myfield != "activebool" & myfield != "boolactive")
                {
                    result.Add(property);
                }
            }
            return result;
        }
        public static List<PropertyInfo> PropertyColNull<T>(T entity) where T : class
        {
            
            string _fullname = string.Empty;
            List<PropertyInfo> resultAwal = new List<PropertyInfo>();
            List<PropertyInfo> result = new List<PropertyInfo>();
            resultAwal = (from sa in entity.GetType().GetProperties().AsQueryable() where sa.GetValue(entity) == null select sa).ToList();
            foreach (PropertyInfo property in resultAwal)
            {
                result.Add(property);
            }
            var cekdatetime = PropertyColNotNullDatetime<T>(entity);
            foreach (PropertyInfo property in cekdatetime)
            {
                result.Add(property);
            }
            var cekActivbool = PropertyColNotNullActiveBool<T>(entity);
            foreach (PropertyInfo property in cekActivbool)
            {
                result.Add(property);
            }
            return result;
        }
        public static T PropertyColDefaultValue<T>(T entity) where T : class
        {
            string _nulltype = "nullable`1";
            List<PropertyInfo> listNull = new List<PropertyInfo>();
            listNull = PropertyColNull<T>(entity);
            string _Fieldtype = string.Empty;
            string _fullname = string.Empty;
            object defaultValue = null;
            bool isnullData = false;
            foreach (PropertyInfo property in listNull)
            {
                isnullData = false;
                _Fieldtype = property.PropertyType.Name.ToLower();
                if (_Fieldtype == _nulltype)
                {
                    _fullname = property.PropertyType.FullName.ToLower().Split(',')[0].ToString();
                    _Fieldtype = _fullname.Replace("system." + _nulltype + "[[", string.Empty).Replace("system.", string.Empty);
                    isnullData = true;

                }
                if (!isnullData)
                {
                    if (_Fieldtype == "int32")
                    {
                        defaultValue = (Int32)0;
                    }
                    else if (_Fieldtype == "int16")
                    {
                        defaultValue = (Int16)0;
                    }
                    else if (_Fieldtype == "int64")
                    {
                        defaultValue = (Int64)0;
                    }
                    else if (_Fieldtype == "single")
                    {
                        defaultValue = (Single)0;
                    }
                    else if (_Fieldtype == "double")
                    {
                        defaultValue = (double)0;
                    }
                    else if (_Fieldtype == "float")
                    {
                        defaultValue = (float)0;
                    }
                    else if (_Fieldtype == "char")
                    {
                        defaultValue = "";
                    }
                    else if (_Fieldtype == "boolean")
                    {
                        defaultValue = false;
                    }
                    else if (_Fieldtype == "string")
                    {
                        defaultValue = "";
                    }
                    else if (_Fieldtype == "guid")
                    {
                        defaultValue = new Guid();
                    }
                    else if (_Fieldtype == "decimal")
                    {
                        defaultValue = (decimal)0;
                    }
                    else if (_Fieldtype == "byte")
                    {
                        defaultValue = (byte)0;
                    }
                    else if (_Fieldtype == "datetime")
                    {
                        defaultValue = DateTime.Now;
                    }
                    else
                    {
                        defaultValue = null;
                    }
                }
                else
                {
                    if (_Fieldtype == "int32")
                    {
                        defaultValue = (Nullable<Int32>)0;
                    }
                    else if (_Fieldtype == "int16")
                    {
                        defaultValue = (Nullable<Int16>)0;
                    }
                    else if (_Fieldtype == "int64")
                    {
                        defaultValue = (Nullable<Int64>)0;
                    }
                    else if (_Fieldtype == "single")
                    {
                        defaultValue = (Nullable<Single>)0;
                    }
                    else if (_Fieldtype == "double")
                    {
                        defaultValue = (Nullable<double>)0;
                    }
                    else if (_Fieldtype == "float")
                    {
                        defaultValue = (Nullable<float>)0;
                    }

                    else if (_Fieldtype == "char")
                    {
                        defaultValue = "";
                    }
                    else if (_Fieldtype == "boolean")
                    {
                        defaultValue = (Nullable<bool>)false;
                    }
                    else if (_Fieldtype == "string")
                    {
                        defaultValue = "";
                    }
                    else if (_Fieldtype == "guid")
                    {
                        defaultValue = (Nullable<Guid>)new Guid();
                    }
                    else if (_Fieldtype == "decimal")
                    {
                        defaultValue = (Nullable<decimal>)0;
                    }
                    else if (_Fieldtype == "byte")
                    {
                        defaultValue = (Nullable<byte>)0;
                    }
                    else if (_Fieldtype == "datetime")
                    {
                        defaultValue = (Nullable<DateTime>)DateTime.Now;
                    }
                    else
                    {
                        defaultValue = null;
                    }

                }
                GenHelperEF.Getinstance.SetColValue<T>(entity, property.Name, defaultValue);
            }
            return entity;
        }
    }
    //public class SearchField
    //{
    //    public string Name { get; set; }
    //    public string Value1 { get; set; }
    //    public string Operator { get; set; }
    //}
    //public class SearchFieldType
    //{
    //    public string Name { get; set; }
    //    public string Value1 { get; set; }
    //    public string Operator { get; set; }
    //    public Type FieldType { get; set; }
    //}
    //public class ParamSearchField
    //{
    //    public string TabName { get; set; }
    //    public int TopTake { get; set; }
    //    public string SortColumn { get; set; }
    //    public bool IsSortAscending { get; set; }
    //    public string EmpnoLogin { get; set; }
    //    public List<SearchField> ListSearchField { get; set; }
    //    public bool AllDataBool { get; set; }

    //}
    //public class ParamCalendar
    //{

    //    public short calTypeID { get; set; }
    //    public DateTime fromDate { get; set; }
    //    public DateTime toDate { get; set; }

    //}

}
