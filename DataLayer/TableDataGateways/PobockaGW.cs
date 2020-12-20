using DTO.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DataLayer.TableDataGateways
{
    /// <summary>
    /// Pomocná třída pro zapouzdření seznamu poboček
    /// </summary>
    [Serializable]
    [XmlRoot("PobockyStorage")]
    public class PobockyStorage
    {
        public List<PobockaDTO> Pobocky { get; set; }
    }

    public class PobockaGW
	{
        private static readonly object m_LockObj = new object();
        private static PobockaGW m_Instance;
        private readonly string path = @"C:\Users\Michal\Dropbox\School\5_semestr\VIS\Projekt\PujcovnaAutomobiluIS\Uloziste\Pobocky.xml";

        private PobockaGW()
		{

		}

        public static PobockaGW Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new PobockaGW();
                }
            }
        }

        /// <summary>
        /// Uložení všech poboček do uložiště
        /// </summary>
        /// <param name="pobockyToSave">Seznam všech poboček k uložení</param>
        /// <param name="msgErr">Chybové hlášení v případě chyby</param>
        /// <returns>True: operace se povedla, False: operace se nepovedla</returns>
        public bool Save(List<PobockaDTO> pobockyToSave, out string msgErr)
        {
            msgErr = string.Empty;
			var pobockyStorage = new PobockyStorage
			{
				Pobocky = pobockyToSave
			};

			XmlSerializer serializer = new XmlSerializer(typeof(PobockyStorage));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                try
                {
                    serializer.Serialize(fs, pobockyStorage);
                }
                catch (Exception e)
                {
                    msgErr = e.Message;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Načtení všech poboček z úložiště
        /// </summary>
        /// <param name="pobocky">Seznam poboček</param>
        /// <param name="msgErr">Chybové hlášení</param>
        /// <returns>True: operace se povedla, False: nastala chyba</returns>
        public bool Load(out List<PobockaDTO> pobocky, out string msgErr)
        {
            pobocky = null;
            msgErr = string.Empty;
            if (!File.Exists(path))
                return true;

            XmlSerializer serializer = new XmlSerializer(typeof(PobockyStorage));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                try
                {
                    var pobockyStorage = (PobockyStorage)serializer.Deserialize(fs);
                    pobocky = pobockyStorage.Pobocky;
                }
                catch (Exception e)
                {
                    msgErr = e.Message;
                    return false;
                }
            }

            return true;
        }
    }
}
