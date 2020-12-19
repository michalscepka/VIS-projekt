﻿using BusinessLayer.BO;
using BusinessLayer.Controllers;
using PresentationLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopApp
{
	/// <summary>
	/// Interaction logic for VyraditVozidlo.xaml
	/// </summary>
	public partial class VyraditVozidlo : UserControl
	{
		private CollectionView view;
		private Vozidlo VozidloNaVyrazeni;

		public VyraditVozidlo()
		{
			InitializeComponent();
			Create();
		}

		private void Create()
		{
			vyraditButton.IsEnabled = true;
			nahraditButton.IsEnabled = false;
			hintLabel.Visibility = Visibility.Hidden;
			VozidloNaVyrazeni = null;

			listView.ItemsSource = SpravaVozidel.Instance.SeznamVozidel;

			view = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
			view.Filter = VehiclesFilter;
			CollectionViewSource.GetDefaultView(listView.ItemsSource).Refresh();
			listView.SelectedIndex = -1;
		}

		private void Vyradit_Click(object sender, RoutedEventArgs e)
		{
			if (listView.SelectedItem == null)
				return;

			searchTextBox.Text = null;

			Vozidlo selectedVozidlo = (listView.SelectedItem as Vozidlo);

			//kontrola jestli se na vozdilo nevazou pozdejsi objednavky
			if (MuzeVyradit(selectedVozidlo))
			{
				VyraditZvoleneVozidlo(selectedVozidlo);
				Create();
			}
			else
			{
				VozidloNaVyrazeni = selectedVozidlo;
				vyraditButton.IsEnabled = false;
				nahraditButton.IsEnabled = true;
				hintLabel.Content = "Zvolte vozidlo, kterým ho chcete nahradit v objednávkách.";
				hintLabel.Visibility = Visibility.Visible;
				CollectionViewSource.GetDefaultView(listView.ItemsSource).Refresh();
				listView.SelectedIndex = -1;
			}
		}

		private void Nahradit_Click(object sender, RoutedEventArgs e)
		{
			if (listView.SelectedItem == null)
				return;

			Vozidlo selectedVozidlo = (listView.SelectedItem as Vozidlo);

			//check jestli nema vybrane auto objednavky ve stejny cas jako odebirane

			List<Rezervace> rezervaceProNahradniVozidlo = new List<Rezervace>();
			List<Rezervace> rezervaceProVyrazovaneVozidlo = new List<Rezervace>();

			foreach (Rezervace rezervace in SpravaRezervaci.Instance.SeznamRezervaci)
			{
				if (rezervace.Vozidlo.Id == selectedVozidlo.Id)
					rezervaceProNahradniVozidlo.Add(rezervace);
				else if (rezervace.Vozidlo.Id == VozidloNaVyrazeni.Id)
					rezervaceProVyrazovaneVozidlo.Add(rezervace);
			}

			for (int i = 0; i < rezervaceProNahradniVozidlo.Count; i++)
			{
				for(int j = 0; j < rezervaceProVyrazovaneVozidlo.Count; j++)
				{
					//TODO nepokryva to objednavky, ktere trvaji vice dni
					if(rezervaceProNahradniVozidlo[i].DatumZacatkuRezervace == rezervaceProVyrazovaneVozidlo[j].DatumZacatkuRezervace ||
						rezervaceProNahradniVozidlo[i].DatumKonceRezervace == rezervaceProVyrazovaneVozidlo[j].DatumKonceRezervace)
					{
						hintLabel.Content = "Kolize v objednavkach.";
						return;
					}
				}
			}

			foreach (Rezervace rezervace in SpravaRezervaci.Instance.SeznamRezervaci)
			{
				if (rezervace.Vozidlo.Id == VozidloNaVyrazeni.Id && rezervace.DatumZacatkuRezervace >= DateTime.Today)
				{
					rezervace.Vozidlo.Id = selectedVozidlo.Id;
					SpravaRezervaci.Instance.UpdateRezervace(rezervace);
				}
			}

			VyraditZvoleneVozidlo(VozidloNaVyrazeni);
			Create();
		}

		private bool MuzeVyradit(Vozidlo selectedVozidlo)
		{
			foreach (Rezervace rezervace in SpravaRezervaci.Instance.SeznamRezervaci)
				if (rezervace.Vozidlo.Id == selectedVozidlo.Id && rezervace.DatumZacatkuRezervace >= DateTime.Today)
					return false;
			return true;
		}

		private void VyraditZvoleneVozidlo(Vozidlo selectedVozidlo)
		{
			selectedVozidlo.Aktivni = false;
			SpravaVozidel.Instance.UpdateVozidlo(selectedVozidlo);
			EmailHelper.Instance.SendEmails();
		}

		private bool VehiclesFilter(object item)
		{
			Vozidlo vozidlo = item as Vozidlo;
			if (string.IsNullOrEmpty(searchTextBox.Text))
				if (VozidloNaVyrazeni == null)
					return vozidlo.Aktivni;
				else
					return vozidlo.Aktivni && vozidlo.Id != VozidloNaVyrazeni.Id;
			else
				return (vozidlo.Znacka.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0 || 
					vozidlo.Model.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0) && 
					vozidlo.Aktivni;
		}

		private void Hledat_Click(object sender, RoutedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(listView.ItemsSource).Refresh();
		}
	}
}