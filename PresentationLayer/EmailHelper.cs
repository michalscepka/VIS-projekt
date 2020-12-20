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

        private EmailHelper()
		{

		}

        /// <summary>
		/// Poslání emailů
		/// </summary>
        public void SendEmails()
		{

		}

        /// <summary>
		/// Poslání emailu
		/// </summary>
        public void SendEmail()
		{

		}
	}
}
