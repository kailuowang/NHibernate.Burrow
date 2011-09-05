using System;
using System.Reflection;

namespace NHibernate.Burrow.Util
{
    /// <summary>
    /// A generic helper loader for loading persistant object by Id
    /// </summary>
    public class EntityLoader
    {
        private static readonly EntityLoader instance = new EntityLoader();

        /// <summary>
        /// a instance of the loader
        /// </summary>
        public static EntityLoader Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Load(System.Type t, object id)
        {
            return GetSession(t).Get(t, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(System.Type t, object id)
        {
            return GetSession(t).Get(t, id);
        }

        public object GetId(object o)
        {
            if (o == null)
            {
                return null;
            }
        	PropertyInfo pi = null;
			try {
        		 pi = o.GetType().GetProperty("Id");
        	}
        	catch ( AmbiguousMatchException e) {
        		 
        	}
			if (pi != null)
            {
                return pi.GetValue(o, null);
            }
            else
            {
                return GetSession(o.GetType()).GetIdentifier(o);
            }
        }

        private static ISession GetSession(System.Type t)
        {
            return new BurrowFramework().GetSession(t);
        }
    }
}