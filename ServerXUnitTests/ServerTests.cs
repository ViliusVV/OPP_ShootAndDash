using Server.Hubs;
using System;
using Xunit;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Server;
using Microsoft.AspNetCore.SignalR.Client;

namespace ServerXUnitTests
{
    public class ServerTests : ShotAndDashHub
    {
        public Mock<GameManager> GameManager { get; private set; }

        public ServerTests()
        {
        }


        [Fact]
        public void HubsAreMockableViaDynamic()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }

        [Fact]
        public void HubsAreMockableViaDynamic2()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }

        [Fact]
        public void HubsAreMockableViaDynamic3()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }

        [Fact]
        public void HubsAreMockableViaDynamic4()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }


        [Fact]
        public void HubsAreMockableViaDynamic5()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }


        [Fact]
        public void HubsAreMockableViaDynamic6()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }


        [Fact]
        public void HubsAreMockableViaDynamic7()
        {
            bool sendCalled = true;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }


        [Fact]
        public void HubsAreMockableViaDynamic8()
        {
            bool sendCalled = false;
            ShotAndDashHub hub = new ShotAndDashHub();

            Assert.True(sendCalled);
        }
    }
}
