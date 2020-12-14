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

        //public ServerTests(Mock<GameManager> gameManager)
        //{

        //    var mockConnection = new Mock<HubConnection>();
        //}
        public ServerTests()
        {

        }
        [Fact]
        public void TestGameManager()
        {
            Assert.True(false);
        }
        //[Fact]
        //public void HubsAreMockableViaDynamic()
        //{
        //    bool sendCalled = false;
        //    ShotAndDashHub hub = new ShotAndDashHub();
        //    var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
        //    hub.Clients = mockClients.Object;
        //    dynamic all = new ExpandoObject();
        //    all.broadcastMessage = new Action<string, string>((name, message) => {
        //        sendCalled = true;
        //    });
        //    mockClients.Setup(m => m.All).Returns((ExpandoObject)all);
        //    hub.Send("TestUser", "TestMessage");
        //    Assert.True(sendCalled);
        //}
    }
}
