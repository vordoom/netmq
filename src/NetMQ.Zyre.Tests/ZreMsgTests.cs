using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NetMQ;
using NetMQ.Zyre;

namespace NetMQ.Zyre.Tests
{
	[TestFixture]
	public class ZreMsgTests
	{
		private void FillArray(byte[] array, byte value)
		{
			for	(int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
		}
	
		[Test]
		public void HelloTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.Hello;

				m.Hello.Sequence = 123;
    			m.Hello.Endpoint = "Life is short but Now lasts for ever";
				m.Hello.Groups = new List<string>();
				m.Hello.Groups.Add("Name: Brutus");
				m.Hello.Groups.Add("Age: 43");               
				m.Hello.Status = 123;
    			m.Hello.Name = "Life is short but Now lasts for ever";
				m.Hello.Headers = new Dictionary<string,string>();
				m.Hello.Headers.Add("Name", "Brutus");
				m.Hello.Headers.Add("Age", "43");              
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.Hello));
				Assert.That(m.Hello.Sequence, Is.EqualTo(123));                 
				Assert.That(m.Hello.Endpoint, Is.EqualTo("Life is short but Now lasts for ever"));                         
				Assert.That(m.Hello.Groups.Count, Is.EqualTo(2));
				Assert.That(m.Hello.Groups[0], Is.EqualTo("Name: Brutus"));                           
				Assert.That(m.Hello.Groups[1], Is.EqualTo("Age: 43"));                                           
				Assert.That(m.Hello.Status, Is.EqualTo(123));                   
				Assert.That(m.Hello.Name, Is.EqualTo("Life is short but Now lasts for ever"));                             
				Assert.That(m.Hello.Headers.Count, Is.EqualTo(2));
				Assert.That(m.Hello.Headers["Name"], Is.EqualTo("Brutus"));                          
				Assert.That(m.Hello.Headers["Age"], Is.EqualTo("43"));                                          
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	
		[Test]
		public void WhisperTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.Whisper;

				m.Whisper.Sequence = 123;
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.Whisper));
				Assert.That(m.Whisper.Sequence, Is.EqualTo(123));               
				Assert.That(m.Whisper.Content.FrameCount, Is.EqualTo(1));                
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	
		[Test]
		public void ShoutTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.Shout;

				m.Shout.Sequence = 123;
    			m.Shout.Group = "Life is short but Now lasts for ever";
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.Shout));
				Assert.That(m.Shout.Sequence, Is.EqualTo(123));                 
				Assert.That(m.Shout.Group, Is.EqualTo("Life is short but Now lasts for ever"));                            
				Assert.That(m.Shout.Content.FrameCount, Is.EqualTo(1));                  
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	
		[Test]
		public void JoinTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.Join;

				m.Join.Sequence = 123;
    			m.Join.Group = "Life is short but Now lasts for ever";
				m.Join.Status = 123;
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.Join));
				Assert.That(m.Join.Sequence, Is.EqualTo(123));                  
				Assert.That(m.Join.Group, Is.EqualTo("Life is short but Now lasts for ever"));                             
				Assert.That(m.Join.Status, Is.EqualTo(123));                    
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	
		[Test]
		public void LeaveTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.Leave;

				m.Leave.Sequence = 123;
    			m.Leave.Group = "Life is short but Now lasts for ever";
				m.Leave.Status = 123;
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.Leave));
				Assert.That(m.Leave.Sequence, Is.EqualTo(123));                 
				Assert.That(m.Leave.Group, Is.EqualTo("Life is short but Now lasts for ever"));                            
				Assert.That(m.Leave.Status, Is.EqualTo(123));                   
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	
		[Test]
		public void PingTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.Ping;

				m.Ping.Sequence = 123;
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.Ping));
				Assert.That(m.Ping.Sequence, Is.EqualTo(123));                  
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	
		[Test]
		public void PingOkTest()
		{
			Action<ZreMsg> setMessage = m => 
			{
				m.Id = ZreMsg.MessageId.PingOk;

				m.PingOk.Sequence = 123;
			};

			Action<ZreMsg> checkMessage = m=> 
			{
				Assert.That(m.Id, Is.EqualTo(ZreMsg.MessageId.PingOk));
				Assert.That(m.PingOk.Sequence, Is.EqualTo(123));                
			};

			using (NetMQContext context = NetMQContext.Create())
			using (var client = context.CreateDealerSocket())
			using (var server = context.CreateRouterSocket())
			{
				server.Bind("inproc://zprototest");
				client.Connect("inproc://zprototest");

				ZreMsg clientMessage = new ZreMsg();
				ZreMsg serverMessage = new ZreMsg();

				for (int i=0; i < 2; i++)
				{
					// client send message to server
					setMessage(clientMessage);				
					clientMessage.Send(client);				
												
					// server receive the message
					serverMessage.Receive(server);
				
					// check that message received ok
					Assert.That(serverMessage.RoutingId, Is.Not.Null);					
					checkMessage(serverMessage);

					// reply to client, no need to set the message, using client data
					serverMessage.Send(server);

					// client receive the message
					clientMessage.Receive(client);
				
					// check that message received ok
					Assert.That(clientMessage.RoutingId, Is.Null);					
					checkMessage(clientMessage);
				}				
			}			
		}	
	}
}