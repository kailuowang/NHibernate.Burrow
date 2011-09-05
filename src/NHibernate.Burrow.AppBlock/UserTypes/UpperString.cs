using System;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NHibernate.Burrow.AppBlock.UserTypes
{
    /// <summary>
    /// Convert the String to Upper when the object it's saved
    /// or when you get it from the base.
    /// </summary>
    public class UpperString : IUserType
    {
        #region IUserType Members

        /// <summary>
        /// Retrieve an instance of the mapped class from a Ado.Net resultset. 
        /// Implementors should handle possibility of null values. 
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="names"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            //treat for the posibility of null values
            string resultString = (string) NHibernateUtil.String.NullSafeGet(rs, names[0]);
            if (resultString != null)
            {
                return resultString.ToUpper();
            }
            return null;
        }

        /// <summary>
        /// Write an instance of the mapped class to a prepared statement. 
        /// Handle possibility of null values. 
        /// A multi-column type should be written to parameters starting from index. 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }

            value = ((String) value).ToUpper();

            NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        /// <summary>
        /// Return a deep copy of the persistent state, 
        /// stopping at entities and at collections. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object DeepCopy(object value)
        {
            if (value == null)
            {
                return null;
            }
            return string.Copy((String) value);
        }

        /// <summary>
        /// During merge, replace the existing (target) value in the entity we are 
        /// merging to with a new (original) value from the detached entity we are 
        /// merging. For immutable objects, or null values, it is safe to simply 
        /// return the first parameter. For mutable objects, it is safe to return a 
        /// copy of the first parameter. For objects with component values, it might 
        /// make sense to recursively replace component values. 
        /// </summary>
        /// <param name="original">the value from the detached entity being merged</param>
        /// <param name="target">the value in the managed entity</param>
        /// <param name="owner">the managed entity</param>
        /// <returns>Returns the first parameter because it is inmutable</returns>
        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        /// <summary>
        /// Reconstruct an object from the cacheable representation. 
        /// At the very least this method should perform a deep copy if the type is mutable. 
        /// (optional operation) 
        /// </summary>
        /// <param name="cached">the object to be cached</param>
        /// <param name="owner">the owner of the cached object</param>
        /// <returns>a reconstructed string from the cachable representation</returns>
        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        /// <summary>
        /// Transform the object into its cacheable representation. 
        /// At the very least this method should perform a deep copy if the type is mutable. 
        /// That may not be enough for some implementations, however; 
        /// for example, associations must be cached as identifier values. 
        /// (optional operation) 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        /// <summary>
        /// The SQL types for the columns mapped by this type. 
        /// In this case just a SQL Type will be returned:<seealso cref="DbType.String"/>
        /// </summary>
        public SqlType[] SqlTypes
        {
            get
            {
                SqlType[] types = new SqlType[1];
                types[0] = new SqlType(DbType.String);
                return types;
            }
        }

        /// <summary>
        /// The returned type is a <see cref="string"/>
        /// </summary>
        public System.Type ReturnedType
        {
            get { return typeof (String); }
        }

        /// <summary>
        /// The strings are not mutables.
        /// </summary>
        public bool IsMutable
        {
            get { return false; }
        }

        /// <summary>
        /// Compare two <see cref="string"/>
        /// </summary>
        /// <param name="x">string to compare 1</param>
        /// <param name="y">string to compare 2</param>
        /// <returns>If are equals or not</returns>
        public new bool Equals(object x, object y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Equals(y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        #endregion
    }
}