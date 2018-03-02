﻿using System;
using Interactr.Model;
using NUnit.Framework;
using System.Collections.Generic;
namespace Interactr.Tests.Model
{
    [TestFixture]
    public class LabelTests
    {
        [Test]
        public void Test_1()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("A;B"));
           
        }
        [Test]
        public void Test_2()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("a;b"));

        }
        [Test]
        public void Test_3()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("A;b"));

        }
        [Test]
        public void Test_4()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(true, p.isValidLabel("a;B"));

        }
        [Test]
        public void Test_5()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel(";b"));

        }
        [Test]
        public void Test_6()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(true, p.isValidLabel("a;Bc"));
        }
        [Test]
        public void Test_7()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("A;BC"));
        }
        [Test]
        public void Test_8()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel(";"));
        }
        [Test]
        public void Test_9()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("a;"));
        }
        [Test]
        public void Test_10()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("A;"));
        }
        [Test]
        public void Test_11()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(true, p.isValidLabel("test;Test"));
        }
        [Test]
        public void Test_12()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("Hey;Hey"));
        }
        [Test]
        public void Test_13()
        {
            Party p = new Party(Party.PartyType.Object, "a");
            Assert.AreEqual(false, p.isValidLabel("hi;hi"));
        }
    }
}
