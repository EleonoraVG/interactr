﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Interactr.Model;
using Interactr.Reactive;
using Interactr.View.Controls;
using Interactr.View.Framework;
using Interactr.ViewModel;
using Interactr.Window;

namespace Interactr.View
{
    /// <summary>
    /// The view for the sequence diagram.
    /// </summary>
    public class SequenceDiagramView : AnchorPanel
    {
        #region ViewModel
        private readonly ReactiveProperty<SequenceDiagramViewModel> _viewModel = new ReactiveProperty<SequenceDiagramViewModel>();

        public SequenceDiagramViewModel ViewModel
        {
            get => _viewModel.Value;
            set => _viewModel.Value = value;
        }

        public IObservable<SequenceDiagramViewModel> ViewModelChanged => _viewModel.Changed;
        #endregion

        public SequenceDiagramView()
        {
            // Define the visibility of this view to be set to the visibility of the latest viewmodel assigned to this view.
            ViewModelChanged.ObserveNested(vm => vm.IsVisibleChanged)
                .Subscribe(isVisible => { this.IsVisible = isVisible; });

            StackPanel stackPanel = new StackPanel
            {
                StackOrientation = Orientation.Horizontal
            };
            Children.Add(stackPanel);

            // Create a list of party views based on the party viewmodel.
            ReactiveList<SequenceDiagramColumnView> columnViews = ViewModelChanged
                .Where(vm => vm != null)
                .Select(vm => vm.PartyViewModels)
                .CreateDerivedListBinding(partyVM => new SequenceDiagramColumnView(this, partyVM))
                .ResultList;

            // Automatically add and remove party views to Children.
            columnViews.OnAdd.Subscribe(e => stackPanel.Children.Insert(e.Index, e.Element));
            columnViews.OnDelete.Subscribe(e => stackPanel.Children.RemoveAt(e.Index));
        }
    }

    class SequenceDiagramColumnView : AnchorPanel
    {
        private readonly PartyView _partyView;
        private readonly LifeLineView _lifeLineView;

        public SequenceDiagramColumnView(SequenceDiagramView parent, PartyViewModel partyVM)
        {
            _partyView = new PartyView
            {
                ViewModel = partyVM
            };
            AnchorsProperty.SetValue(_partyView, Anchors.Left | Anchors.Top | Anchors.Right);
            this.Children.Add(_partyView);

            _lifeLineView = new LifeLineView();
            MarginsProperty.SetValue(_lifeLineView, new Margins(0, _partyView.PreferredHeight, 0, 0));
            this.Children.Add(_lifeLineView);
            parent.ViewModelChanged.Select(vm => vm.StackVM)
                .Subscribe(stackVM => _lifeLineView.ViewModel = stackVM.CreateLifeLineForParty(partyVM));
        }
    }
}