using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

public class YesNoType : IUserType
{
    public bool IsMutable
    {
        get { return false; }
    }

    public Type ReturnedType
    {
        get { return typeof(YesNoType); }
    }

    public SqlType[] SqlTypes
    {
        get { return new[]{NHibernateUtil.String.SqlType}; }
    }

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        var obj = NHibernateUtil.String.NullSafeGet(rs, names[0], session);

        if(obj == null ) return null;

        var yesNo = (string) obj;

        if( yesNo != "YES" && yesNo != "NO" )
            throw new Exception($"Expected data to be 'YES' or 'NO' but was '{yesNo}'.");

        return yesNo == "YES";
    }

    public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
        if(value == null)
        {
            ((IDataParameter) cmd.Parameters[index]).Value = DBNull.Value;
        }
        else
        {
            var yes = (bool) value;
            ((IDataParameter)cmd.Parameters[index]).Value = yes ? "YES" : "NO";
        }
    }

    public object DeepCopy(object value)
    {
        return value;
    }

    public object Replace(object original, object target, object owner)
    {
        return original;
    }

    public object Assemble(object cached, object owner)
    {
        return cached;
    }

    public object Disassemble(object value)
    {
        return value;
    }

    public new bool Equals(object x, object y)
    {
        if( ReferenceEquals(x,y) ) return true;

        if( x == null || y == null ) return false;

        return x.Equals(y);
    }

    public int GetHashCode(object x)
    {
        return x == null ? typeof(bool).GetHashCode() + 473 : x.GetHashCode();
    }
}