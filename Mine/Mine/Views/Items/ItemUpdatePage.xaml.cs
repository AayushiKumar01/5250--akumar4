using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Mine.Models;
using Mine.ViewModels;

namespace Mine.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemUpdatePage : ContentPage
    {
        public ItemModel Item { get; set; }

        /// <summary>
        /// Constructor that takes a viewModel
        /// </summary>
        /// <param name="viewModel"></param>
        public ItemUpdatePage(ItemReadViewModel viewModel)
        {
            InitializeComponent();
            Item = viewModel.Item;

            BindingContext = this;
        }
        /// <summary>
        /// Update selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void UpdateItem(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "UpdateItem", Item);
            await Navigation.PopModalAsync();
        }

        /// <summary>
        /// Cancel Update operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        /// <summary>
        /// Update the Display Value when the Stepper changes
        ///</summary>
        void Value_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ValueValue.Text = string.Format("{0}", e.NewValue);
        }
    }
}