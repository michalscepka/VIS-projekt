using BusinessLayer.BO;
using BusinessLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer
{
	public class UzivateleHelper
	{
        private static readonly object m_LockObj = new object();
        private static UzivateleHelper m_Instance;

        public static UzivateleHelper Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new UzivateleHelper();
                }
            }
        }

        /// <summary>
        /// Objekt spravy zamestnancu
        /// </summary>
        private UzivateleHelper()
        {

        }

        public Zakaznik GetPrihlasenyZakaznik()
		{
			return SpravaZakazniku.Instance.FindZakaznik(1);
		}
	}
}
