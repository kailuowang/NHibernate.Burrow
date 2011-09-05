using System.Collections.Generic;
using System.Configuration;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;
using NHibernate.Burrow.Util;
using NHibernate.Cfg;

namespace NHibernate.Burrow.Configuration
{
    /// <summary>
    /// Root Section for NHibernate.Burrow Configuration
    /// </summary>
    public class NHibernateBurrowCfgSection : ConfigurationSection, IBurrowConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SectionName = "NHibernate.Burrow";

        private readonly IDictionary<string, object> savedSettings = new Dictionary<string, object>();
        private IConfigurator configurator;
        private IList<IPersistenceUnitCfg> pucs;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Declare a collection element represented 
        /// in the configuration file by the sub-section
        /// <![CDATA[
        /// <persistenceUnits> <add .../> </persistenceUnits> 
        /// Note: the "IsDefaultCollection = false" 
        /// instructs the .NET Framework to build a nested 
        /// section like <persistenceUnits> ...</persistenceUnits>.
        /// ]]>
        /// </remarks>
        [ConfigurationProperty("persistenceUnits", IsDefaultCollection = false)]
        public PersistenceUnitElementCollection PersistenceUnits
        {
            get
            {
                PersistenceUnitElementCollection persistenceUnits =
                    (PersistenceUnitElementCollection) base["persistenceUnits"];
                return persistenceUnits;
            }
        }

        #region IBurrowConfig Members

        public ICollection<IPersistenceUnitCfg> PersistenceUnitCfgs
        {
            get { return pucs; }
        }

        ///<summary>
        /// The converstaion timout minutes
        ///</summary>
        [ConfigurationProperty("conversationTimeOut", DefaultValue = "30", IsRequired = false, IsKey = false)]
        public int ConversationTimeOut
        {
            get { return (int) Get("conversationTimeOut"); }
            set { Set("conversationTimeOut", value); }
        }

        ///<summary>
        /// The conversation clean up frequency,
        ///  for how many times of conversation Timeout,
        ///  the conversation pool clean up its timeoutted converstaions.
        ///  must be greater than 1
        ///</summary>
        [ConfigurationProperty("conversationCleanupFrequency", DefaultValue = "4", IsRequired = false, IsKey = false)]
        public int ConversationCleanupFrequency
        {
            get { return (int) Get("conversationCleanupFrequency"); }
            set { Set("conversationCleanupFrequency", value); }
        }

        ///<summary>
        /// The conversation clean up frequency,
        ///  for how many times of conversation Timeout,
        ///  the conversation pool clean up its timeoutted converstaions.
        ///  must be greater than 1
        ///</summary>
        [ConfigurationProperty("conversationExpirationChecker", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string ConversationExpirationChecker
        {
            get { return (string) Get("conversationExpirationChecker"); }
            set { Set("conversationExpirationChecker", value); }
        }

        /// <summary>
        /// for user to set a customer IWorkSpaceNameSniffer for WebUtil to use
        /// </summary>
        [ConfigurationProperty("workSpaceNameSniffer", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string WorkSpaceNameSniffer
        {
            get { return (string) Get("workSpaceNameSniffer"); }
            set { Set("workSpaceNameSniffer", value); }
        }

        /// <summary>
        /// for user to set a customer IConfigurator to config everything before environment initiates
        /// </summary>
        [ConfigurationProperty("customConfigurator", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string CustomConfigurator
        {
            get { return (string) Get("customConfigurator"); }
            set { Set("customConfigurator", value); }
        }


        ///<summary>
        /// whether the transaction is manually managed by client    
        ///</summary>
        [ConfigurationProperty("manualTransactionManagement", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool ManualTransactionManagement
        {
            get { return (bool) Get("manualTransactionManagement"); }
            set { Set("manualTransactionManagement", value); }
        }


        /// <summary>
        /// Get the DBConnectionString for the DB where <paramref name="entityType"/> is persistent in
        /// </summary>
        /// <returns></returns>
        public string DBConnectionString(System.Type entityType)
        {
            return
                PersistenceUnitRepo.Instance.GetPU(entityType).NHConfiguration.Properties[Environment.ConnectionString];
        }

        public IConfigurator Configurator
        {
            get
            {
                if (configurator != null)
                    return configurator;
                if (string.IsNullOrEmpty(CustomConfigurator))
                    return null;
                return InstanceLoader.Load<IConfigurator>(CustomConfigurator);
            }
            set { configurator = value; }
        }

        #endregion

        private void Set(string key, object value)
        {
            new Util().CheckCanChangeCfg();
            savedSettings[key] = value;
        }

        private object Get(string key)
        {
            if (savedSettings.ContainsKey(key))
            {
                return savedSettings[key];
            }
            else
            {
                return this[key];
            }
        }

        /// <summary>
        /// Get the instance from the current application's config file
        /// </summary>
        /// <returns></returns>
        public static NHibernateBurrowCfgSection CreateInstance()
        {
            NHibernateBurrowCfgSection section =
                ConfigurationManager.GetSection(SectionName) as NHibernateBurrowCfgSection;
            if (section == null)
            {
                throw new GeneralException("Section \"" + SectionName + "\" is not found");
            }
            section.pucs = new List<IPersistenceUnitCfg>();
            foreach (PersistenceUnitElement pue in section.PersistenceUnits)
            {
                section.pucs.Add(pue);
            }
            return section;
        }
    }
}