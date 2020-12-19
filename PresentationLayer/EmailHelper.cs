using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer
{
	public class EmailHelper
	{
        private static readonly object m_LockObj = new object();
        private static EmailHelper m_Instance;

        public static EmailHelper Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new EmailHelper();
                }
            }
        }

        /// <summary>
        /// Objekt spravy zamestnancu
        /// </summary>
        private EmailHelper()
        {

        }

        public void SendEmails()
		{

		}

        public void SendEmail()
		{

		}
	}
}
