﻿using Interactr.Model;
using Interactr.Reactive;

namespace Interactr.ViewModel
{
    public class MainViewModel
    {
        public ReactiveList<DiagramEditorViewModel> Diagrams { get; } = new ReactiveArrayList<DiagramEditorViewModel>();

        public MainViewModel()
        {
        }

        public void CreateNewDiagram()
        {
            Diagrams.Add(new DiagramEditorViewModel(new Diagram()));
        }
    }
}