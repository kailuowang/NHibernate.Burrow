using System;
using System.Configuration;

namespace NHibernate.Burrow.Impl
{
    public class ConversationExpirationCheckerByTimeout : IConversationExpirationChecker
    {
        private TimeSpan cleanUpTimeSpan;
        private TimeSpan timeout;

        public ConversationExpirationCheckerByTimeout()
        {
            IBurrowConfig cfg = new BurrowFramework().BurrowEnvironment.Configuration;
            int timeoutMinutes = cfg.ConversationTimeOut;
            if (timeoutMinutes < 1)
            {
                throw new ConfigurationErrorsException("ConversationTimeOut must be greater than 1");
            }

            timeout = TimeSpan.FromMinutes(timeoutMinutes);
            int freq = cfg.ConversationCleanupFrequency;
            if (freq < 1)
            {
                throw new ConfigurationErrorsException("ConversationCleanupFrequency must be greater than 1");
            }
            cleanUpTimeSpan = new TimeSpan(0, timeoutMinutes * freq, 0);
        }

        #region IConversationExpirationChecker Members

        public bool IsConversationExpired(IConversation c)
        {
            return (((AbstractConversation) c).LastVisit + timeout) < DateTime.Now;
        }

        public TimeSpan CleanUpTimeSpan
        {
            get { return cleanUpTimeSpan; }
        }

        #endregion
    }
}