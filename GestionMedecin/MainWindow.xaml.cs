using Emoji.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionMedecin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddConsultation_Click(object sender, RoutedEventArgs e)
        {
            // Instanciez votre nouvelle fenêtre
            WinConsultation nouvelleFenetre = new WinConsultation();

            // Affichez la nouvelle fenêtre
            nouvelleFenetre.Show();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si tous les TextBox sont remplis
            if (string.IsNullOrWhiteSpace(tbIdSpe.Text) || string.IsNullOrWhiteSpace(tbNameSpe.Text) || string.IsNullOrWhiteSpace(tbDescriptionSpe.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter une nouvelle spécialité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifier si l'ID respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbIdSpe.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("L'ID doit contenir uniquement des lettres et des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Vérifier si l'ID a 10 caractères
            if (tbIdSpe.Text.Length != 5)
            {
                MessageBox.Show("L'ID doit être composé de 5 caractères.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifier si le nom respecte la norme d'écriture
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbNameSpe.Text, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Le nom doit contenir uniquement des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Vérifier si la description respecte la norme d'écriture
            if (tbDescriptionSpe.Text.Length > 200)
            {
                MessageBox.Show("La description ne doit pas dépasser 200 caractères.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Créer un nouvel objet avec les informations de vos TextBox
            var nouvelElement = new { IdSpe = tbIdSpe.Text, NameSpe = tbNameSpe.Text, DescriptionSpe = tbDescriptionSpe.Text };

            // Vérifier si un élément avec le même ID existe déjà
            foreach (var item in dgSimple.Items)
            {
                if (((dynamic)item).IdSpe == tbIdSpe.Text)
                {
                    // Afficher une boîte de dialogue demandant à l'utilisateur s'il est sûr de vouloir remplacer l'élément existant
                    MessageBoxResult result = MessageBox.Show("Un élément avec cet ID existe déjà. Êtes-vous sûr de vouloir le remplacer ?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Supprimer l'élément existant
                        dgSimple.Items.Remove(item);

                        // Sortir de la boucle
                        break;
                    }
                    else
                    {
                        // Si l'utilisateur choisit "Non", sortir de la méthode sans ajouter ni supprimer aucun élément
                        return;
                    }
                }
            }

            // Ajouter l'objet à votre DataGrid
            dgSimple.Items.Add(nouvelElement);

            // Ajouter le nom de la nouvelle spécialité au ComboBox
            CbSpe.Items.Add(tbNameSpe.Text);

            // Trier les éléments dans le ComboBox par ordre alphabétique
            CbSpe.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));

            // Effacer les TextBox pour la prochaine entrée
            tbIdSpe.Clear();
            tbNameSpe.Clear();
            tbDescriptionSpe.Clear();
        }


        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dgSimple.SelectedItem != null)
            {
                // Obtenir l'élément sélectionné et le convertir en objet dynamique
                dynamic item = dgSimple.SelectedItem;

                // Trouver l'index de l'ancien nom dans le ComboBox
                int index = CbSpe.Items.IndexOf(item.NameSpe);
                if (index != -1)
                {
                    
                    // Supprimer l'ancien nom du ComboBox
                    CbSpe.Items.RemoveAt(index);

                    // Ajouter le nouveau nom au ComboBox
                    CbSpe.Items.Add(tbNameSpe.Text);

                }

                // Remplir les TextBox avec les informations de l'élément sélectionné
                tbIdSpe.Text = item.IdSpe;
                tbNameSpe.Text = item.NameSpe;
                tbDescriptionSpe.Text = item.DescriptionSpe;

                // Rafraîchir la DataGrid
                dgSimple.Items.Refresh();
            }
            // Parcourir tous les éléments du ComboBox
            for (int i = CbSpe.Items.Count - 1; i >= 0; i--)
            {
                // Si l'élément est vide, le supprimer
                if (string.IsNullOrWhiteSpace(CbSpe.Items[i].ToString()))
                {
                    CbSpe.Items.RemoveAt(i);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dgSimple.SelectedItem != null)
            {
                // Afficher une boîte de dialogue de confirmation avant de supprimer l'élément
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cette spécialité ?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Obtenir l'élément sélectionné et le convertir en objet dynamique
                    dynamic item = dgSimple.SelectedItem;

                    // Supprimer l'élément sélectionné de la DataGrid
                    dgSimple.Items.Remove(dgSimple.SelectedItem);

                    // Trouver l'index de l'ancien nom dans le ComboBox
                    int index = CbSpe.Items.IndexOf(item.NameSpe);
                    if (index != -1)
                    {
                        // Supprimer l'ancien nom du ComboBox
                        CbSpe.Items.RemoveAt(index);
                    }
                }
            }
            // Parcourir tous les éléments du ComboBox
            for (int i = CbSpe.Items.Count - 1; i >= 0; i--)
            {
                // Si l'élément est vide, le supprimer
                if (string.IsNullOrWhiteSpace(CbSpe.Items[i].ToString()))
                {
                    CbSpe.Items.RemoveAt(i);
                }
            }

        }



        private void BtnAdd2_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
