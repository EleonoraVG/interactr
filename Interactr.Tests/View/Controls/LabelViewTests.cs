﻿using System;
using System.Reactive.Concurrency;
using Interactr.View.Controls;
using Interactr.View.Framework;
using Interactr.Window;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace Interactr.Tests.View.Controls
{
    [TestFixture]
    public class LabelViewTests
    {
        private TestableLabelView _labelView;
        private TestScheduler _scheduler;

        [SetUp]
        public void BeforeEach()
        {
            _labelView = new TestableLabelView();
            _scheduler = new TestScheduler();
        }

        [Test]
        public void EscKeyFunctionalityEvent()
        {
            KeyEventData keyEventData = new KeyEventData(KeyEvent.KEY_RELEASED, KeyEvent.VK_ESCAPE, '\x1b');
            _labelView.CanLeaveEditMode = true;
            // set to true because the default is false
            _labelView.IsInEditMode = true;

            bool result = _labelView.RunOnKeyEvent(keyEventData);

            // Check if an action occurred.
            Assert.IsTrue(result);

            // Check if expected ESC action occurred.
            Assert.IsFalse(_labelView.IsInEditMode);
        }

        public class TestableLabelView : LabelView
        {
            // Count the number of times repaint is called in this labelView.
            public int RepaintCounter { get; private set; } = 0;

            public bool RunOnKeyEvent(KeyEventData keyEventData)
            {
                return OnKeyEvent(keyEventData);
            }
        }

        [Test]
        public void EscKeyFunctionalityNoEvent()
        {
            KeyEventData keyEventData = new KeyEventData(KeyEvent.KEY_RELEASED, -1, '\x1b');

            _labelView.CanLeaveEditMode = true;
            _labelView.IsInEditMode = true;

            bool result = _labelView.RunOnKeyEvent(keyEventData);

            // Check if an action occurred.
            Assert.IsFalse(result);

            // Check if expected ESC action occurred.
            Assert.IsTrue(_labelView.IsInEditMode);
        }

        [Test]
        public void EscKeyFunctionalityDontEnterEditMode()
        {
            KeyEventData keyEventData = new KeyEventData(KeyEvent.KEY_RELEASED, KeyEvent.VK_ESCAPE, '\x1b');
            _labelView.CanLeaveEditMode = true;

            bool result = _labelView.RunOnKeyEvent(keyEventData);

            // Check if an action occurred.
            Assert.IsTrue(result);

            // Check if expected ESC action occurred.
            Assert.IsFalse(_labelView.IsInEditMode);
        }

        [Test]
        public void MouseClickFunctionalityEventInLabel()
        {
            UIElement ui = new UIElement();
            LabelView labelview = new LabelView();
            ui.Children.Add(labelview);
            MouseEventData mouseEventData1 = new MouseEventData(MouseEvent.MOUSE_CLICKED, new Point(0, 0), 1);
            MouseEventData mouseEventData2 = new MouseEventData(MouseEvent.MOUSE_CLICKED, new Point(0, 0), 2);


            bool result1 = UIElement.HandleMouseEvent(labelview, mouseEventData1);
            bool result2 = UIElement.HandleMouseEvent(labelview, mouseEventData2);


            // Check if an action occurred.
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);

            // Check if expected mouse action occured.
            Assert.IsTrue(_labelView.IsInEditMode);
        }

        [Test]
        public void MouseClickFunctionalityEventOutsideLabel()
        {
            UIElement ui = new UIElement();
            LabelView labelview = new LabelView();
            labelview.Height = 2;
            labelview.Width = 2;
            ui.Children.Add(labelview);
            MouseEventData mouseEventData =
                new MouseEventData(MouseEvent.MOUSE_CLICKED, new Point(int.MaxValue, int.MaxValue), 1);

            bool result = UIElement.HandleMouseEvent(ui, mouseEventData);

            // Check if an action occurred.
            Assert.IsTrue(result);

            // Check if expected mouse action occured.
            Assert.IsFalse(_labelView.IsInEditMode);
        }

        [Test]
        public void MouseClickFunctionalityEventNoClick()
        {
            LabelView labelview = new LabelView();
            MouseEventData mouseEventData =
                new MouseEventData(MouseEvent.NOBUTTON, new Point(int.MaxValue, int.MaxValue), 1);

            bool result = UIElement.HandleMouseEvent(labelview, mouseEventData);

            // Check if an action occurred.
            Assert.IsFalse(result);

            // Check if expected mouse action occured.
            Assert.IsFalse(_labelView.IsInEditMode);
        }
    }

    [TestFixture]
    public class LabelViewObservablesTests
    {
        private LabelViewTests.TestableLabelView _labelView;
        private TestScheduler _scheduler;

        [SetUp]
        public void Before()
        {
            _labelView = new LabelViewTests.TestableLabelView();
            _scheduler = new TestScheduler();
        }

        [Test]
        public void LabelTextChangesWhenOneKeyTyped()
        {
            _labelView.IsInEditMode = true;
            ScheduleTypeTextInLabelView("TEST");
            var expected = new[]
            {
                ReactiveTest.OnNext(1, ""),
                ReactiveTest.OnNext(10, "T"),
                ReactiveTest.OnNext(20, "TE"),
                ReactiveTest.OnNext(30, "TES"),
                ReactiveTest.OnNext(40, "TEST")
            };
            var actual = _scheduler.Start(() => _labelView.TextChanged, 0, 0, 1000).Messages;

            ReactiveAssert.AreElementsEqual(expected, actual);
        }

        [Test]
        public void TestBackSpaceInLabelView()
        {
            _labelView.IsInEditMode = true;
            ScheduleTypeTextInLabelView("TEST\bA");
            var expected = new[]
            {
                ReactiveTest.OnNext(1, ""),
                ReactiveTest.OnNext(10, "T"),
                ReactiveTest.OnNext(20, "TE"),
                ReactiveTest.OnNext(30, "TES"),
                ReactiveTest.OnNext(40, "TEST"),
                ReactiveTest.OnNext(50, "TES"),
                ReactiveTest.OnNext(60, "TESA")
            };
            var actual = _scheduler.Start(() => _labelView.TextChanged, 0, 0, 1000).Messages;

            // Assertion
            ReactiveAssert.AreElementsEqual(expected, actual);
        }

        [Test]
        public void BackSpaceWithNoTextInLabelViewShouldNotChangeText()
        {
            ScheduleTypeTextInLabelView("\b");
            var expected = new[]
            {
                ReactiveTest.OnNext(1, "")
            };
            var actual = _scheduler.Start(() => _labelView.TextChanged, 0, 0, 1000).Messages;

            // Assertion
            ReactiveAssert.AreElementsEqual(expected, actual);
        }

        /// <summary>
        /// Helper function for faking the typing of characters.
        /// A character is typed every 10 ticks.
        /// </summary>
        /// <param name="text">The text to schedule type events for.</param>
        public void ScheduleTypeTextInLabelView(string text)
        {
            int scheduleTicks = 10;
            // Schedule to type a character in every 10 ticks.
            foreach (char character in text)
            {
                KeyEventData keyEventData =
                    new KeyEventData(KeyEvent.KEY_TYPED, KeyEvent.CHAR_UNDEFINED, character);
                _scheduler.Schedule(TimeSpan.FromTicks(scheduleTicks),
                    () => _labelView.RunOnKeyEvent(keyEventData));
                scheduleTicks += 10;
            }
        }
    }
}