﻿using Interactr.View;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using System.Reactive.Concurrency;
using System;
using Interactr.View.Framework;
using Interactr.Reactive;

namespace Interactr.Tests.View
{
    class CommunicationDiagramViewTests
    {

        private TestScheduler _scheduler;
        private TestableCommunicationDiagramView _communicationDiagramView;

        [SetUp]
        public void BeforeEach()
        {
            _scheduler = new TestScheduler();
            _communicationDiagramView = new TestableCommunicationDiagramView();
            _communicationDiagramView.ViewModel = new Interactr.ViewModel.CommunicationDiagramViewModel(new Interactr.Model.Diagram());
        }

        [Test]
        public void VisibilityChangeTest()
        {
            int scheduleTicks = 10;
            _scheduler.Schedule(TimeSpan.FromTicks(scheduleTicks), () => _communicationDiagramView.ViewModel.IsVisible = true);

            var expected = new[]
            {
                ReactiveTest.OnNext(1, false),
                ReactiveTest.OnNext(10, true)
            };

            // Check if the visibility is as expected.
            var actual = _scheduler.Start(() => _communicationDiagramView.IsVisibleChanged, 0, 0, 1000).Messages;

            ReactiveAssert.AreElementsEqual(expected, actual);
        }


        [Test]
        public void AddPartyOnDoubleMouseClick()
        {
           _communicationDiagramView.RunOnMouseEvent(new MouseEventData(Window.MouseEvent.MOUSE_CLICKED,new Point(0,0),2)));
            Assert.AreEqual(_communicationDiagramView.PartyViews.Count, 1);
        }

        [Test]
        public void 

        private class TestableCommunicationDiagramView : CommunicationDiagramView
        {

            public IReadOnlyReactiveList<PartyView> PartyViews => partyViews;

            public bool RunOnMouseEvent(MouseEventData e)
            {
                return OnMouseEvent(e);
            }

        }
    }
}

